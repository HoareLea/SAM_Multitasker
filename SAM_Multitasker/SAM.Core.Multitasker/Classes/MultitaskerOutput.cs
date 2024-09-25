namespace SAM.Core.Multitasker.Classes
{
    public class MultitaskerOutput
    {
        private object result;

        public MultitaskerOutput(object result)
        {
            this.result = result;
        }

        public object Result
        {
            get
            {
                return result;
            }
        }
    }
}
