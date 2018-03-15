using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.Compiler;

namespace CompilerExample.TypedLang
{
    public class TypedLangTokenizer : Tokenizer
    {
        public TypedLangTokenizer()
        {
            DefineIgnore("[\\s]*");         //Whitespace
            DefineIgnore("//.*\\n");        //Single line comments
            DefineIgnore("/\\*.*\\*/");     //Multiline comments
            
            DefineToken("literalInt", "[0-9]+");
            DefineToken("literalFloat", "[0-9]+\\.[0-9]+([eE][-+][0-9]+)?");
            DefineToken("literalString", "'([^'\\\\]|\\\\[\\\\nrt'])*'");

            DefineToken("ident", "[_a-zA-Z][_a-zA-Z0-9]*");

            DefineToken("int", "int");
            DefineToken("float", "float");
            DefineToken("string", "string");

            DefineToken(";", ";");
            DefineToken("=", "=");
            DefineToken("+", "\\+");
            DefineToken("-", "-");
            DefineToken("/", "/");
            DefineToken("*", "\\*");
            DefineToken("%", "%");
            DefineToken("(", "\\(");
            DefineToken(")", "\\)");

            DefineToken("print", "print");
        }
    }
}
