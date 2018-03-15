using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CompilerExample.Compiler
{
    class TokenDefinition
    {
        public Regex Regex { get; }
        public string Type { get; }
        public int Priority { get; }
        public bool Ignore { get; }
        
        public TokenDefinition(int priority, string type, string pattern)
        {
            Priority = priority;
            Regex = new Regex(pattern);
            Type = type;
            Ignore = false;
        }

        public TokenDefinition(string pattern)
        {
            Priority = -1;
            Regex = new Regex(pattern);
            Ignore = true;
        }

        public void Match(string input, ICollection<TokenMatch> matches)
        {
            var regexMatches = Regex.Matches(input);
            foreach (var regexMatch in regexMatches.Cast<Match>())
            {
                var match = new TokenMatch(this, regexMatch);
                matches.Add(match);
            }
        }
    }
}
