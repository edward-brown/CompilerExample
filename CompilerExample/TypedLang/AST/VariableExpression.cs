using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    static class VariableExpression
    {
        public static IExpression Create(string name, ProgramState state)
        {
            var variable = state[name];
            if (variable.Type == DataType.Float)
                return new FloatVariableExpression(variable);
            else if (variable.Type == DataType.Int)
                return new IntVariableExpression(variable);
            else
                return new StringVariableExpression(variable);
        }
    }

    class IntVariableExpression : IntExpression
    {
        public Variable Variable { get; }

        public IntVariableExpression(Variable variable)
        {
            Variable = variable;
        }

        public override int Evaluate(ProgramState state)
        {
            return Variable.GetValue<int>();
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.Add((byte)OpCodes.Load);
            var address = Variable.Address;
            for (int i = 0; i < 4; i++)
            {
                result.Add((byte)(address & 0xff));
                address >>= 8;
            }
            return result;
        }
    }

    class FloatVariableExpression : FloatExpression
    {
        public Variable Variable { get; }

        public FloatVariableExpression(Variable variable)
        {
            Variable = variable;
        }

        public override double Evaluate(ProgramState state)
        {
            return Variable.GetValue<double>();
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.Add((byte)OpCodes.fLoad);
            var address = Variable.Address;
            for (int i = 0; i < 4; i++)
            {
                result.Add((byte)(address & 0xff));
                address >>= 8;
            }
            return result;
        }
    }

    class StringVariableExpression : StringExpression
    {
        public Variable Variable { get; }

        public StringVariableExpression(Variable variable)
        {
            Variable = variable;
        }

        public override string Evaluate(ProgramState state)
        {
            return Variable.GetValue<string>();
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.Add((byte)OpCodes.sLoad);
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
