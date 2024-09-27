using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using SAM.Analytical.Grasshopper.Multitasker.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SAM.Core.Grasshopper.Multitasker
{
    public class SAMMultitaskerRun : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("f2fa4316-2d2c-426c-ab5f-efeb65c72456");

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
        public SAMMultitaskerRun()
          : base("SAMMultitasker.Run", "SAMMultitasker.Run",
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
                result.Add(new GH_SAMParam(new Param_GenericObject() { Name = "inputs_", NickName = "inputs_", Description = "Inputs", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));

                Param_String param_String = new Param_String() { Name = "_script", NickName = "_script", Description = "Script", Access = GH_ParamAccess.item };
                result.Add(new GH_SAMParam(param_String, ParamVisibility.Binding));
                

                Param_Boolean param_Boolean = new Param_Boolean() { Name = "_run_", NickName = "_run_", Description = "Run", Access = GH_ParamAccess.item };
                param_Boolean.SetPersistentData(false);

                result.Add(new GH_SAMParam(param_Boolean, ParamVisibility.Binding));
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

                result.Add(new GH_SAMParam(new Param_GenericObject() { Name = "outputs", NickName = "outputs", Description = "Outputs", Access = GH_ParamAccess.list }, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new Param_String() { Name = "errors", NickName = "errors", Description = "Errors", Access = GH_ParamAccess.list }, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new Param_Boolean() { Name = "successful", NickName = "successful", Description = "Successful", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
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

            bool run = false;
            index = Params.IndexOfInputParam("_run_");
            if (index == -1 || !dataAccess.GetData(index, ref run) || !run)
            {
                dataAccess.SetData(0, false);
                return;
            }


            string script = null;
            index = Params.IndexOfInputParam("_script");
            if (index == -1 || !dataAccess.GetData(index, ref script) || string.IsNullOrEmpty(script))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            Core.Multitasker.Multitasker multitasker = new Core.Multitasker.Multitasker(script);

            List<Assembly> assemblies = new List<Assembly>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(assembly == null || assembly.IsDynamic)
                {
                    continue;
                }

                assemblies.Add(assembly);
            }

            multitasker.AddReferences(assemblies?.ToArray());

            Task<Core.Multitasker.MultitaskerResults> task = multitasker.Run();

            bool succedded = false;
            if(task != null)
            {
                Core.Multitasker.MultitaskerResults multitaskerResults = task.Result;
                if(multitaskerResults != null)
                {
                    succedded = multitaskerResults.Succedded;

                    index = Params.IndexOfOutputParam("errors");
                    if (index != -1)
                    {
                        dataAccess.SetDataList(index, multitaskerResults.Diagnostics?.ConvertAll(x => x.GetMessage()));
                    }
                }
            }

            index = Params.IndexOfOutputParam("successful");
            if (index != -1)
            {
                dataAccess.SetData(index, succedded);
            }
        }
    }
}