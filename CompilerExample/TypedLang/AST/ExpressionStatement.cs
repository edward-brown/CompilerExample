using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    class ExpressionStatement : Statement
    {
        public IExpression Expression { get; }

        public ExpressionStatement(IExpression expr)
        {
            Expression = expr;
        }

        public override void Execute(ProgramState state)
        {
            if (Expression.Type == DataType.Float)
                (Expression as Expression<double>).Evaluate(state);
            else if (Expression.Type == DataType.Int)
                (Expression as Expression<int>).Evaluate(state);
            else
                (Expression as Expression<string>).Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            if (Expression.Type == DataType.Int)
                result.Add((byte)OpCodes.Pop);
            else if (Expression.Type == DataType.Float)
                result.Add((byte)OpCodes.fPop);
            else
                result.Add((byte)OpCodes.sPop);
            return result;
        }
    }
}
