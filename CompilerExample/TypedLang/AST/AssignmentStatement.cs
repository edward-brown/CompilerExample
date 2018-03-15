using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.Compiler;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    class AssignmentStatement : Statement
    {
        public Variable Variable { get; }
        public IExpression Right { get; }

        public AssignmentStatement(string name, ProgramState state, IExpression right)
        {
            Variable = state[name];
            Right = right;
            if (Variable.Type == DataType.Int && Right.Type != DataType.Int)
                throw new CompilerException("Cannot implicitly convert " + Right.Type.ToString() + " to Int.");
            if (Variable.Type == DataType.Float && Right.Type == DataType.String)
                throw new CompilerException("Cannot implicitly convert String to Float.");
        }

        public override void Execute(ProgramState state)
        {
            if (Right.Type == DataType.Float)
                Variable.SetValue((Right as Expression<double>).Evaluate(state));
            else if (Right.Type == DataType.Int)
                Variable.SetValue((Right as Expression<int>).Evaluate(state));
            else
                Variable.SetValue((Right as Expression<string>).Evaluate(state));
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Right.CompileToVMCode());
            if (Right.Type == DataType.Int)
                result.Add((byte)OpCodes.Store);
            else if (Right.Type == DataType.Float)
                result.Add((byte)OpCodes.fStore);
            else
                result.Add((byte)OpCodes.sStore);
            var address = Variable.Address;
            for (int i = 0; i < 4; i++)
            {
                result.Add((byte)(address & 0xff));
                address >>= 8;
            }
            return result;
        }
    }
}
