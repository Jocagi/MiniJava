using System.Collections.Generic;
using MiniJava.SemanticAnalyzer;

namespace MiniJava.Parser.Descendente
{
    public class ParserReport
    {
        public bool isCorrect;
        public List<ParserError> Errors;
        public List<List<Symbol>> TablaSimbolos;

        public ParserReport() 
        {
            this.isCorrect = true;
            this.Errors = new List<ParserError>();
        }

        public void addError(ParserError error) 
        {
            Errors.Add(error);
            isCorrect = false;
        }
    }
}
