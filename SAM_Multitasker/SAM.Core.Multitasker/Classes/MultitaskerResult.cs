using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAM.Core.Multitasker.Classes
{
    public class MultitaskerResult
    {
        public CompilationErrorException? CompilationErrorException { get; }
        public MultitaskerInput MultitaskerInput { get; }
        public MultitaskerOutput MultitaskerOutput { get; }

        public MultitaskerResult(MultitaskerInput multitaskerInput, MultitaskerOutput multitaskerOutput, CompilationErrorException compilationErrorException = null)
        {
            MultitaskerInput = multitaskerInput;
            MultitaskerOutput = multitaskerOutput;
            CompilationErrorException = compilationErrorException;
        }
    }
}
