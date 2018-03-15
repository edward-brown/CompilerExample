using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.Compiler;

namespace CompilerExample.TypedLang.AST
{
    public class Variable
    {
        public int Address { get; set; }
        public DataType Type { get; }
        public string Name { get; }

        private object _value;

        public T GetValue<T>()
        {
            return (T)_value;
        }

        public void SetValue(int val)
        {
            if (Type != DataType.Int)
                throw new CompilerException("Unable to imlictily cast Int to " + Type.ToString() + ".");
            _value = val;
        }

        public void SetValue(double val)
        {
            if (Type == DataType.String)
                throw new CompilerException("Unable to implicitly cast Float to String.");
            _value = val;
        }

        public void SetValue(string val)
        {
            if (Type != DataType.String)
                throw new CompilerException("Unable to implicity cast String to " + Type.ToString() + ".");
            _value = val;
        }
        
        public Variable(string name, int val)
        {
            Name = name;
            Type = DataType.Int;
            _value = val;
        }

        public Variable(string name, double val)
        {
            Name = name;
            Type = DataType.Float;
            _value = val;
        }

        public Variable(string name, string val)
        {
            Name = name;
            Type = DataType.String;
            _value = val;
        }

        public Variable(string name, DataType type)
        {
            Name = name;
            Type = type;
            if (Type == DataType.Int)
                _value = 0;
            else if (Type == DataType.Float)
                _value = 0.0;
            else
                _value = "";
        }
    }
}
