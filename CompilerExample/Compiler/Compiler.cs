using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerExample.Compiler
{
    public class Compiler
    {
        private IReadOnlyList<Token> _tokens;
        private int _index = 0;

        public Compiler(IReadOnlyList<Token> tokens)
        {
            _tokens = tokens;
        }

        protected Token Next()
        {
            if (_index < _tokens.Count)
                return _tokens[_index++];
            else
                return null;
        }

        protected string Peek()
        {
            if (_index < _tokens.Count)
                return _tokens[_index].Type;
            else
                return null;
        }

        protected string Peek(int n)
        {
            if (_index + n < _tokens.Count)
                return _tokens[_index + n].Type;
            else
                return null;
        }

        protected Token Prev()
        {
            if (_index > 0)
                return _tokens[_index - 1];
            else
                return null;
        }

        protected void Expect(string type)
        {
            var token = Next();
            if (token.Type != type)
                throw new CompilerException("Expected " + type + " but got " + token.Type + " instead.");
        }

        protected void Expect(string type, out Token token)
        {
            token = Next();
            if (token.Type != type)
                throw new CompilerException("Expected " + type + " but got " + token.Type + " instead.");
        }

        protected bool Accept(string type)
        {
            if (Peek() == type)
            {
                Next();
                return true;
            }
            return false;
        }

        protected bool Accept(string type, out Token token)
        {
            if (Peek() == type)
            {
                token = Next();
                return true;
            }
            token = null;
            return false;
        }

        protected bool EndOfFile()
        {
            return _index >= _tokens.Count;
        }
    }
}
