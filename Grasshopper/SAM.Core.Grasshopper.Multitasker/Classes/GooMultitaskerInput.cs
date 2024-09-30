using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Multitasker.Properties;
using SAM.Core.Multitasker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SAM.Core.Grasshopper.Multitasker
{
    public class GooMultitaskerInput : GooJSAMObject<MultitaskerInput>
    {
        public GooMultitaskerInput()
            : base()
        {
        }

        public GooMultitaskerInput(MultitaskerInput multitaskerInput)
            : base(multitaskerInput)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMultitaskerInput(Value);
        }

        public override string TypeName
        {
            get
            {
                return Value == null ? typeof(MultitaskerInput).Name : Value.GetType().Name;
            }
        }
    }

    public class GooMultitaskerInputParam : GH_PersistentParam<GooMultitaskerInput>
    {
        public override Guid ComponentGuid => new Guid("fdac916f-3e4f-4df6-8029-ea950823b4e8");

        protected override Bitmap Icon => new Bitmap(new MemoryStream(Resources.SAM_Small));

        public GooMultitaskerInputParam()
            : base("MultitaskerInput", "MultitaskerInput", "SAM Multitasker MultitaskerInput", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMultitaskerInput> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMultitaskerInput value)
        {
            throw new NotImplementedException();
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Save As...", Menu_SaveAs, VolatileData.AllData(true).Any());

            //Menu_AppendSeparator(menu);

            base.AppendAdditionalMenuItems(menu);
        }

        private void Menu_SaveAs(object sender, EventArgs e)
        {
            Query.SaveAs(VolatileData);
        }
    }
}