## Tokens

literalInt = [0-9]+
.

literalFloat = [0-9]+\.[0-9]+[eE][-+][0-9]+
.

literalString = '([^"\\]|\\[\\nrt'])+*'
.

ident = [_a-zA-Z][_a-zA-Z0-9]*
.

## Grammer

Program =		{ Statement }
.

Statement =		Type ident ["=" Expr] ";"
			|	ident ["=" Expr] ";"
			|	"print" Expr ";"
			|	Expr ";"
.

Type =			"int"
			|	"float"
			|	"string"
.

Expr =		Term { ( "+" | "-" ) Term }
.

Term =		Factor { ("*" | "/" | "%" ) Factor }
.

Factor =	["-"] UnaryTerm
.

UnaryTerm = ident
		|	literalInt
		|	literalFloat
		|	literalString
		|	"(" Type ")" Expr
		|	"(" Expr ")"
.

