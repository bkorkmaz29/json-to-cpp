using System;
using System.IO;
using System.Diagnostics;
using static jsontocpp.Lexer;
using System.Collections.Generic;

namespace jsontocpp
{
    class Program
    {
        static void Main(string[] args)
        {
     
            string path = "../../../input.json";
            string json = File.ReadAllText(path);


            System.IO.DirectoryInfo di = new DirectoryInfo("../../../output");

            if (di.Exists){ 
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            }
            Dictionary<string, JSONNode> parsed = Parser.Parse(json);
            List<CppClass> classes = ClassConstructor.GetClasses(parsed);
            List<CppCbject> objects = ClassConstructor.GetObjects(parsed);
            Dictionary<string, List<string>> arrays = ClassConstructor.GetArrays(parsed);
            Generator.Generate(classes, objects, arrays);
            
        }
    }
}


