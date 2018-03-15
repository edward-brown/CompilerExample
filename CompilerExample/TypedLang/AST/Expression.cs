using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerExample.TypedLang.AST
{
    public interface IExpression
    {
        DataType Type { get; }
        Expression<TOther> As<TOther>();
        List<byte> CompileToVMCode();
    }

    public abstract class Expression<T> : IExpression
    {
        public abstract DataType Type { get; }
        public abstract Expression<TOther> As<TOther>();
        public abstract T Evaluate(ProgramState state);
        public abstract List<byte> CompileToVMCode();
    }

    abstract class IntExpression : Expression<int>
    {
        public override DataType Type => DataType.Int;
        public override Expression<TOther> As<TOther>()
        {
            return CastExpression<TOther>.Create(this);
        }
    }

    abstract class FloatExpression : Expression<double>
    {
        public override DataType Type => DataType.Float;
        public override Expression<TOther> As<TOther>()
        {
            return CastExpression<TOther>.Create(this);
        }
    }

    abstract class StringExpression : Expression<string>
    {
        public override DataType Type => DataType.String;
        public override Expression<TOther> As<TOther>()
        {
            return CastExpression<TOther>.Create(this);
        }
    }
}
