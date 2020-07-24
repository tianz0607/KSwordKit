using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace KSwordKit.Contents.Editor
{
    public class MakeNewComponentEditorWindow : EditorWindow
    {
        static MakeNewComponentEditorWindow window;

        public static void Open()
        {

            window = GetWindow<MakeNewComponentEditorWindow>(false, KSwordKitConst.KSwordKitName + ": " + ContentsEditor.MakeNewWindowTitle);

            window.Show();
        }

        string defaultImportConfigContent = @"
{
    ""Name"":""#NewName#"",
    ""Dependencies"":[],
    ""ExampleFolderPaths"":[],
    ""FileSettingList"":[]
}
";
        string newComponentPath = string.Empty;
        string name = "";
        string dependencies = "";
        string exampleFolderPaths = "";
        string reSelect = "";
        bool isCreateNew = false;
        string newComonentContent = string.Empty;
        bool errorloged;
        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            GUILayout.Label("新部件配置文件: " + newComponentPath);
            reSelect = string.IsNullOrEmpty(newComponentPath) ? "已存在新部件配置文件" : "重新已存在的新部件配置文件";
            EditorGUILayout.BeginHorizontal();
            if (isCreateNew)
            {
                if (GUILayout.Button("删除生成的配置文件"))
                {
                    System.IO.File.Delete(newComponentPath);
                    System.IO.File.Delete(newComponentPath + ".meta");
                    newComponentPath = string.Empty;
                    reSelect = string.Empty;
                    isCreateNew = false;
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                if (GUILayout.Button(reSelect))
                {
                    newComponentPath = EditorUtility.OpenFilePanel("选择已存在的新部件配置文件", "Assets", "json");
                }
                if (reSelect == "重新已存在的新部件配置文件")
                {
                    if (GUILayout.Button("重置"))
                    {
                        newComponentPath = string.Empty;
                        reSelect = string.Empty;
                    }
                }
                else
                {
                    if (GUILayout.Button("创建新部件的配置文件"))
                    {
                        newComponentPath = EditorUtility.SaveFilePanel("保存配置文件", "Assets", "ImportConfig", "json");
                        if (!string.IsNullOrEmpty(newComponentPath))
                        {
                            isCreateNew = true;
                            System.IO.File.WriteAllText(newComponentPath, defaultImportConfigContent);
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(newComponentPath))
            {
                newComonentContent = System.IO.File.ReadAllText(newComponentPath);
                newComonentContent = EditorGUILayout.TextArea(newComonentContent, GUILayout.Height(200));
                System.IO.File.WriteAllText(newComponentPath, newComonentContent);
                ImportConfig config = null;
                try
                {
                   config = JsonUtility.FromJson<ImportConfig>(newComonentContent);
                  
                }catch(System.Exception e)
                {
                    Debug.LogError(KSwordKitConst.KSwordKitName + ": json格式不正确！"+ e.Message);
                    errorloged = true;
                    return;
                }
                if (errorloged)
                {
                    errorloged = false;
                    
                    var assembly = System.Reflection.Assembly.GetAssembly(typeof(ActiveEditorTracker));
                    var type = assembly.GetType("UnityEditorInternal.LogEntries");
                    if (type == null)
                    {
                        type = assembly.GetType("UnityEditor.LogEntries");
                    }
                    var method = type.GetMethod("Clear");
                    method.Invoke(new object(), null);
                }
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("部件名称：", config.Name);
                EditorGUILayout.LabelField("部件目录：", new System.IO.FileInfo(newComponentPath).Directory.FullName);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("依赖：");
                var dependencies = string.Empty;
                foreach(var d in config.Dependencies)
                {
                    if (string.IsNullOrEmpty(dependencies)) dependencies = d;
                    else dependencies += "\n" + d;
                }
                if (string.IsNullOrEmpty(dependencies))
                    dependencies = "\t无";
                EditorGUILayout.LabelField(dependencies);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("示例代码位置：");
                var exampleFolderPaths = string.Empty;
                foreach (var e in config.ExampleFolderPaths)
                {
                    if (string.IsNullOrEmpty(exampleFolderPaths)) exampleFolderPaths = e;
                    else exampleFolderPaths += "\n" + e;
                }
                if (string.IsNullOrEmpty(exampleFolderPaths))
                    exampleFolderPaths = "\t无";
                EditorGUILayout.LabelField(exampleFolderPaths);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("特殊文件设置: ");
                var filesettings = string.Empty;
                foreach (var s in config.FileSettings)
                {
                    var str = (s.IsDir ? "文件夹：" : "文件：") + s.Path + " -> " + s.ImportPath;
                    if (string.IsNullOrEmpty(filesettings)) filesettings = str;
                    else filesettings += "\n" + str;
                }
                if (string.IsNullOrEmpty(filesettings))
                    filesettings = "\t无";
                EditorGUILayout.LabelField(filesettings);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                bool exportSuccess = false;
                if (GUILayout.Button("导出新部件"))
                {
                    exportSuccess = ExportNewComponent(config, newComponentPath);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);

            }
        }

        bool ExportNewComponent(ImportConfig importConfig, string importConfigPath)
        {
            var _dirinfo = new System.IO.DirectoryInfo(KSwordKitConst.KSwordKitContentsSourceDiretory);
            var _names = new List<string>();
            foreach(var dir in _dirinfo.GetDirectories())
            {
                var config = JsonUtility.FromJson<ImportConfig>(System.IO.File.ReadAllText(System.IO.Path.Combine(dir.FullName, ContentsEditor.ImportConfigFileName)));
                _names.Add(config.Name);
                if(config.Name == importConfig.Name)
                {
                    EditorUtility.DisplayDialog("导出新部件 '" + importConfig.Name + "' ", "失败：部件名称冲突，请使用其他名称重试！", "确定");
                    return false;
                }
            }

            foreach(var name in importConfig.Dependencies)
            {
                if (!_names.Contains(name))
                {
                    if(EditorUtility.DisplayDialog("导出新部件 '" + importConfig.Name + "' ", "依赖项 `" + name + "` 在当前框架中不存在，请更新框架或修改配置。", "更新框架", "修改配置"))
                    {
                        Application.OpenURL("https://github.com/keenlovelife/KSwordKit.git");
                    }
                    return false;
                }
            }

            var importConfigDirinfo = new System.IO.FileInfo(importConfigPath).Directory;
            foreach(var filesetting in importConfig.FileSettings)
            {
                bool isError = false;
                if (filesetting.IsDir)
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.Combine(importConfigDirinfo.FullName, filesetting.Path)))
                        isError = true;
                }
                else
                {
                    if (!System.IO.File.Exists(System.IO.Path.Combine(importConfigDirinfo.FullName, filesetting.Path)))
                        isError = true;
                }
                if (isError)
                {
                    EditorUtility.DisplayDialog("导出新部件 '" + importConfig.Name + "' ", "特殊文件配置出错: \nFileSettings项中 `Path: " + filesetting.Path +"` 并不是一个真实的" + (filesetting.IsDir?"文件夹":"文件") + "\n\n请修改配置。", "确定");
                    return false;
                }
            }

            var dirinfo = new System.IO.FileInfo(newComponentPath).Directory;
            var error = importConfig.Export(dirinfo.FullName, System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsSourceDiretory, importConfig.Name), importConfigPath);
            EditorUtility.DisplayDialog("导出新部件 '" + importConfig.Name + "' ", string.IsNullOrEmpty(error) ? "导出成功！" : "导出失败：" + error, "确定");
            return string.IsNullOrEmpty(error);
        }

    }

}