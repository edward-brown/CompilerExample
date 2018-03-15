using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerExample.TypedLang.AST
{
    abstract class UnaryExpression : Expression
    {
        public Expression Right { get; }

        public UnaryExpression(Expression right)
        {
            Right = right;
        }
    }
}
