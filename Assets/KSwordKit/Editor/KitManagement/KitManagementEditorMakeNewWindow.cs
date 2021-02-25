using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KSwordKit.Editor.KitManagement
{
    enum ComponentClassificationEnum
    {
        Basic,
        Framework
    }

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
        const string newConfigDependencies = "输入新组件的依赖列表（以分号\";\"分割）";
        static string kitUserInputNewConfigDependencies = newConfigDependencies;
        const string newConfigFileSettings = "输入新组件内的文件设置";
        static string kitUserInputNewConfigFileSettings = newConfigFileSettings;
        const string kitTempFileName = "kswordkit.tempfile";
        const string kitNewConfigTempFileName = "kswordkit.newconfig.tempfile";
        static string kitUserSelectedComponentSrcPath = "";
        const string kitUserSelectedComponentClassification_Basic = "Basic";
        const string kitUserSelectedComponentClassification_Framework = "Framework";
        static string kitUserSelectedComponentClassification = kitUserSelectedComponentClassification_Basic;
        static ComponentClassificationEnum componentClassificationEnum = ComponentClassificationEnum.Basic;


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
            window.minSize = new Vector2(1000, 500);
            newConfig = new KitConfig();
            newConfig.Dependencies = new List<string>();
            newConfig.FileSettings = new List<KitConfigFileSetting>();
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
            drawGUI();
        }
        void drawGUI()
        {
            // 绘制标题
            drawTitle("填写新组件各项配置值");
            // 绘制新组件源码位置
            kitUserSelectedComponentSrcPath = drawUserSelectedComponentSrcPath();
            // 绘制新组件名称
            kitUserInputNewConfigName = drawUserInputNewConfigName();
            // 绘制新组建作者
            kitUserInputNewConfigAuthor = drawUserInputNewConfigAuthor();
            // 绘制作者联系方式
            kitUserInputNewConfigContact = drawUserInputNewConfigContact();
            // 绘制作者主页
            kitUserInputNewConfigHomePage = drawUserInputNewConfigHomePage();
            // 绘制新组件版本号
            kitUserInputNewConfigVersion = drawUserInputNewConfigVersion();
            // 绘制新组件描述信息
            kitUserInputNewConfigDescription = drawUserInputNewConfigDescription();
            // 绘制新组件的依赖列表
            var dependencies = drawUserInputNewConfigDependencies();
            if (!newConfig.Dependencies.Contains(dependencies))
            {
                newConfig.Dependencies.Add(dependencies);
                if (kitUserInputNewConfigDependencies == newConfigDependencies)
                    kitUserInputNewConfigDependencies = dependencies;
                else
                    kitUserInputNewConfigDependencies += dependencies;
            }
            // 绘制新组件分类
            kitUserSelectedComponentClassification = drawUserSelectedComponentClassification();
            // 绘制文件设置
            drawFileSetting();
            // 制作新组件制作完成按钮
            drawDone();
        }
        void drawTitle(string title)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("", EditorStyles.boldLabel, GUILayout.Height(5));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("");
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("", EditorStyles.boldLabel, GUILayout.Height(5));
            EditorGUILayout.EndHorizontal();
        }
        string drawUserSelectedComponentSrcPath()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("新组件源码位置: ", GUILayout.Height(20), GUILayout.Width(90));
            var buttonName = "浏览";
            var selected = !string.IsNullOrEmpty(kitUserSelectedComponentSrcPath);
            if (selected)
                buttonName = "重新选择";
            EditorGUILayout.LabelField(kitUserSelectedComponentSrcPath, GUILayout.Height(20));
            if (GUILayout.Button(buttonName, GUILayout.Width(120), GUILayout.Height(20)))
            {
                kitUserSelectedComponentSrcPath = EditorUtility.OpenFolderPanel("选择新组件源码位置", Application.dataPath, "NewComponentFolder");
            }
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            return kitUserSelectedComponentSrcPath;
        }
        string drawUserInputNewConfigName()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigName = EditorGUILayout.TextField("新组件名称：", kitUserInputNewConfigName, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            return kitUserInputNewConfigName;
        }
        string drawUserInputNewConfigAuthor()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigAuthor = EditorGUILayout.TextField("新组件作者：", kitUserInputNewConfigAuthor, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            return kitUserInputNewConfigAuthor;
        }
        string drawUserInputNewConfigContact()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigContact = EditorGUILayout.TextField("新组件作者的联系方式：", kitUserInputNewConfigContact, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            return kitUserInputNewConfigContact;
        }
        string drawUserInputNewConfigHomePage()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigHomePage = EditorGUILayout.TextField("新组件作者的个人主页：", kitUserInputNewConfigHomePage, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            return kitUserInputNewConfigHomePage;
        }
        string drawUserInputNewConfigVersion()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserInputNewConfigVersion = EditorGUILayout.TextField("新组件版本号：", kitUserInputNewConfigVersion, GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            return kitUserInputNewConfigVersion;
        }
        string drawUserInputNewConfigDescription()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("新组件简介：", GUILayout.Height(20), GUILayout.Width(150));
            kitUserInputNewConfigDescription = EditorGUILayout.TextArea("请输入简介", GUILayout.Height(100));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            return kitUserInputNewConfigDescription;
        }
        string drawUserInputNewConfigDependencies()
        {
            var d = "";
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("新组件的依赖列表：", GUILayout.Height(20), GUILayout.Width(150));
            kitUserInputNewConfigDependencies = EditorGUILayout.TextArea(kitUserInputNewConfigDependencies, GUILayout.Height(60));
            if (GUILayout.Button("<< 添加一个", GUILayout.Width(90), GUILayout.Height(40)))
            {
                var dependency = EditorUtility.OpenFolderPanel("选择依赖的组件文件夹", Application.dataPath, "ComponentFolder");
                if (!string.IsNullOrEmpty(dependency))
                {
                    var componentPath = System.IO.Path.Combine(dependency, windowData.KitConfigFileName);
                    if (System.IO.File.Exists(componentPath))
                    {
                        try
                        {
                            var config = JsonUtility.FromJson<KitConfig>(System.IO.File.ReadAllText(componentPath, System.Text.Encoding.UTF8));
                            d = config.Name + "@" + config.Version;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning(windowData.TitleString + ": 该组件配置文件解析失败！");
                            Debug.LogWarning(e.Message);
                        }
                    }
                    else
                    {
                        Debug.LogWarning(windowData.TitleString + ": 该组件配置文件不存在！");
                    }
                }
            }
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            return d;
        }
        string drawUserSelectedComponentClassification()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("新组件的分类：", GUILayout.Height(20), GUILayout.Width(150));
            componentClassificationEnum = (ComponentClassificationEnum)EditorGUILayout.EnumPopup(componentClassificationEnum);
            kitUserSelectedComponentClassification = componentClassificationEnum == ComponentClassificationEnum.Basic ? kitUserSelectedComponentClassification_Basic : kitUserSelectedComponentClassification_Framework;
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            return kitUserSelectedComponentClassification;
        }
        void drawFileSetting()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("新组件的文件设置：", GUILayout.Height(20), GUILayout.Width(150));
            EditorGUILayout.BeginVertical();
            for (var i = 0; i < newConfig.FileSettings.Count; i++)
            {
                var file = newConfig.FileSettings[i];

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("源文件位置: ", GUILayout.Height(20), GUILayout.Width(120));
                EditorGUILayout.LabelField(file.SourcePath, GUILayout.Height(20));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("安装位置: ", GUILayout.Height(20), GUILayout.Width(120));
                EditorGUILayout.LabelField(file.DestPath, GUILayout.Height(20));
                if (GUILayout.Button("删除第" + i + "项", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    newConfig.FileSettings.RemoveAt(i);
                    Debug.Log("已删除第" + i + "项！");
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("<< 添加一个", GUILayout.Width(90), GUILayout.Height(40)))
            {
                KitManagementEditorMakeNewWindowAddFileSettingWindow.Open("添加新组件文件设置项", (newFileSetting) => {

                    var find = false;
                    for (var j = 0; j < newConfig.FileSettings.Count; j++)
                    {
                        var _file = newConfig.FileSettings[j];
                        if (_file.SourcePath == newFileSetting.SourcePath && _file.DestPath == newFileSetting.DestPath)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                        newConfig.FileSettings.Add(newFileSetting);
                });
            }
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
        }
        void drawDone()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("完成", GUILayout.Width(window.position.size.x - 5), GUILayout.Height(50)))
            {
                newConfig.Name = kitUserInputNewConfigName;
                newConfig.Author = kitUserInputNewConfigAuthor;
                newConfig.Contact = kitUserInputNewConfigContact;
                newConfig.HomePage = kitUserInputNewConfigHomePage;
                newConfig.Version = kitUserInputNewConfigVersion;
                newConfig.Date = new System.DateTime().ToString("yyyy-MM-dd");
                newConfig.Description = kitUserInputNewConfigDescription;
                newConfig.DisplayedName = newConfig.Name + "@" + newConfig.Version;
                newConfig.Classification = kitUserSelectedComponentClassification;

                try
                {
                    var configJson = JsonUtility.ToJson(newConfig, true);
                    var temp_config_filePath = System.IO.Path.Combine(Application.temporaryCachePath, kitNewConfigTempFileName);
                    if (System.IO.File.Exists(temp_config_filePath))
                        System.IO.File.Delete(temp_config_filePath);
                    System.IO.File.WriteAllText(temp_config_filePath, configJson);
                    commit(temp_config_filePath);

                    EditorUtility.DisplayDialog(windowData.TitleString,"新组件制作成功！\n已导出到KSwordKit组件库中", "ok", "关闭");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(windowData.TitleString + ": 该组件配置文件json数据写入失败！");
                    Debug.LogWarning(e.Message);
                }


            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
        // 提交新组件制作操作
        void commit(string json_file_path)
        {
            var exportNewComponentPath = System.IO.Path.Combine(windowData.KitLocalResourceRootDirectory, newConfig.Classification);
            exportNewComponentPath = System.IO.Path.Combine(exportNewComponentPath, newConfig.DisplayedName);
            DirectoryDelete(exportNewComponentPath);
            DirectoryCopy(kitUserSelectedComponentSrcPath, exportNewComponentPath, true);
            var config_file_path = System.IO.Path.Combine(exportNewComponentPath, windowData.KitConfigFileName);
            if (!System.IO.File.Exists(config_file_path))
                System.IO.File.Copy(json_file_path, config_file_path);
        }
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="destDir">目标文件夹</param>
        /// <param name="copySubDirs">是否递归拷贝子目录</param>
        void DirectoryCopy(string sourceDir, string destDir, bool copySubDirs = true)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sourceDir);
            if (!dir.Exists)
            {
                throw new System.IO.DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir);
            }

            System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
            System.IO.Directory.CreateDirectory(destDir);
            System.IO.FileInfo[] files = dir.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, true);
            }

            if (copySubDirs)
            {
                foreach (System.IO.DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDir, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">要删除的目录</param>
        void DirectoryDelete(string dir)
        {
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir);
            var dirMetaFilePath = dir + ".meta";
            if (System.IO.File.Exists(dirMetaFilePath))
                System.IO.File.Delete(dirMetaFilePath);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径</param>
        void FileDelete(string file)
        {
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);
            var fileMetaPath = file + ".meta";
            if (System.IO.File.Exists(fileMetaPath))
                System.IO.File.Delete(fileMetaPath);
        }
    }
}

