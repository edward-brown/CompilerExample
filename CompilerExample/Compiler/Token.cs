using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerExample.Compiler
{
    public class Token
    {
        public string Type { get; }
        public string Text { get; }

        public Token(string type, string text)
        {
            Type = type;
            Text = text;
        }
    }
}
