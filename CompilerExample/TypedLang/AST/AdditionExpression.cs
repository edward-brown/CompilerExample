using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    static class AdditionExpression
    {
        public static IExpression Create(IExpression left, IExpression right)
        {
            if (left.Type == DataType.String || right.Type == DataType.String)
            {
                return new StringAdditionExpression(left.As<string>(), right.As<string>());
            }
            else if (left.Type == DataType.Float || right.Type == DataType.Float)
            {
                return new FloatAdditionExpression(left.As<double>(), right.As<double>());
            }
            else
            {
                return new IntAdditionExpression(left.As<int>(), right.As<int>());
            }
        }
    }

    class IntAdditionExpression : IntExpression
    {
        public Expression<int> Left { get; }
        public Expression<int> Right { get; }

        public IntAdditionExpression(Expression<int> left, Expression<int> right)
        {
            Left = left;
            Right = right;
        }

        public override int Evaluate(ProgramState state)
        {
            return Left.Evaluate(state) + Right.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            List<byte> result = new List<byte>();
            result.AddRange(Left.CompileToVMCode());
            result.AddRange(Right.CompileToVMCode());
            result.Add((byte)OpCodes.Add);
            return result;
        }
    }

    class FloatAdditionExpression : FloatExpression
    {
        public Expression<double> Left { get; }
        public Expression<double> Right { get; }

        public FloatAdditionExpression(Expression<double> left, Expression<double> right)
        {
            Left = left;
            Right = right;
        }

        public override double Evaluate(ProgramState state)
        {
            return Left.Evaluate(state) + Right.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            List<byte> result = new List<byte>();
            result.AddRange(Left.CompileToVMCode());
            result.AddRange(Right.CompileToVMCode());
            result.Add((byte)OpCodes.fAdd);
            return result;
        }
    }

    class StringAdditionExpression : StringExpression
    {
        public Expression<string> Left { get; }
        public Expression<string> Right { get; }

        public StringAdditionExpression(Expression<string> left, Expression<string> right)
        {
            Left = left;
            Right = right;
        }

        public override string Evaluate(ProgramState state)
        {
            return Left.Evaluate(state) + Right.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            List<byte> result = new List<byte>();
            result.AddRange(Left.CompileToVMCode());
            result.AddRange(Right.CompileToVMCode());
            result.Add((byte)OpCodes.Cat);
            return result;
        }
    }
}
