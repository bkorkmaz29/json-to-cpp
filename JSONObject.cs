using System;
using System.Collections.Generic;
using System.Text;

namespace jsontocpp
{
    public class JSONObject
    {
        public Dictionary<string, Prop> Dict { get; set; }
        public string Name { get; set; }
    }
}
