using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Lexer
{
    /// <summary>
    /// Especifica la fila y las columnas en las que se encuentra un determinado token
    /// </summary>
    public class TokenLocation
    {
        public int row;

        public int firstCol;


        public int lastCol;

        public TokenLocation(int row, int firstCol, int lastCol) 
        {
            this.row = row;
            this.firstCol = firstCol;
            this.lastCol = lastCol;
        }
    }
}
