using System.Collections.Generic;

namespace SAM.Core.Multitasker.Classes
{
    public class MultitaskerResults
    {
        private List<MultitaskerResult> multitaskerResults;


        public MultitaskerResults()
        {
            multitaskerResults = null;
        }

        public MultitaskerResults(IEnumerable<MultitaskerResult> multitaskerResults)
        {
            this.multitaskerResults = multitaskerResults == null ? null : new List<MultitaskerResult>(multitaskerResults);
        }

        public bool Succedded
        {
            get
            {
                return multitaskerResults != null && multitaskerResults.Count > 0;
            }
        }
    }
}
