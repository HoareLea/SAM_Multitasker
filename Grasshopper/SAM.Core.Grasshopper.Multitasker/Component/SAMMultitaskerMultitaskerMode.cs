using SAM.Core.Grasshopper.Multitasker.Properties;
using SAM.Core.Multitasker;
using System;
using System.Drawing;
using System.IO;

namespace SAM.Core.Grasshopper.Multitasker
{
    public class SAMMultitaskerMultitaskerMode : GH_SAMEnumComponent<MultitaskerMode>
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1fd108c6-a6bf-478e-9a96-4604040801c6");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => new Bitmap(new MemoryStream(Resources.SAM_Small));

        /// <summary>
        /// About SAM Enum Component
        /// </summary>
        public SAMMultitaskerMultitaskerMode()
          : base("SAMMultitasker.MultitaskerMode", "SAMMultitasker.MultitaskerMode",
              "Right click to select MultitaskerMode",
              "SAM WIP", "Multitasker")
        {
        }
    }
}