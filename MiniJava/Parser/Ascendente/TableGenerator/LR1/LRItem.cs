using System.Collections.Generic;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.Parser;
using MiniJava.Parser.Ascendente.TableGenerator.Grammar;

namespace MiniJava.Parser.Ascendente.TableGenerator.LR1
{
    public class LRItem
    {
        /// <summary>
        /// Produccion completa
        /// Ej. E -> AB
        /// </summary>
        public Production Production { get; set; }
        /// <summary>
        /// Se refiere a la posicion actual analizada en la produccion.
        /// Ej. E -> A*B  (Regresa '1')
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Se refiere al siguiente item posible.
        /// </summary>
        public List<TokenType> lookahead { get; set; }
        /// <summary>
        /// El tipo de accion que hace el parser
        /// </summary>
        public ActionType action { get; set; }
        /// <summary>
        /// Si la accion es shift, el estado al que apunta
        /// </summary>
        public int shiftTo { get; set; }

        public LRItem(Production production, int position, List<TokenType> lookahead)
        {
            this.Position = position;
            this.lookahead = new List<TokenType>(lookahead);
            this.Production = production;
            this.action = ActionType.Shift;
            this.shiftTo = -1;
        }
        public LRItem(Production production, int position)
        {
            this.Position = position;
            this.Production = production;
            this.lookahead = new List<TokenType>{TokenType.Epsilon};
            this.action = ActionType.Shift;
            this.shiftTo = -1;
        }
        public LRItem Copy()
        {
            LRItem temp = (LRItem)this.MemberwiseClone();
            return temp;
        }
    }
}
