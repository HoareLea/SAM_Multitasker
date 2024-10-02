using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using SAM.Core.Grasshopper.Multitasker.Properties;
using SAM.Core.Multitasker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SAM.Core.Grasshopper.Multitasker
{
    public class SAMMultitaskerDefaultScripts : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("97d3d237-ec7b-421e-913c-0a692a244980");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => new Bitmap(new MemoryStream(Resources.SAM_Small));

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public SAMMultitaskerDefaultScripts()
          : base("SAMMultitasker.DefaultScripts", "SAMMultitasker.DefaultScripts",
              "Returns DefaultScripts",
              "SAM WIP", "Multitasker")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();

                Param_String param_String = new Param_String() { Name = "names_", NickName = "names_", Description = "Script Names", Access = GH_ParamAccess.list, Optional = true };
                result.Add(new GH_SAMParam(param_String, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();

                result.Add(new GH_SAMParam(new GooScriptParam() { Name = "scripts", NickName = "scripts", Description = "Scripts", Access = GH_ParamAccess.list }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">
        /// The DA object is used to retrieve from inputs and store in outputs.
        /// </param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index = -1;

            List<string> names = null;
            index = Params.IndexOfInputParam("names_");
            if (index != -1)
            {
                names = new List<string>();

                if(!dataAccess.GetDataList(index, names) || names.Count == 0)
                {
                    names = null;
                }
            }

            List<Script> scripts = Core.Multitasker.Query.DefaultScripts();
            if(names != null)
            {
                scripts = scripts.FindAll(x => names.Contains(x.Name));
            }


            index = Params.IndexOfOutputParam("scripts");
            if (index != -1)
            {
                dataAccess.SetDataList(index, scripts);
            }
        }
    }
}