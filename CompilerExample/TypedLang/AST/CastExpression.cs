using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.TypedLang.AST
{
    abstract class CastExpression<T>
    {
        public static Expression<T> Create(Expression<string> expr)
        {
            if (typeof(T) == typeof(string))
                return expr as Expression<T>;
            else if (typeof(T) == typeof(int))
                return new StringToIntCastExpression(expr) as Expression<T>;
            else
                return new StringToFloatCastExpression(expr) as Expression<T>;
        }

        public static Expression<T> Create(Expression<int> expr)
        {
            if (typeof(T) == typeof(int))
                return expr as Expression<T>;
            else if (typeof(T) == typeof(double))
                return new IntToFloatCastExpression(expr) as Expression<T>;
            else
                return new IntToStringCastExpression(expr) as Expression<T>;
        }

        public static Expression<T> Create(Expression<double> expr)
        {
            if (typeof(T) == typeof(double))
                return expr as Expression<T>;
            else if (typeof(T) == typeof(int))
                return new FloatToIntCastExpression(expr) as Expression<T>;
            else
                return new FloatToStringCastExpression(expr) as Expression<T>;
        }
    }

    class IntToFloatCastExpression : FloatExpression
    {
        public Expression<int> Expression { get; }

        public IntToFloatCastExpression(Expression<int> expr)
        {
            Expression = expr;
        }

        public override double Evaluate(ProgramState state)
        {
            return (double)Expression.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.IntToFloat);
            return result;
        }
    }

    class IntToStringCastExpression : StringExpression
    {
        public Expression<int> Expression { get; }

        public IntToStringCastExpression(Expression<int> expr)
        {
            Expression = expr;
        }

        public override string Evaluate(ProgramState state)
        {
            return Expression.Evaluate(state).ToString();
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.IntToStr);
            return result;
        }
    }

    class FloatToIntCastExpression : IntExpression
    {
        public Expression<double> Expression { get; }

        public FloatToIntCastExpression(Expression<double> expr)
        {
            Expression = expr;
        }

        public override int Evaluate(ProgramState state)
        {
            return (int)Expression.Evaluate(state);
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.FloatToInt);
            return result;
        }
    }

    class FloatToStringCastExpression : StringExpression
    {
        public Expression<double> Expression { get; }

        public FloatToStringCastExpression(Expression<double> expr)
        {
            Expression = expr;
        }

        public override string Evaluate(ProgramState state)
        {
            return Expression.Evaluate(state).ToString();
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.FloatToStr);
            return result;
        }
    }

    class StringToIntCastExpression : IntExpression
    {
        public Expression<string> Expression { get; }

        public StringToIntCastExpression(Expression<string> expr)
        {
            Expression = expr;
        }

        public override int Evaluate(ProgramState state)
        {
            if (int.TryParse(Expression.Evaluate(state), out var result))
                return result;
            throw new RuntimeException("Invalid cast from string to int.");
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.StrToInt);
            return result;
        }
    }

    class StringToFloatCastExpression : FloatExpression
    {
        public Expression<string> Expression { get; }

        public StringToFloatCastExpression(Expression<string> expr)
        {
            Expression = expr;
        }

        public override double Evaluate(ProgramState state)
        {
            if (double.TryParse(Expression.Evaluate(state), out var result))
                return result;
            throw new RuntimeException("Invalid cast from string to float.");
        }

        public override List<byte> CompileToVMCode()
        {
            var result = new List<byte>();
            result.AddRange(Expression.CompileToVMCode());
            result.Add((byte)OpCodes.StrToFloat);
            return result;
        }
    }
}
