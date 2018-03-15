using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerExample.TypedLang.AST
{
    public abstract class Statement
    {
        public abstract void Execute(ProgramState state);
        public abstract List<byte> CompileToVMCode();
    }
}
