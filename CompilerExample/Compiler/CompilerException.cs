using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerExample.Compiler
{
    public class CompilerException : Exception
    {
        public CompilerException(string msg)
            : base(msg)
        {

        }
    }
}
