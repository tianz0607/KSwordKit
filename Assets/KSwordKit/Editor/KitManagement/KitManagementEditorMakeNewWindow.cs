using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KSwordKit.Editor.KitManagement
{
    public class KitManagementEditorMakeNewWindow : EditorWindow
    {
        static KitManagementEditorMakeNewWindow window;
        static KitManagementEditorWindowData windowData;
        static KitConfig newConfig;
        const string newConfigName = "输入新组件名称";
        static string kitUserInputNewConfigName = newConfigName;
        const string newConfigAuthor = "输入新组件作者";
        static string kitUserInputNewConfigAuthor = newConfigAuthor;
        const string newConfigContact = "输入新组件作者的联系方式";
        static string kitUserInputNewConfigContact = newConfigContact;
        const string newConfigHomePage = "输入新组件作者的个人主页";
        static string kitUserInputNewConfigHomePage = newConfigHomePage;
        const string newConfigVersion = "输入新组件的版本号";
        static string kitUserInputNewConfigVersion = newConfigVersion;
        const string newConfigDescription = "请描述这个新组件";
        static string kitUserInputNewConfigDescription = newConfigDescription;
        const string newConfigDependencies = "输入新组件的依赖列表";
        static string kitUserInputNewConfigDependencies = newConfigDependencies;
        const string newConfigFileSettings = "输入新组件内的文件设置";
        static string kitUserInputNewConfigFileSettings = newConfigFileSettings;
        const string kitTempFileName = "kswordkit.tempfile";
        static string kitUserSelectedComponentSrcPath = "";

        /// <summary>
        /// 窗口打开显示函数
        /// </summary>
        /// <param name="data">窗口数据</param>
        public static void Open(KitManagementEditorWindowData data)
        {
            windowData = data;
            var windowTitle = windowData.TitleString + ": " + windowData.SubTitleString;
            window = GetWindow<KitManagementEditorMakeNewWindow>(true, windowTitle);
            var tempfilePath = System.IO.Path.Combine(Application.temporaryCachePath, kitTempFileName);
            if (System.IO.File.Exists(tempfilePath))
                System.IO.File.Delete(tempfilePath);
            System.IO.File.WriteAllText(tempfilePath, windowTitle + "\n" + windowData.SubTitleString);
            window.minSize = new Vector2(800, 500);
            newConfig = new KitConfig();
            window.Show();
        }

        private void OnGUI()
        {
            if (window == null || windowData == null)
            {
                var tempfilePath = System.IO.Path.Combine(Application.temporaryCachePath, kitTempFileName);
                if (System.IO.File.Exists(tempfilePath))
                {
                    var tempfilelines = System.IO.File.ReadAllLines(tempfilePath);
                    var windowtitle = tempfilelines[0];
                    GetWindow<KitManagementEditorWindow>(true, windowtitle).Close();
                    KitManagementEditor.MakeNewFunction();
                }
                return;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("填写新组件如下各项配置值");
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigName = EditorGUILayout.TextField("组件名称：", kitUserInputNewConfigName, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigAuthor = EditorGUILayout.TextField("组件作者：", kitUserInputNewConfigAuthor, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigContact = EditorGUILayout.TextField("组件作者的联系方式：", kitUserInputNewConfigContact, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigHomePage = EditorGUILayout.TextField("组件作者的个人主页：", kitUserInputNewConfigHomePage, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigVersion = EditorGUILayout.TextField("组件版本号：", kitUserInputNewConfigVersion, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigDescription = EditorGUILayout.TextField("组件简介：", kitUserInputNewConfigDescription, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("组件源码位置: ", GUILayout.Height(20));
            var buttonName = "浏览位置";
            if (!string.IsNullOrEmpty(kitUserSelectedComponentSrcPath))
                buttonName = "重新选择位置";

            if (GUILayout.Button(buttonName, GUILayout.Width(120), GUILayout.Height(20)))
            {
                kitUserSelectedComponentSrcPath = EditorUtility.OpenFolderPanel("选择组件源码位置", Application.dataPath, "NewComponentFolder");
            }
            EditorGUILayout.LabelField(kitUserSelectedComponentSrcPath, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

        }
    }
}

