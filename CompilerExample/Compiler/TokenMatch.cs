using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CompilerExample.Compiler
{
    class TokenMatch
    {
        public int Index { get; }
        public int Length { get; }
        public TokenDefinition TokenDefinition { get; }

        public bool Ignore => TokenDefinition.Ignore;
        public int Priority => TokenDefinition.Priority;
        public string Type => TokenDefinition.Type;
        
        public TokenMatch(TokenDefinition tokenDef, Match match)
        {
            TokenDefinition = tokenDef;
            Index = match.Index;
            Length = match.Length;
        }

        public Token GetToken(string input)
        {
            var text = input.Substring(Index, Length);
            return new Token(Type, text);
        }
    }
}
