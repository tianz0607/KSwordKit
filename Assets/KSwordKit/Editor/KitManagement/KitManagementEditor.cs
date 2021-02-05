using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KSwordKit.Editor.KitManagement
{
    public class KitManagementEditor
    {
        static KitManagementEditorWindowData kitManagementEditorWindowData = new KitManagementEditorWindowData {
            TitleString = Const.ConfigurationConst.KitName,
            SubTitleString = "",
            KitLocalResourceRootDirectory = Const.ConfigurationConst.KitLocalResourceDirectory,
            KitLocalResourceAllComponentsRootPathList = new List<string>() { "Basic", "Framework" },
            KitComponentInstallRootDirectory = Const.ConfigurationConst.KitComponentInstallRootDirectory,
            KitConfigFileName = "config.json",
            KitConfigList = new List<KitConfig>(),
            kitShouldShowConfigList = new List<KitConfig>(),
            KitError = null,
            KitMaxShowScrollItemCount = 20
        };


        public const string ImportChild_Assets = "Assets/KSwordKit/框架管理/安装组件";
        public const string ImportChild = "KSwordKit/框架管理/安装组件 _%#I";
        public const string InstallComponentWindowTitle = "安装组件";

        public const string DeleteChild_AlreadyImport_Assets = "Assets/KSwordKit/框架管理/卸载组件";
        public const string DeleteChild_AlreadyImport = "KSwordKit/框架管理/卸载组件 _%#D";
        public const string UninstallComponentWindowTitle = "卸载组件";

        public const string MakeNew_Assets = "Assets/KSwordKit/框架管理/制作新组件";
        public const string MakeNew = "KSwordKit/框架管理/制作新组件 _%&N";
        public const string MakeNewWindowTitle = "制作新模块";

        public const string About_Assets = "Assets/KSwordKit/框架管理/关于作者";
        public const string AboutUs = "KSwordKit/框架管理/关于作者 _%&M";
        public const string AboutUsWindowTitle = "关于作者";

        public const string Update_Assets = "Assets/KSwordKit/框架管理/更新框架";
        public const string Update = "KSwordKit/框架管理/更新框架 _%#U";


        [MenuItem(ImportChild_Assets, false, 0)]
        [MenuItem(ImportChild, false, 0)]
        public static void InstallComponentFunction()
        {
            kitManagementEditorWindowData.SubTitleString = InstallComponentWindowTitle;
            KitManagementEditorWindow.Open(kitManagementEditorWindowData);
        }

        [MenuItem(DeleteChild_AlreadyImport_Assets, false, 1)]
        [MenuItem(DeleteChild_AlreadyImport, false, 1)]
        public static void UninstallComponentFunction()
        {
            kitManagementEditorWindowData.SubTitleString = UninstallComponentWindowTitle;
            KitManagementEditorWindow.Open(kitManagementEditorWindowData);
        }

        [MenuItem(MakeNew_Assets, false, 20)]
        [MenuItem(MakeNew, false, 20)]
        public static void MakeNewFunction()
        {
            //MakeNewComponentEditorWindow.Open();
        }

        [MenuItem(About_Assets, false, 40)]
        [MenuItem(AboutUs, false, 40)]
        public static void AboutFunction()
        {
            Application.OpenURL("https://github.com/keenlovelife");
        }

        [MenuItem(Update_Assets, false, 41)]
        [MenuItem(Update, false, 41)]
        public static void UpdateFunction()
        {
            Application.OpenURL("https://github.com/keenlovelife/KSwordKit.git");
        }

    }
}

