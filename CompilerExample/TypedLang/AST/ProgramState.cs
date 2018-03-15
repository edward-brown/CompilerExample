using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.Compiler;

namespace CompilerExample.TypedLang.AST
{
    public class ProgramState
    {
        private Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();
        private List<Statement> _statements = new List<Statement>();

        public void DefineVariable(Variable variable)
        {
            if (_variables.ContainsKey(variable.Name))
                throw new CompilerException("The variable '" + variable.Name + "' is already defined.");
            _variables.Add(variable.Name, variable);
        }

        public Variable this[string name]
        {
            get
            {
                if (_variables.TryGetValue(name, out var variable))
                    return variable;
                else
                    throw new CompilerException("Attempted to access undefined variable '" + name + "'.");
            }
        }

        public void AddStatement(Statement statement)
        {
            _statements.Add(statement);
        }

        public void Execute()
        {
            foreach (var statement in _statements)
                statement.Execute(this);
        }

        public List<byte> CompileToVMCode()
        {
            //Calculate variable addresses
            int address = 0;
            foreach (var variable in _variables.Values)
            {
                variable.Address = address;
                if (variable.Type == DataType.Int)
                    address += 4;
                else if (variable.Type == DataType.Float)
                    address += 8;
                else
                    address += 256;
            }

            var result = new List<byte>();
            foreach (var statement in _statements)
                result.AddRange(statement.CompileToVMCode());
            return result;
        }
    }
}
