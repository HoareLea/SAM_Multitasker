using System.Reflection;

namespace SAM.Core.Multitasker
{
    public static partial class ActiveSetting
    {
        private static Setting setting = Load();

        private static Setting Load()
        {
            Setting setting = ActiveManager.GetSetting(Assembly.GetExecutingAssembly());
            if (setting == null)
                setting = GetDefault();

            return setting;
        }

        public static Setting Setting
        {
            get
            {
                return setting;
            }
        }

        public static Setting GetDefault()
        {
            Setting result = new Setting(Assembly.GetExecutingAssembly());

            ////File Names
            //result.SetValue(AnalyticalSettingParameter.DefaultMaterialLibraryFileName, "SAM_MaterialLibrary.JSON");


            //string path = null;

            //path = Query.DefaultPath(result, AnalyticalSettingParameter.DefaultNCMNameCollectionFileName);
            //if (System.IO.File.Exists(path))
            //    result.SetValue(AnalyticalSettingParameter.DefaultNCMNameCollection, Core.Create.IJSAMObject<NCMNameCollection>(System.IO.File.ReadAllText(path)));

            return result;
        }
    }
}