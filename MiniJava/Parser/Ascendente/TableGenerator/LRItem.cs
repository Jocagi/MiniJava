using System.Collections.Generic;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class LRItem
    {
        /// <summary>
        /// Se refiere al elemento en la parte izquierda en una produccion.
        /// Ej. E -> AB  (Se refiere a 'E')
        /// </summary>
        public TokenType Token { get; set; }
        /// <summary>
        /// Se refiere a la posicion actual analizada en la produccion.
        /// Ej. E -> A.B  (Regresa '1')
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Se refiere al siguiente item posible.
        /// </summary>
        public List<TokenType> lookahead { get; set; }

        public LRItem(TokenType state, int position, List<TokenType> lookahead)
        {
            this.Token = state;
            this.Position = position;
            this.lookahead = new List<TokenType>(lookahead);
        }
        public LRItem(TokenType state, int position)
        {
            this.Token = state;
            this.Position = position;
            this.lookahead = new List<TokenType>{TokenType.Epsilon};
        }

        public bool Equals(LRItem item)
        {
            return (Token == item.Token) && (Position == item.Position);
        }
    }
}
