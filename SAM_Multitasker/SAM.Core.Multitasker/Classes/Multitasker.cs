using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SAM.Core.Multitasker;
using SAM.Core.Multitasker.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SAM.Multitasker
{
    public class Multitasker
    {
        private string code;
        private ScriptOptions scriptOptions = ScriptOptions.Default;
        private MultitaskerMode multitaskerMode = MultitaskerMode.Series;

        public Multitasker(string code, MultitaskerMode multitaskerMode = MultitaskerMode.Series)
        {
            this.code = code;
            this.multitaskerMode = multitaskerMode;
        }

        public async Task<MultitaskerResults> Run(IEnumerable<MultitaskerInput> multitaskerInputs)
        {
            if (code == null || scriptOptions == null || multitaskerInputs == null)
            {
                return default;
            }

            HashSet<string> names = new HashSet<string>();
            foreach(MultitaskerInput multitaskerInput in multitaskerInputs)
            {
                if(multitaskerInput.Variables != null)
                {
                    foreach(string name in multitaskerInput.Variables.Keys)
                    {
                        if(names.Contains(name))
                        {
                            continue;
                        }

                        code = code.Replace(string.Format("[{0}]", name), string.Format(@"Variables[""{0}""]", name));
                    }
                }
            }


            Script<object> script = CSharpScript.Create(code, scriptOptions);

            ImmutableArray<Diagnostic> immutableArray = script.Compile();
            if(immutableArray.Length != 0)
            {
                return new MultitaskerResults();
            }

            Func<MultitaskerInput, Task<MultitaskerResult>> func = async (x) => 
            {
                object @object = null;
                CompilationErrorException compilationErrorException = null;

                bool succedded = false;

                try
                {
                    @object = await script.RunAsync(x);
                    succedded = true;
                }
                catch (CompilationErrorException compilationErrorException_Temp)
                {
                    compilationErrorException = compilationErrorException_Temp;
                }
                catch (Exception exception)
                {

                }

                return new MultitaskerResult(x, succedded ? new MultitaskerOutput(@object) : null, compilationErrorException);
            };

            List<MultitaskerResult> multitaskerResults = Enumerable.Repeat<MultitaskerResult>(null, multitaskerInputs.Count()).ToList();

            if(multitaskerMode == MultitaskerMode.Series)
            {
                for(int i =0; i < multitaskerInputs.Count(); i++)
                {
                    multitaskerResults[i] = await func.Invoke(multitaskerInputs.ElementAt(i));
                }
            }
            else if(multitaskerMode == MultitaskerMode.Parallel)
            {
                Parallel.For(0, multitaskerResults.Count, async i =>
                {
                    multitaskerResults[i] = await func.Invoke(multitaskerInputs.ElementAt(i));
                });
            }

            return new MultitaskerResults(multitaskerResults);
        }

        public void AddReferences(params Assembly[] assemblies)
        {
            if(assemblies == null || assemblies.Length == 0)
            {
                return;
            }

            //.AddReferences(Assembly.Load("System.Runtime"))  // Reference system libraries
            //.AddReferences(Assembly.Load("Newtonsoft.Json"))  // Reference external libraries
            scriptOptions = scriptOptions.AddReferences(assemblies);
        }

        public void AddImports(params string[] imports)
        {
            if (imports == null || imports.Length == 0)
            {
                return;
            }

            //.AddImports("System", "Newtonsoft.Json", "SAM.Core");
            scriptOptions.AddImports(imports);
        }
    }
}
