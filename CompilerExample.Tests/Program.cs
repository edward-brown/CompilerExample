using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.TypedLang;
using CompilerExample.TypedLang.VM;

namespace CompilerExample.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = @"
int a = 5;
float b = 4.5;
print a + b * 2;
a = 5;
";

            var tokenizer = new TypedLangTokenizer();
            var tokens = tokenizer.Tokenize(s);
            var compiler = new TypedLangCompiler(tokens);
            var progState = compiler.ParseProgram();
            progState.Execute();

            Console.WriteLine();
            Console.WriteLine();

            var bytes = progState.CompileToVMCode();
            foreach (var b in bytes)
                Console.WriteLine("0x{0:X2}", b);


            VirtualMachine vm = new VirtualMachine(bytes.ToArray());
            vm.Run();

            Console.ReadLine();
        }
    }
}
