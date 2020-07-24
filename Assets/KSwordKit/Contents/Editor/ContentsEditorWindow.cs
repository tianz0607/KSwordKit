/*************************************************************************
 *  Copyright (C), 2020-2021. All rights reserved.
 *
 *  FileName: ContentsWindow.cs
 *  Author: ks   
 *  Version: 1.0.0   
 *  CreateDate: 2020-7-9
 *  File Description: Ignore.
 *************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KSwordKit.Contents.Editor
{
    public class ImportChildWindow : EditorWindow
    {
        List<ImportConfig> list = null;
        Vector2 scorllPos;
        static ImportChildWindow window;

        public static void Open()
        {
         
            window = GetWindow<ImportChildWindow>(false, KSwordKitConst.KSwordKitName+": "+ ContentsEditor.ImportWindowTitle);
            window.list = new List<ImportConfig>();
            window.initData();
            window.Show();
        }

        private void OnGUI()
        { 
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("所有可用部件如下：");
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            if (list.Count > 9)
                scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, true, GUILayout.Height(200));
            else
                scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, false, GUILayout.Height(200));

            foreach (var item in list)
            {
                addItem(item);
            }
            EditorGUILayout.EndScrollView();
        }

        void initData()
        {
            var dirInfo = new System.IO.DirectoryInfo(KSwordKit.KSwordKitConst.KSwordKitContentsSourceDiretory);
            foreach (var dir in dirInfo.GetDirectories())
            {
                var filePath = System.IO.Path.Combine(dir.FullName, ContentsEditor.ImportConfigFileName);
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        var importConfig = JsonUtility.FromJson<ImportConfig>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
                        list.Add(importConfig);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(KSwordKitConst.KSwordKitName + ": 导入部件时出错, " + e.Message);
                    }
                }
            }
        }

        void addItem(ImportConfig config)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(config.Name, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();

            var destPath = System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsDirectory, config.Name);
            var isExists = System.IO.Directory.Exists(destPath);
            var buttonName = "导入";
            if (isExists)
            {
                buttonName = "重新导入";
            }

            if (GUILayout.Button(buttonName, GUILayout.Width(110)))
            {
                var error = config.Import(new System.IO.DirectoryInfo(System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsSourceDiretory, config.Name)).FullName, destPath);
                EditorUtility.DisplayDialog("导入部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ?"导入成功！": "导入失败: \n" + error, "确定");
                AssetDatabase.Refresh();
            }

            EditorGUI.BeginDisabledGroup(!isExists);
            if (GUILayout.Button("删除", GUILayout.Width(110)))
            {
                var error = config.Delete();
                EditorUtility.DisplayDialog("删除部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ? "删除成功！" : "删除失败: \n" + error, "确定");
                AssetDatabase.Refresh();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }
    }
}