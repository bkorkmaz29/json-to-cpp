using System;
using System.Collections.Generic;
using System.Text;

namespace jsontocpp
{
    public class JSONNode
    {
        public enum Type
        {
            OBJECT,
            ARRAY
        }
   
        public  Dictionary<string,Prop> Object { get; set; }
        public List<JSONObject> Array { get; set; }
        public  Type NodeType { get; set; }
    }


}
