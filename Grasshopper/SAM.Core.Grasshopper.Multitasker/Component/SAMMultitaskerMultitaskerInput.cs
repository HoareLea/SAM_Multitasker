﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Multitasker.Properties;
using SAM.Core.Multitasker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

                Param_GenericObject param_GenericObject = new Param_GenericObject() { Name = "value_", NickName = "value_", Description = "Value", Access = GH_ParamAccess.item, Optional = true };
                result.Add(new GH_SAMParam(param_GenericObject, ParamVisibility.Binding));

                Param_String param_String;

                param_String = new Param_String() { Name = "_name_", NickName = "_name_", Description = "Input name", Access = GH_ParamAccess.item, Optional = true };
                param_String.SetPersistentData(Core.Multitasker.Name.DefaultMultitaskerInput);
                result.Add(new GH_SAMParam(param_String, ParamVisibility.Binding));

                param_String = new Param_String() { Name = "_valueType_", NickName = "_valueType_", Description = "ValueType", Access = GH_ParamAccess.item, Optional = true };
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

            string name = null;
            index = Params.IndexOfInputParam("_name_");
            if (index == -1 || !dataAccess.GetData(index, ref name) || string.IsNullOrEmpty(name))
            {
                name = Core.Multitasker.Name.DefaultMultitaskerInput;
            }

            GH_ObjectWrapper gH_ObjectWrapper = null;
            index = Params.IndexOfInputParam("value_");
            if (index == -1 || !dataAccess.GetData(index, ref gH_ObjectWrapper))
            {
                gH_ObjectWrapper = null;
            }

            object value = gH_ObjectWrapper?.Value;
            if (value is IGH_Goo)
            {
                value = (value as dynamic).Value;
            }

            ValueType? valueType = null;

            string valueTypeString = null;
            index = Params.IndexOfInputParam("_valueType_");
            if (index == -1 || !dataAccess.GetData(index, ref valueTypeString) || string.IsNullOrEmpty(valueTypeString))
            {
                valueType = null;
            }
            else if(!string.IsNullOrWhiteSpace(valueTypeString))
            {
                valueType = Core.Query.Enum<ValueType>(valueTypeString);
            }

            index = Params.IndexOfOutputParam("multitaskerInput");
            if (index != -1)
            {
                MultitaskerInput multitaskerInput = valueType == null || !valueType.HasValue ? new MultitaskerInput(name, value) : new MultitaskerInput(name, valueType.Value, value);

                dataAccess.SetData(index, multitaskerInput);
            }
        }
    }
}