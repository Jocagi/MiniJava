﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Parser.RecursiveDescent
{
    public class ParserReport
    {
        public bool isCorrect;
        public List<ParserError> Errors;

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
