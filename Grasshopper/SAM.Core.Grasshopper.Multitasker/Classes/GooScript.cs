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
    public class GooScript : GooJSAMObject<Script>
    {
        public GooScript()
            : base()
        {
        }

        public GooScript(Script script)
            : base(script)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooScript(Value);
        }

        public override bool CastFrom(object source)
        {
            object @object = source;
            if(@object is IGH_Goo)
            {
                @object = ((dynamic)@object).Value;
            }
            
            if(@object is string)
            {
                Value = new Script((string)@object);
                return true;
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Y>(ref Y target)
        {
            if(typeof(Y) == typeof(string))
            {
                target = (Y)(object)Value.Code;
            }

            return base.CastTo(ref target);
        }

        public override string TypeName
        {
            get
            {
                return Value == null ? typeof(GooScript).Name : Value.GetType().Name;
            }
        }
    }

    public class GooScriptParam : GH_PersistentParam<GooScript>
    {
        public override Guid ComponentGuid => new Guid("400a16f6-a61d-4848-a9d6-6f0ade098ba3");

        protected override Bitmap Icon => new Bitmap(new MemoryStream(Resources.SAM_Small));

        public GooScriptParam()
            : base("Script", "Script", "SAM Script", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooScript> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooScript value)
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