using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerExample.Compiler;
using CompilerExample.TypedLang.AST;

namespace CompilerExample.TypedLang
{
    public class TypedLangCompiler : Compiler.Compiler
    {
        private ProgramState _state;


        public TypedLangCompiler(IReadOnlyList<Token> tokens)
            : base(tokens)
        {
        }

        public ProgramState ParseProgram()
        {
            _state = new ProgramState();
            while (!EndOfFile())
                _state.AddStatement(ParseStatement());
            return _state;
        }

        protected Statement ParseStatement()
        {
            var nextType = Peek();
            if (nextType == "int" || nextType == "float" || nextType == "string")
                return ParseVariableDefinition();
            else if (nextType == "print")
                return ParsePrintStatement();
            else if (Peek(1) == "=")
                return ParseAssignmentStatement();
            else
                return ParseExpressionStatement();
        }

        protected Statement ParseVariableDefinition()
        {
            Statement result;
            var type = ParseType();
            Expect("ident", out var ident);
            if (Accept("="))
            {
                var right = ParseExpression();
                result = new VariableDefintionStatement(ident.Text, type, _state, right);
            }
            else
            {
                result = new VariableDefintionStatement(ident.Text, type, _state);
            }
            Expect(";");
            return result;
        }

        protected Statement ParsePrintStatement()
        {
            Expect("print");
            var expr = ParseExpression();
            Expect(";");
            return new PrintStatement(expr);
        }

        protected Statement ParseAssignmentStatement()
        {
            Expect("ident", out var token);
            Expect("=");
            var right = ParseExpression();
            Expect(";");
            return new AssignmentStatement(token.Text, _state, right);
        }

        protected Statement ParseExpressionStatement()
        {
            var expr = ParseExpression();
            Expect(";");
            return new ExpressionStatement(expr);
        }

        protected IExpression ParseExpression()
        {
            var left = ParseTerm();
            while (Accept("+") || Accept("-"))
            {
                if (Prev().Type == "+")
                    left = AdditionExpression.Create(left, ParseTerm());
                else
                    left = SubtractionExpression.Create(left, ParseTerm());
            }
            return left;
        }

        protected IExpression ParseTerm()
        {
            var left = ParseFactor();
            while (Accept("*") || Accept("/") || Accept("%"))
            {
                if (Prev().Type == "*")
                    left = MultiplyExpression.Create(left, ParseFactor());
                else if (Prev().Type == "/")
                    left = DivideExpression.Create(left, ParseFactor());
                else
                    left = ModuloExpression.Create(left, ParseFactor());
            }
            return left;
        }

        protected IExpression ParseFactor()
        {
            if (Accept("-"))
                return NegateExpression.Create(ParseUnaryTerm());
            else
                return ParseUnaryTerm();
        }

        protected IExpression ParseUnaryTerm()
        {
            if (Accept("ident", out var identToken))
                return VariableExpression.Create(identToken.Text, _state);
            else if (Accept("literalInt", out var intToken))
                return new IntConstantExpression(int.Parse(intToken.Text));
            else if (Accept("literalFloat", out var floatToken))
                return new FloatConstantExpression(double.Parse(floatToken.Text));
            else if (Accept("literalString", out var stringToken))
                return new StringConstantExpression(StringFromLiteral(stringToken.Text));
            else if (Accept("("))
            {
                if (Peek() == "int" || Peek() == "float" || Peek() == "string")
                {
                    var type = ParseType();
                    Expect(")");
                    var expr = ParseExpression();
                    if (type == DataType.Float)
                        return expr.As<double>();
                    else if (type == DataType.Int)
                        return expr.As<int>();
                    else
                        return expr.As<string>();
                }
                else
                {
                    var expr = ParseExpression();
                    Expect(")");
                    return expr;
                }
            }
            throw new CompilerException("Invalid statement.");
        }

        protected DataType ParseType()
        {
            if (Accept("int"))
                return DataType.Int;
            else if (Accept("float"))
                return DataType.Float;
            else if (Accept("string"))
                return DataType.Float;
            throw new CompilerException("Expected int, float or string but got " + Next().Type);
        }

        protected string StringFromLiteral(string stringLit)
        {
            var sb = new StringBuilder();
            bool escaped = false;
            for (int i = 1; i < stringLit.Length - 1; i++)
            {
                char c = stringLit[i];
                if (escaped)
                {
                    if (c == '\\' || c == '\'')
                        sb.Append(c);
                    else if (c == 'n')
                        sb.Append('\n');
                    else if (c == 'r')
                        sb.Append('\r');
                    else if (c == 't')
                        sb.Append('\t');
                    else
                        throw new CompilerException("Invalid string literal.");
                    escaped = false;
                }
                else
                {
                    if (c == '\\')
                        escaped = true;
                    else
                        sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
