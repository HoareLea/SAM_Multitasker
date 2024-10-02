using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Multitasker.Properties;
using SAM.Core.Multitasker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SAM.Core.Grasshopper.Multitasker
{
    public class SAMMultitaskerMultitaskerInput : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("14c83218-ad03-48b4-8e1f-e24bc371be6b");

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
        public SAMMultitaskerMultitaskerInput()
          : base("SAMMultitasker.MultitaskerInput", "SAMMultitasker.MultitaskerInput",
              "Runs Multitasker",
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

                result.Add(new GH_SAMParam(new GooMultitaskerVariableParam() { Name = "multitaskerVariables_", NickName = "multitaskerVariables_", Description = "SAM Multitasker multitaskerVariable list", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));

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

                result.Add(new GH_SAMParam(new GooMultitaskerInputParam() { Name = "multitaskerInput", NickName = "multitaskerInput", Description = "MultitaskerInput", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
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

            List<MultitaskerVariable> multitaskerVariables = null;

            index = Params.IndexOfInputParam("multitaskerVariables_");
            if (index == -1)
            {
                multitaskerVariables = new List<MultitaskerVariable>();

                if(!dataAccess.GetDataList(index, multitaskerVariables) || multitaskerVariables == null || multitaskerVariables.Count == 0)
                {
                    multitaskerVariables = null;
                }
            }

            index = Params.IndexOfOutputParam("multitaskerInput");
            if (index != -1)
            {
                MultitaskerInput multitaskerInput = new MultitaskerInput(multitaskerVariables);

                dataAccess.SetData(index, multitaskerInput);
            }
        }
    }
}