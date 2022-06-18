using System;
using System.Collections.Generic;
using System.Text;

namespace jsontocpp
{
    public class CppCbject
    {
        public Dictionary<string, Prop> Variables = new Dictionary<string, Prop>();
        public string Name { get; set; }
    }
}
