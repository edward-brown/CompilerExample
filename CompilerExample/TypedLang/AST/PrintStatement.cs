using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    public class PrintStatement : Statement
    {
        public Expression<string> Expression { get; }

        public PrintStatement(IExpression expression)
        {
            Expression = expression.As<string>();
        }

        public override void Execute(ProgramState state)
        {
            Console.Write(Expression.Evaluate(state));
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.Print);
            return result;
        }
    }
}
