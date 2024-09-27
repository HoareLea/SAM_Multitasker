using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SAM.Core.Multitasker
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

        public async Task<MultitaskerResults> Run(IEnumerable<MultitaskerInput> multitaskerInputs = null)
        {
            if (code == null || scriptOptions == null)
            {
                return default;
            }

            HashSet<string> names = new HashSet<string>();
            if(multitaskerInputs != null)
            {
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
            }


            Script<object> script = CSharpScript.Create(code, scriptOptions);

            ImmutableArray<Diagnostic> immutableArray = script.Compile();
            if(immutableArray.Length != 0)
            {
                return new MultitaskerResults(immutableArray);
            }

            Func<MultitaskerInput, Task<MultitaskerResult>> func = async (x) => 
            {
                object @object = null;
                CompilationErrorException compilationErrorException = null;

                bool succedded = false;

                try
                {
                    @object = await script.RunAsync(globals: x);
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

            if(multitaskerInputs == null || multitaskerInputs.Count() == 0)
            {
                MultitaskerResult multitaskerResult = await func.Invoke(null);
                return new MultitaskerResults(new MultitaskerResult[] { multitaskerResult });
            }

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

            List<Assembly> assemblies_Temp = new List<Assembly>();
            foreach(Assembly assembly in assemblies)
            {
                if(assembly == null || assembly.IsDynamic)
                {
                    continue;
                }

                assemblies_Temp.Add(assembly);  
            }

            if(assemblies_Temp.Count == 0)
            {
                return;
            }

            //.AddReferences(Assembly.Load("System.Runtime"))  // Reference system libraries
            //.AddReferences(Assembly.Load("Newtonsoft.Json"))  // Reference external libraries
            scriptOptions = scriptOptions.AddReferences(assemblies_Temp);
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
