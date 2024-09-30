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
    public class GooMultitaskerOutput : GooJSAMObject<MultitaskerOutput>
    {
        public GooMultitaskerOutput()
            : base()
        {
        }

        public GooMultitaskerOutput(MultitaskerOutput multitaskerOutput)
            : base(multitaskerOutput)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMultitaskerOutput(Value);
        }

        public override string TypeName
        {
            get
            {
                return Value == null ? typeof(MultitaskerOutput).Name : Value.GetType().Name;
            }
        }
    }

    public class GooMultitaskerOutputParam : GH_PersistentParam<GooMultitaskerOutput>
    {
        public override Guid ComponentGuid => new Guid("cfc14cb0-df0a-4774-858a-7a652551b3bb");

        protected override Bitmap Icon => new Bitmap(new MemoryStream(Resources.SAM_Small));

        public GooMultitaskerOutputParam()
            : base("MultitaskerOutput", "MultitaskerOutput", "SAM Multitasker MultitaskerOutput", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMultitaskerOutput> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMultitaskerOutput value)
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