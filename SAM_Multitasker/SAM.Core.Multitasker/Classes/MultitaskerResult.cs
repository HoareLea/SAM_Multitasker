using Microsoft.CodeAnalysis.Scripting;

namespace SAM.Core.Multitasker
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
