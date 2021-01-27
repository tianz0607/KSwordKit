
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KSwordKit.Editor.KitManagement
{
    public class KitManagementEditorWindowData
    {
        public string TitleString;
        public string SubTitleString;
        public string KitLocalResourceRootDirectory;
        public string KitLocalResourceCoreDirectory;
        public string KitLocalResourceModulesDirectory;
        public string ModuleConfigFileName;
    }

    public class KitManagementEditorWindow : EditorWindow
    {
        Vector2 scorllPos;
        static KitManagementEditorWindow window;
        static KitManagementEditorWindowData kitManagementEditorWindowData;

        public static void Open(KitManagementEditorWindowData data)
        {
            kitManagementEditorWindowData = data;
            window = GetWindow<KitManagementEditorWindow>(false, kitManagementEditorWindowData.TitleString);
            window.Show();

            InitData();
        }

        static void InitData()
        {
            var kitdir = kitManagementEditorWindowData.KitLocalResourceRootDirectory;
            var configFileName = kitManagementEditorWindowData.ModuleConfigFileName;
            var done = false;
            string error = null;
            if (string.IsNullOrEmpty(kitdir))
            {
                done = true;
                error = "本地资源路径值为空";
                
            }

            var kitCoreDir = kitManagementEditorWindowData.KitLocalResourceCoreDirectory;
            var kitModules = kitManagementEditorWindowData.KitLocalResourceModulesDirectory;


            //    var dirInfo = new System.IO.DirectoryInfo(kitdir);
            //    foreach (var dir in dirInfo.GetDirectories())
            //    {
            //        var filePath = System.IO.Path.Combine(dir.FullName, configFileName);
            //        if (System.IO.File.Exists(filePath))
            //        {
            //            try
            //            {
            //                var importConfig = JsonUtility.FromJson<ModuleConfig>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
            //                list.Add(importConfig);
            //            }
            //            catch (System.Exception e)
            //            {
            //                Debug.LogError("解析模块配置文件时出错, " + e.Message);
            //            }
            //        }
            //    }
        }

        private void OnGUI()
        { 
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label(kitManagementEditorWindowData.SubTitleString);
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            //if (list.Count > 9)
            //    scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, true, GUILayout.Height(200));
            //else
            //    scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, false, GUILayout.Height(200));

            //foreach (var item in list)
            //{
            //    addItem(item);
            //}
            //EditorGUILayout.EndScrollView();
        }

        //void initData()
        //{
        //    var dirInfo = new System.IO.DirectoryInfo(KSwordKit.KSwordKitConst.KSwordKitContentsSourceDiretory);
        //    foreach (var dir in dirInfo.GetDirectories())
        //    {
        //        var filePath = System.IO.Path.Combine(dir.FullName, ContentsEditor.ImportConfigFileName);
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            try
        //            {
        //                var importConfig = JsonUtility.FromJson<ImportConfig>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
        //                list.Add(importConfig);
        //            }
        //            catch (System.Exception e)
        //            {
        //                Debug.LogError(KSwordKitConst.KSwordKitName + ": 导入部件时出错, " + e.Message);
        //            }
        //        }
        //    }
        //}

        //void addItem(ImportConfig config)
        //{
        //    EditorGUILayout.BeginHorizontal();

        //    GUILayout.Label(config.Name, EditorStyles.boldLabel);
        //    GUILayout.FlexibleSpace();

        //    var destPath = System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsDirectory, config.Name);
        //    var isExists = System.IO.Directory.Exists(destPath);
        //    var buttonName = "导入";
        //    if (isExists)
        //    {
        //        buttonName = "重新导入";
        //    }

        //    if (GUILayout.Button(buttonName, GUILayout.Width(110)))
        //    {
        //        var error = config.Import(new System.IO.DirectoryInfo(System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsSourceDiretory, config.Name)).FullName, destPath);
        //        EditorUtility.DisplayDialog("导入部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ?"导入成功！": "导入失败: \n" + error, "确定");
        //        AssetDatabase.Refresh();
        //    }

        //    EditorGUI.BeginDisabledGroup(!isExists);
        //    if (GUILayout.Button("删除", GUILayout.Width(110)))
        //    {
        //        var error = config.Delete();
        //        EditorUtility.DisplayDialog("删除部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ? "删除成功！" : "删除失败: \n" + error, "确定");
        //        AssetDatabase.Refresh();
        //    }
        //    EditorGUI.EndDisabledGroup();

        //    EditorGUILayout.EndHorizontal();
        //}
    }
}