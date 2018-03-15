using CompilerExample.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    class MultiplyExpression
    {
        public static IExpression Create(IExpression left, IExpression right)
        {
            if (left.Type == DataType.String || right.Type == DataType.String)
                throw new CompilerException("Cannot apply '*' operator to an operand of type String.");
            else if (left.Type == DataType.Float || right.Type == DataType.Float)
                return new FloatMultiplyExpression(left.As<double>(), right.As<double>());
            else
                return new IntMultiplyExpression(left.As<int>(), right.As<int>());
        }
    }

    class IntMultiplyExpression : IntExpression
    {
        public Expression<int> Left { get; }
        public Expression<int> Right { get; }

        public IntMultiplyExpression(Expression<int> left, Expression<int> right)
        {
            Left = left;
            Right = right;
        }

        public override int Evaluate(ProgramState state)
        {
            return Left.Evaluate(state) * Right.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            List<byte> result = new List<byte>();
            result.AddRange(Left.CompileToVMCode());
            result.AddRange(Right.CompileToVMCode());
            result.Add((byte)OpCodes.Mul);
            return result;
        }
    }

    class FloatMultiplyExpression : FloatExpression
    {
        public Expression<double> Left { get; }
        public Expression<double> Right { get; }

        public FloatMultiplyExpression(Expression<double> left, Expression<double> right)
        {
            Left = left;
            Right = right;
        }

        public override double Evaluate(ProgramState state)
        {
            return Left.Evaluate(state) * Right.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            List<byte> result = new List<byte>();
            result.AddRange(Left.CompileToVMCode());
            result.AddRange(Right.CompileToVMCode());
            result.Add((byte)OpCodes.fMul);
            return result;
        }
    }
}
