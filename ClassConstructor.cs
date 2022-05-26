using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace jsontocpp
{
    static public class ClassConstructor
    {
        public static List<CppClass> Classes = new List<CppClass>();
        public static List<CppCbject> Objects = new List<CppCbject>();
        public static Dictionary<string, List<string>> Arrays = new Dictionary<string, List<string>>();

        static public bool IsExist(CppClass cls, List<CppClass> list)
        {
            var keys = cls.Properties.Keys;

            foreach (var cls2 in list)
            {
                if (keys.Intersect(cls2.Properties.Keys).Any())
                {
                    return false;
                }
            }

            return true;
        }


        static public CppClass GetClass(string objectName, JSONNode Node)
        {

            if (Node.NodeType == JSONNode.Type.OBJECT)
            {
                CppClass cls = new CppClass();
                CppCbject obj = new CppCbject();
                obj.Name = objectName;

                var enumerator = Node.Object.GetEnumerator();
                while (enumerator.MoveNext())
                {

                    cls.Properties.Add(enumerator.Current.Key, enumerator.Current.Value.type);
                    obj.Variables.Add(enumerator.Current.Key, enumerator.Current.Value);
                }
                Objects.Add(obj);
                return cls;
            }

            else if (Node.NodeType == JSONNode.Type.ARRAY)
            {
                CppClass cls = new CppClass();

                foreach (var entry in Node.Array)
                {

                    CppCbject obj = new CppCbject();
                    obj.Name = entry.Name;

                    var enumerator = entry.Dict.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        if (entry == Node.Array.First())
                        {
                            cls.Properties.Add(enumerator.Current.Key, enumerator.Current.Value.type);
                        }

                        obj.Variables.Add(enumerator.Current.Key, enumerator.Current.Value);
                    }
                    Objects.Add(obj);
                }

                return cls;
            }

            else return null;

        }

        static public List<CppClass> GetUniqueClasses()
        {

            List<CppClass> ClassList = new List<CppClass>();

            foreach (var cls in Classes.ToList())
            {
                if (ClassList.Count() == 0)
                {
                    ClassList.Add(cls);
                }

                if (IsExist(cls, ClassList))
                {
                    ClassList.Add(cls);

                }

            }

            return ClassList;
        }

        static public List<CppCbject> GetObjects(Dictionary<string, JSONNode> JSONTree)
        {

            return Objects;
        }

        static public Dictionary<string, List<string>> GetArrays(Dictionary<string, JSONNode> JSONTree)
        {

            return Arrays;
        }

        static public List<CppClass> GetClasses(Dictionary<string, JSONNode> JSONTree)
        {
            var enumerator = JSONTree.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.NodeType == JSONNode.Type.ARRAY)
                {
                    List<string> list = new List<string>();
                    foreach (var i in enumerator.Current.Value.Array)
                    {
                        list.Add(i.Name);
                    }

                    Arrays.Add(enumerator.Current.Key, list);
                }

                Classes.Add(GetClass(enumerator.Current.Key, enumerator.Current.Value));


            }

            return GetUniqueClasses();

        }
    }
}
