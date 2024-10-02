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
    public class GooMultitaskerVariable : GooJSAMObject<MultitaskerVariable>
    {
        public GooMultitaskerVariable()
            : base()
        {
        }

        public GooMultitaskerVariable(MultitaskerVariable multitaskerVariable)
            : base(multitaskerVariable)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMultitaskerVariable(Value);
        }

        public override bool CastFrom(object source)
        {
            object @object = source;
            if (@object is IGH_Goo)
            {
                @object = ((dynamic)@object).Value;
            }

            if (@object is MultitaskerVariable)
            {
                Value = new MultitaskerVariable((MultitaskerVariable)@object);
                return true;
            }
            else
            {
                Value = new MultitaskerVariable(Name.DefaultMultitaskerVariable, @object);
                return true;
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Y>(ref Y target)
        {
            if (typeof(Y) == typeof(MultitaskerVariable))
            {
                target = (Y)(object)Value;
            }

            return base.CastTo(ref target);
        }

        public override string TypeName
        {
            get
            {
                return Value == null ? typeof(MultitaskerVariable).Name : Value.GetType().Name;
            }
        }
    }

    public class GooMultitaskerVariableParam : GH_PersistentParam<GooMultitaskerVariable>
    {
        public override Guid ComponentGuid => new Guid("e2a46e73-b05f-427a-9c51-f6340f9fd98f");

        protected override Bitmap Icon => new Bitmap(new MemoryStream(Resources.SAM_Small));

        public GooMultitaskerVariableParam()
            : base("MultitaskerVariable", "MultitaskerVariable", "SAM Multitasker MultitaskerVariable", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMultitaskerVariable> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMultitaskerVariable value)
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