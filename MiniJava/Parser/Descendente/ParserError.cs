using MiniJava.General;
using MiniJava.Lexer;

namespace MiniJava.Parser.Descendente
{
    public class ParserError
    {
        public ErrorType errorType;
        public TokenType value;
        public TokenType expected;
        public TokenLocation location;

        public ParserError(TokenType value, TokenType expected) 
        {
            this.value = value;
            this.expected = expected;
        }
        public ParserError(TokenType value, TokenType expected, TokenLocation location)
        {
            this.value = value;
            this.expected = expected;
            this.location = location;
        }
        public ParserError(TokenType value, TokenType expected, TokenLocation location, ErrorType errorType)
        {
            this.value = value;
            this.expected = expected;
            this.location = location;
            this.errorType = errorType;
        }
    }
}
