// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors

using System.Reflection;

namespace SAM.Core.Multitasker
{
    public static partial class ActiveSetting
    {
        private static Setting setting = null;

        private static readonly object settingLock = new ();

        private static Setting Load()
        {
            Setting setting = ActiveManager.GetSetting(Assembly.GetExecutingAssembly());
            if (setting == null)
            {
                setting = GetDefault();
            }

            return setting;
        }

        public static Setting Setting
        {
            get
            {
                lock (settingLock)
                {
                    setting ??= Load();
                }

                return setting;
            }
        }

        public static Setting GetDefault()
        {
            Setting result = new (Assembly.GetExecutingAssembly());

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