using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static jsontocpp.Lexer;

namespace jsontocpp
{
    static public class Parser
    {
        public static Dictionary<string, JSONNode> JSONTree = new Dictionary<string, JSONNode>();
        public static int i = 0;

        public static JSONNode ParseObject(List<Token> Tokens)
        {
            Dictionary<string, Prop> JSONObject = new Dictionary<string, Prop>();

            while (Tokens[i].Type != TokenType.CURLY_CLOSE)
            {
                if (Tokens[i].Type == TokenType.CURLY_OPEN)
                {
                    i++;
                }

                if (Tokens[i].Type == TokenType.STRING)
                {
                    i += 2;

                    if (Tokens[i].Type == TokenType.STRING)
                    {
                        Prop prop = new Prop();
                        prop.type = "string";
                        prop.value = Tokens[i].Value;
                        JSONObject.Add(Tokens[i - 2].Value, prop);
                        i++;
                    }
                    else if (Tokens[i].Type == TokenType.NUMBER)
                    {
                        Prop prop = new Prop();
                        prop.type = "int";
                        prop.value = Tokens[i].Value;
                        JSONObject.Add(Tokens[i - 2].Value, prop);
                        i++;
                    }
                    else if (Tokens[i].Type == TokenType.BOOLEAN)
                    {
                        Prop prop = new Prop();
                        prop.type = "bool";
                        prop.value = Tokens[i].Value;
                        JSONObject.Add(Tokens[i - 2].Value, prop);
                        i++;
                    }

                }
                if (Tokens[i].Type == TokenType.COMMA)
                {
                    i++;
                }
            }
            JSONNode Node = new JSONNode();
            Node.Object = JSONObject;
            Node.NodeType = JSONNode.Type.OBJECT;

            return Node;
        }




        public static JSONNode ParseArray(List<Token> Tokens)
        {
            List<JSONObject> JSONArray = new List<JSONObject>();

            JSONNode Node = new JSONNode();

            while (Tokens[i].Type != TokenType.ARRAY_CLOSE)
            {
                if (Tokens[i].Type == TokenType.CURLY_OPEN)
                {
                    i++;
                    JSONObject obj = new JSONObject();
                    obj.Name = Tokens[i].Value;
                    JSONNode nd = ParseObject(Tokens);
                    
                    obj.Dict = nd.Object;
                    
                    JSONArray.Add(obj);
                    i += 2;
                }
                else if (Tokens[i].Type == TokenType.STRING)
                {
                    JSONNode nd = ParseObject(Tokens);
                    JSONObject obj = new JSONObject();
                    obj.Dict = nd.Object;
                    obj.Name = Tokens[i].Value;
                    JSONArray.Add(obj);
                    i += 2;

                }
                else if (Tokens[i].Type == TokenType.COMMA)
                {
                    i += 1;
                }
                else
                {
                    throw new Exception("Wrong JSON file") ;
                }
             

            }
            Node.Array = JSONArray;
            Node.NodeType = JSONNode.Type.ARRAY;

            return Node;

        }


        public static Dictionary<string, JSONNode> Parse(string file)
        {
            List<Token> Tokens = Lexer.Tokenizer(file);

            while (i < Tokens.Count)
            {
                if (Tokens[i].Type == TokenType.CURLY_OPEN)
                {
                    i++;
                }
                if (Tokens[i].Type == TokenType.COMMA)
                {
                    i++;
                }
                if (Tokens[i].Type == TokenType.STRING)
                {
                    i += 2;
                    if (Tokens[i].Type == TokenType.CURLY_OPEN)
                    { 
                        string Name = Tokens[i - 2].Value;
                        i++;
                        JSONNode Node = ParseObject(Tokens);
                        JSONTree.Add(Name, Node);
                        i++;

                    }
                    else if (Tokens[i].Type == TokenType.ARRAY_OPEN)
                    {
                        string Name = Tokens[i - 2].Value;
                        i++;
                        JSONNode Node = ParseArray(Tokens);
                        JSONTree.Add(Name, Node);
                        i++;
                    }
                    else
                    {
                        Console.WriteLine($"Error at {i}. token");
                        throw new Exception("Error");
                        
                    }
                }

                i++;

            }
            return JSONTree;
        }
    }
}
