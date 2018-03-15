using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CompilerExample.Compiler
{
    public class Tokenizer
    {
        private List<TokenDefinition> _tokenDefs = new List<TokenDefinition>();
        
        public void DefineIgnore(string pattern)
        {
            _tokenDefs.Add(new TokenDefinition(pattern));
        }

        public void DefineToken(string type, string pattern)
        {
            int priority = _tokenDefs.Count;
            _tokenDefs.Add(new TokenDefinition(priority, type, pattern));
        }
        
        public IReadOnlyList<Token> Tokenize(string input)
        {
            //Get all matches
            List<TokenMatch> matches = new List<TokenMatch>();
            foreach (var tokenDef in _tokenDefs)
                tokenDef.Match(input, matches);

            //Sort matches
            matches = matches.OrderBy(x => x.Index).ThenByDescending(x => x.Length).ThenByDescending(x => x.Priority).ToList();

            //Get tokens
            List<Token> tokens = new List<Token>();
            int currentIndex = 0;
            foreach (var match in matches)
            {
                if (match.Index < currentIndex)
                    continue;
                if (match.Index != currentIndex)
                    throw new CompilerException("Invalid token at position " + currentIndex);
                if (!match.Ignore)
                    tokens.Add(match.GetToken(input));
                currentIndex += match.Length;
            }

            return tokens;
        }
    }
}
