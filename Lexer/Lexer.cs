using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace jsontocpp
{

    public class Lexer
    {
        public enum TokenType
        {
            CURLY_OPEN,
            CURLY_CLOSE,
            COLON,
            STRING,
            NUMBER,
            ARRAY_OPEN,
            ARRAY_CLOSE,
            COMMA,
            BOOLEAN,
            NULL
        }

        public struct Token
        {
            public string Value { get; set; }
            public TokenType Type { get; set; }

            public override string ToString() => $"({Type},{Value})";
        }

        public static string RemoveWhitespace(string input)
        {
            input = input.Replace("\\", "");
            input = Regex.Replace(input, @"\s+", string.Empty);

            return input;
        }


        public static List<Token> GetTokens(string fileName)
        {
            List<Token> Tokens = new List<Token>();
            string Str = "";
            string input = RemoveWhitespace(fileName);

            Char[] letters = input.ToCharArray();

            int i = 0;
            while (i < letters.Length)
            {
                Token newToken = new Token();

                if (Char.Equals(letters[i], '{'))
                {
                    newToken.Type = TokenType.CURLY_OPEN;

                }
                else if (Char.Equals(letters[i], '}'))
                {
                    newToken.Type = TokenType.CURLY_CLOSE;

                }
                else if (Char.Equals(letters[i], ':'))
                {
                    newToken.Type = TokenType.COLON;

                }
                else if (Char.Equals(letters[i], '"'))
                {

                    i++;
                    while (!Char.Equals(letters[i], '"'))
                    {
                        Str += letters[i];
                        i++;
                    }
                    newToken.Type = TokenType.STRING;
                    newToken.Value = Str;
                    Str = "";

                }
                else if (Char.IsDigit(letters[i]))
                {
                    while (Char.IsDigit(letters[i]) || Char.Equals(letters[i], '.'))
                    {
                        Str += letters[i].ToString();
                        i++;
                    }
                    i--;
                    newToken.Type = TokenType.NUMBER;
                    newToken.Value = Str;
                    Str = "";

                }
                else if (Char.Equals(letters[i], 't'))
                {
                    while (!Char.Equals(letters[i], 'e'))
                    {
                        Str += letters[i].ToString();
                        i++;
                    }
                    Str += "e";
                    newToken.Type = TokenType.BOOLEAN;
                    newToken.Value = Str;
                    Str = "";

                }
                else if (Char.Equals(letters[i], 'f'))
                {
                    while (!Char.Equals(letters[i], 'e'))
                    {
                        Str += letters[i].ToString();
                        i++;
                    }
                    Str += "e";
                    newToken.Type = TokenType.BOOLEAN;
                    newToken.Value = Str;
                    Str = "";

                }
                else if (Char.Equals(letters[i], 'n'))
                {
                    while (!Char.Equals(letters[i], 'l'))
                    {
                        Str += letters[i].ToString();
                        i++;
                    }
                    Str += "ll";
                    newToken.Type = TokenType.NULL;
                    newToken.Value = Str;
                    Str = "";

                }
                else if (Char.Equals(letters[i], '['))
                {
                    newToken.Type = TokenType.ARRAY_OPEN;
                }
                else if (Char.Equals(letters[i], ']'))
                {
                    newToken.Type = TokenType.ARRAY_CLOSE;
                }
                else if (Char.Equals(letters[i], ','))
                {
                    newToken.Type = TokenType.COMMA;
                }
                else
                {
                    Console.WriteLine($"Error at {i}. character");
                    throw new Exception("Error ");
                }

                Tokens.Add(newToken);
                i++;
            }

            return Tokens;

        }


        public static List<Token> Tokenizer(string fileName)
        {
            string fileWithoutSpace = RemoveWhitespace(fileName);
            return GetTokens(fileWithoutSpace);

        }

    }
}
