using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.Compiler;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    static class NegateExpression
    {
        public static IExpression Create(IExpression right)
        {
            if (right.Type == DataType.String)
                throw new CompilerException("The unary '-' operator does not support String operands.");
            else if (right.Type == DataType.Float)
                return new FloatNegateExpression(right.As<double>());
            else
                return new IntNegateExpression(right.As<int>());
        }
    }

    class IntNegateExpression : IntExpression
    {
        public Expression<int> Expression { get; }

        public IntNegateExpression(Expression<int> expr)
        {
            Expression = expr;
        }

        public override int Evaluate(ProgramState state)
        {
            return -Expression.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.Neg);
            return result;
        }
    }

    class FloatNegateExpression : FloatExpression
    {
        public Expression<double> Expression { get; }

        public FloatNegateExpression(Expression<double> expr)
        {
            Expression = expr;
        }

        public override double Evaluate(ProgramState state)
        {
            return -Expression.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.fNeg);
            return result;
        }
    }
}
