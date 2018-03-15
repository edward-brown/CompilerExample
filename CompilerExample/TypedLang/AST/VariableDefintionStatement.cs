using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.Compiler;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    class VariableDefintionStatement : Statement
    {
        public IExpression Right { get; }
        public Variable Variable { get; }

        public VariableDefintionStatement(string name, DataType type, ProgramState state)
        {
            Variable = new Variable(name, type);
            state.DefineVariable(Variable);
            Right = null;
        }

        public VariableDefintionStatement(string name, DataType type, ProgramState state, IExpression right)
        {
            Variable = new Variable(name, type);
            state.DefineVariable(Variable);
            if (Variable.Type == DataType.Int)
            {
                if (right.Type != DataType.Int)
                    throw new CompilerException("Cannot implicitly convert " + Right.Type + " to Int.");
                Right = right.As<int>();
            }
            else if (Variable.Type == DataType.Float)
            {
                if (right.Type == DataType.String)
                    throw new CompilerException("Cannot implicitly convert String to Float.");
                Right = right.As<double>();
            }
            else
            {
                Right = right.As<string>();
            }
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
