using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    class IntConstantExpression : IntExpression
    {
        public int Value { get; }

        public IntConstantExpression(int value)
        {
            Value = value;
        }

        public override int Evaluate(ProgramState state)
        {
            return Value;
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.Add((byte)OpCodes.Push);
            var val = Value;
            for (int i = 0; i < 4; i++)
            {
                result.Add((byte)(val & 0xff));
                val >>= 8;
            }
            return result;
        }
    }

    class FloatConstantExpression : FloatExpression
    {
        public double Value { get; }

        public FloatConstantExpression(double value)
        {
            Value = value;
        }

        public override double Evaluate(ProgramState state)
        {
            return Value;
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.Add((byte)OpCodes.fPush);
            var val = BitConverter.DoubleToInt64Bits(Value);
            for (int i = 0; i < 8; i++)
            {
                result.Add((byte)(val & 0xff));
                val >>= 8;
            }
            return result;
        }
    }

    class StringConstantExpression : StringExpression
    {
        public string Value { get; }

        public StringConstantExpression(string value)
        {
            Value = value;
        }

        public override string Evaluate(ProgramState state)
        {
            return Value;
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.Add((byte)OpCodes.sPush);
            for (int i = 0; i < Value.Length; i++)
                result.Add((byte)Value[i]);
            result.Add(0);
            return result;
        }
    }
}
