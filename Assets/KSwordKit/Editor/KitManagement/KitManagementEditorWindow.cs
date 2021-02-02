
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
        public List<string> KitLocalResourceAllComponentsRootPathList; // 本地资源内的所有组件路径
        public string KitConfigFileName;
        public List<KitConfig> KitConfigList;
        public List<KitConfig> kitShouldShowConfigList;
        public string KitError;
        public uint KitMaxShowScrollItemCount;
    }

    public class KitManagementEditorWindow : EditorWindow
    {
        Vector2 scorllPos;
        static KitManagementEditorWindow window;
        static KitManagementEditorWindowData windowData;
        static string KitUserSearchDefaultInputString = "组件名或组件作者";

        public static void Open(KitManagementEditorWindowData data)
        {
            windowData = data;
            window = GetWindow<KitManagementEditorWindow>(false, windowData.TitleString);
            window.Show();

            UpdateData();
        }

        static void UpdateData()
        {
            var kitRootDir = windowData.KitLocalResourceRootDirectory;
            var configFileName = windowData.KitConfigFileName;
            var kitAllComponentsRootPathList = windowData.KitLocalResourceAllComponentsRootPathList;
            var kitConfigList = windowData.KitConfigList;
            if (!string.IsNullOrEmpty(kitRootDir))
            {
                var errorPath = "";
                foreach (var componentRootPath in kitAllComponentsRootPathList)
                {
                    if (!string.IsNullOrEmpty(componentRootPath))
                    {
                        var rootPath = System.IO.Path.Combine(kitRootDir, componentRootPath);
                        var rootDirInfo = new System.IO.DirectoryInfo(rootPath);
                        foreach(var rootDir in rootDirInfo.GetDirectories())
                        {
                            var configPath = System.IO.Path.Combine(rootDir.FullName, configFileName);
                            if (System.IO.File.Exists(configPath))
                            {
                                try
                                {
                                    var config = JsonUtility.FromJson<KitConfig>(System.IO.File.ReadAllText(configPath, System.Text.Encoding.UTF8));
                                    kitConfigList.Add(config);
                                }
                                catch (System.Exception e)
                                {
                                    errorPath += configPath + "\n";
                                    Debug.LogError(e.Message);
                                }

                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(errorPath))
                {
                    Debug.LogError("解析配置文件时失败：\n" + errorPath);
                }

                windowData.kitShouldShowConfigList.AddRange(kitConfigList);
            } 
            else
            {
                windowData.KitError = "本地资源路径值为空";
            }
        }

        private void OnGUI()
        { 
            //EditorGUILayout.BeginHorizontal();
            //GUILayout.Space(10);
            //GUILayout.Label(windowData.SubTitleString);
            //GUILayout.Space(10);
            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            KitUserSearchDefaultInputString = EditorGUILayout.TextField("所有组件：", KitUserSearchDefaultInputString);
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            // 显示所有组件
            if (windowData.KitConfigList.Count > windowData.KitMaxShowScrollItemCount)
                scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, true, GUILayout.Height(200));
            else
                scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, false, GUILayout.Height(200));
            foreach (var item in windowData.kitShouldShowConfigList)
            {
                addItem(item);
            }
            EditorGUILayout.EndScrollView();
        }


        void addItem(KitConfig config)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(config.Name, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();

            //var destPath = System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsDirectory, config.Name);
            //var isExists = System.IO.Directory.Exists(destPath);
            var buttonName = "导入";
            //if (isExists)
            //{
            //    buttonName = "重新导入";
            //}

            if (GUILayout.Button(buttonName, GUILayout.Width(110)))
            {
                //var error = config.Import(new System.IO.DirectoryInfo(System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsSourceDiretory, config.Name)).FullName, destPath);
                //EditorUtility.DisplayDialog("导入部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ? "导入成功！" : "导入失败: \n" + error, "确定");
                AssetDatabase.Refresh();
            }

            //EditorGUI.BeginDisabledGroup(!isExists);
            if (GUILayout.Button("删除", GUILayout.Width(110)))
            {
                //var error = config.Delete();
                //EditorUtility.DisplayDialog("删除部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ? "删除成功！" : "删除失败: \n" + error, "确定");
                AssetDatabase.Refresh();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }
    }
}