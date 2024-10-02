using System;

namespace SAM.Core.Multitasker
{
    public class MultitaskerResult
    {
        public Exception? Exception { get; }
        public MultitaskerInput MultitaskerInput { get; }
        public MultitaskerOutput MultitaskerOutput { get; }

        public MultitaskerResult(MultitaskerInput multitaskerInput, MultitaskerOutput multitaskerOutput, Exception exception = null)
        {
            MultitaskerInput = multitaskerInput;
            MultitaskerOutput = multitaskerOutput;
            Exception = exception;
        }
    }
}
