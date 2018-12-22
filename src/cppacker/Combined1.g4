grammar pckrDirectives;

/*
 * Parser Rules
 */

directivename	: COMMENTBANG 
 
 
/*
 * Lexer Rules
 */

DIRECTIVENAME : '[a-z]' ;
KEYVALUEDELIMITER : '=' ;
GROUPDELIMITER : ';' ;

PACKER	    : ('packer:')
COMMENTBANG : ('//!' | '\'\'!') ;

WHITESPACE	: (' ' | '\t') ;
NEWLINE		: ('\r'? '\n' | '\r')+ ;
TEXT		: ~[])]+ ;

WS
	:	' ' -> channel(HIDDEN)
	;
