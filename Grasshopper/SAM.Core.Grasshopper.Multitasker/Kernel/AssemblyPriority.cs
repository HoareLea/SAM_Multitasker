using Grasshopper;
using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Multitasker.Properties;
using System.Drawing;
using System.IO;

namespace SAM.Core.Grasshopper.Multitasker
{
    public class AssemblyPriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon("SAM", new Bitmap(new MemoryStream(Resources.SAM_Small)));
            Instances.ComponentServer.AddCategorySymbolName("SAM", 'S');
            return GH_LoadingInstruction.Proceed;
        }
    }
}