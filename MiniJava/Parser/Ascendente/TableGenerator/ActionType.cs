using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    /// <summary>
    /// the type of action the parser will perform
    /// </summary>
    public enum ActionType
    {
        Accept,
        Reduce,
        Shift,
        Error
    };
}
