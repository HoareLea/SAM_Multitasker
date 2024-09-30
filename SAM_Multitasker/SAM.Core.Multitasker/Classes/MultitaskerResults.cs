using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace SAM.Core.Multitasker
{
    public class MultitaskerResults
    {
        private List<Diagnostic> diagnostics;
        private List<MultitaskerResult> multitaskerResults;


        public MultitaskerResults(IEnumerable<Diagnostic> diagnostics)
        {
            multitaskerResults = null;
            this.diagnostics = diagnostics == null ? null : new List<Diagnostic> (diagnostics);
        }

        public MultitaskerResults(IEnumerable<MultitaskerResult> multitaskerResults)
        {
            this.multitaskerResults = multitaskerResults == null ? null : new List<MultitaskerResult>(multitaskerResults);
        }

        public bool Succedded
        {
            get
            {
                return diagnostics == null || diagnostics.Count == 0;
            }
        }

        public List<Diagnostic> Diagnostics
        {
            get
            {
                return diagnostics;
            }
        }

        public List<T> GetOutputs<T>()
        {
            if (multitaskerResults == null)
            {
                return null;
            }

            List<T> result = new List<T>();
            foreach (MultitaskerResult multitaskerResult in multitaskerResults)
            {
                object @object = multitaskerResult.MultitaskerOutput.Result;
                if(@object is T)
                {
                    result.Add((T)@object);
                }
                else if(Query.TryConvert(@object, out T t))
                {
                    result.Add((T)@object);
                }
            }

            return result;
        }
    }
}
