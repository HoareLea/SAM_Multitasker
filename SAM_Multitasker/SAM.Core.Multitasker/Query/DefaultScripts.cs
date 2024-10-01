using System.Collections.Generic;
using System.Reflection;

namespace SAM.Core.Multitasker
{
    public static partial class Query
    {
        public static List<Script> DefaultScripts()
        {
            string resourcesDirectory = Core.Query.ResourcesDirectory(Assembly.GetExecutingAssembly());
            if (string.IsNullOrWhiteSpace(resourcesDirectory))
            {
                return null;
            }

            string scriptsDirectory = System.IO.Path.Combine(resourcesDirectory, "scripts");
            if(scriptsDirectory == null || !System.IO.Directory.Exists(scriptsDirectory))
            {
                return null;
            }

            if(!System.IO.Directory.Exists(scriptsDirectory))
            {
                return null;
            }

            string[] paths = System.IO.Directory.GetFiles(scriptsDirectory, "*.cs");
            if(paths == null || paths.Length == 0)
            {
                return null;
            }

            List<Script> result = new List<Script>();
            foreach(string path in paths)
            {
                string code = System.IO.File.ReadAllText(path);

                string name = System.IO.Path.GetFileNameWithoutExtension(path);

                result.Add(new Script(ProgrammingLanguage.CSharp, name, code));
            }

            return result;
        }
    }
}
