using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace jsontocpp
{
    public static class Generator
    {
        public static int ClassCount = 0;
        public static string Indent(int Count)
        {
            return new string(' ', Count);
        }

        public static void WriteHeader(CppClass cls)
        {
            ClassCount++;

            // Class 
            string Line = $"class Class{ClassCount}\n" + "{\n";
            Line += Indent(4) + "private:\n";

            // Variables
            var enumerator = cls.Properties.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.Equals("string"))
                {
                    Line += Indent(8) + "std::string " + enumerator.Current.Key + ";" + "\n";
                }
                else if (enumerator.Current.Value.Equals("int"))
                {
                    Line += Indent(8) + "int " + enumerator.Current.Key + ";" + "\n";
                }
                else if (enumerator.Current.Value.Equals("bool"))
                {
                    Line += Indent(8) + "bool " + enumerator.Current.Key + ";" + "\n";
                }
            }
            Line += "\n" + Indent(4) + "public:\n";


            // Class constructor
            Line += Indent(8) + $"Class{ClassCount}(";

            foreach (var prop in cls.Properties)
            {
                if (prop.Value.Equals("std::string"))
                {
                    Line += "string " + prop.Key + ", ";
                }
                else if (prop.Value.Equals("int"))
                {
                    Line += "int " + prop.Key + ", ";
                }
                else if (prop.Value.Equals("bool"))
                {
                    Line += "bool " + prop.Key + ", ";
                }

                if (prop.Equals(cls.Properties.Last()))
                {
                    Line = Line.Remove(Line.Length - 2) + ");\n";
                }
            }

            // Destructor
            Line += Indent(8) + $"~Class{ClassCount}();\n";

            // Copy constructor
            Line += Indent(8) + $"Class{ClassCount}(const Class{ClassCount}&);\n";

            // Assignment operator
            Line += Indent(8) + $"Class{ClassCount}& operator=(const Class{ClassCount}&);\n\n";


            // Getters and setters
            foreach (var prop in cls.Properties)
            {
                if (prop.Value.Equals("string"))
                {
                    Line += Indent(8) + "std::string" + " get" + prop.Key + "();\n";
                    Line += Indent(8) + "void " + " set" + prop.Key + "(std::string s);\n";
                }
                else if (prop.Value.Equals("int"))
                {
                    Line += Indent(8) + "int" + " get" + prop.Key + "();\n";
                    Line += Indent(8) + "void " + " set" + prop.Key + "(int n);\n";
                }
                else if (prop.Value.Equals("bool"))
                {
                    Line += Indent(8) + "bool" + " get" + prop.Key + "();\n";
                    Line += Indent(8) + "void " + " set" + prop.Key + "(bool b);\n";
                }
            }


            Line += "};";
            System.IO.Directory.CreateDirectory("../../../output");
            File.WriteAllText($"../../../output/Class{ClassCount}.h", Line);
        }
        
        public static string GetClass(CppCbject obj, List<CppClass> Classes)
        {
            foreach (var cls in Classes)
            {
                if (obj.Variables.Keys.Intersect(cls.Properties.Keys).Any())
                {
                    int classNo = Classes.IndexOf(cls) + 1;
                    return classNo.ToString();
                }
               
            }
            return "";
        }
        public static void GenerateHeaders(List<CppClass> Classes)
        {
            foreach (var cls in Classes)
            {
                WriteHeader(cls);
            }
        }

        public static void GenerateMain(List<CppClass> Classes, List<CppCbject> Objects, Dictionary<string, List<string>> Arrays )
        {

            string Line = "";

            foreach (var obj in Objects)
            {
                string Class = $"Class{GetClass(obj, Classes)} ";
                Line += Class + obj.Name + "(";
                foreach (var variable in obj.Variables.Values)
                {
                    if (variable.type.Equals("bool") || variable.type.Equals("int"))
                    {
                        Line += $"{variable.value}, ";
                    }
                    else
                    {
                        Line += $"\"{variable.value}\", ";
                    }

                    if (variable == obj.Variables.Values.Last())
                    {
                        Line = Line.Remove(Line.Length - 2);
                    }

                }

                Line += ");\n";
            }
            Line += "\n";
            foreach (var array in Arrays)
            {
                Line += "std::vector<Class2> " + $"{array.Key};\n";
                foreach (var item in array.Value)
                {
                    Line += $"{array.Key}.push_back({item});\n";
                }
            }

        
            File.WriteAllText($"../../../output/main.cpp", Line);
        }

        public static void Generate(List<CppClass> Classes, List<CppCbject> Objects, Dictionary<string, List<string>> Arrays)
        {
            GenerateHeaders(Classes);
            GenerateMain(Classes, Objects, Arrays);
        }


    }
}
