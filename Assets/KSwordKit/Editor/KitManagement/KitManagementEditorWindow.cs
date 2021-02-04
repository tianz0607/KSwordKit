
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace KSwordKit.Editor.KitManagement
{
    public class KitManagementEditorWindowData
    {
        public string TitleString;                                      // 标题
        public string SubTitleString;                                   // 子标题
        public string KitLocalResourceRootDirectory;                    // 本地资源所在的根目录
        public List<string> KitLocalResourceAllComponentsRootPathList;  // 本地资源内的所有组件路径
        public string KitComponentInstallRootDirectory;                 // 套件组件安装的根目录
        public string KitConfigFileName;                                // 套件组件统一的组件配置文件名称
        public List<KitConfig> KitConfigList;                           // 套件所有组件的配置列表
        public List<KitConfig> kitShouldShowConfigList;                 // 套件所有应该被显示到窗口上的组件列表
        public string KitError;                                         // 套件内错误信息
        public uint KitMaxShowScrollItemCount;                          // 套件编辑器窗口显示滚动列表最大子项数量
    }

    public class KitManagementEditorWindow : EditorWindow
    {
        Vector2 scorllPos;
        static KitManagementEditorWindow window;
        static KitManagementEditorWindowData windowData;
        const string kitUserSearchDefaultInputString = "搜索组件名或组件作者";
        string kitUserSearchInputString = kitUserSearchDefaultInputString;

        public static void Open(KitManagementEditorWindowData data)
        {
            windowData = data;
            window = GetWindow<KitManagementEditorWindow>(false, windowData.TitleString);
            window.Show();

            UpdateData();
        }

        /// <summary>
        /// 窗口更新数据
        /// </summary>
        static void UpdateData()
        {
            windowData.KitConfigList.Clear();
            var kitLocalResourceRootDir = windowData.KitLocalResourceRootDirectory;
            var configFileName = windowData.KitConfigFileName;
            var kitAllComponentsRootPathList = windowData.KitLocalResourceAllComponentsRootPathList;
            var kitConfigList = windowData.KitConfigList;
            if (!string.IsNullOrEmpty(kitLocalResourceRootDir))
            {
                var errorPath = "";
                eachKitLocalResourceComponentRootDirectory(kitLocalResourceRootDir, kitAllComponentsRootPathList, (rootPath, componentRootPath) => {
                    var configPath = System.IO.Path.Combine(componentRootPath, configFileName);
                    if (System.IO.File.Exists(configPath))
                    {
                        try
                        {
                            var config = JsonUtility.FromJson<KitConfig>(System.IO.File.ReadAllText(configPath, System.Text.Encoding.UTF8));
                            config.LocalResourceDirectory = componentRootPath;
                            config.DisplayedName = config.Name + " " + config.Version + " by " + config.Author;
                            config.Classification = rootPath;
                            kitConfigList.Add(config);
                        }
                        catch (System.Exception e)
                        {
                            errorPath += configPath + "\n";
                            Debug.LogError(e.Message);
                        }

                    }
                });
                if (!string.IsNullOrEmpty(errorPath))
                {
                    Debug.LogError("解析配置文件时失败：\n" + errorPath);
                }
            } 
            else
            {
                windowData.KitError = "本地资源路径值为空";
            }
        }
        /// <summary>
        /// 遍历得到本地所有组件所在目录
        /// </summary>
        /// <param name="kitLocalResourceRootDir">套件本地资源所在得根目录</param>
        /// <param name="kitAllComponentsRootPathList">套件所有所在的组件的根目录列表，每个目录内都可能包含多个组件资源。</param>
        /// <param name="targetComponentDirectoryCallback">遍历得到的目标组件所在的目录位置回调</param>
        static void eachKitLocalResourceComponentRootDirectory(string kitLocalResourceRootDir, List<string> kitAllComponentsRootPathList, System.Action<string, string> targetComponentDirectoryCallback)
        {
            if (!string.IsNullOrEmpty(kitLocalResourceRootDir) && kitAllComponentsRootPathList != null && targetComponentDirectoryCallback != null)
            {
                foreach (var componentRootPath in kitAllComponentsRootPathList)
                {
                    if (!string.IsNullOrEmpty(componentRootPath))
                    {
                        var rootPath = System.IO.Path.Combine(kitLocalResourceRootDir, componentRootPath);
                        var rootDirInfo = new System.IO.DirectoryInfo(rootPath);
                        foreach (var rootDir in rootDirInfo.GetDirectories())
                        {
                            targetComponentDirectoryCallback(componentRootPath, rootDir.FullName);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 窗口GUI更新时触发
        /// </summary>
        private void OnGUI()
        { 
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            kitUserSearchInputString = EditorGUILayout.TextField("所有组件：", kitUserSearchInputString);
            window.searchItem(kitUserSearchInputString);
            GUILayout.Space(15);
            EditorGUILayout.EndHorizontal();

            // 显示所有组件
            if (windowData.KitConfigList.Count > windowData.KitMaxShowScrollItemCount)
                scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, true, GUILayout.Height(200));
            else
                scorllPos = EditorGUILayout.BeginScrollView(scorllPos, false, false, GUILayout.Height(200));

            foreach (var item in windowData.kitShouldShowConfigList)
            {
                window.addItemView(item);
            }

            EditorGUILayout.EndScrollView();
        }
        /// <summary>
        /// 根据用户输入的搜索字符串，筛选匹配的子项
        /// </summary>
        /// <param name="userSearchInputString">用户输入的搜索字符串</param>
        void searchItem(string userSearchInputString)
        {
            if (string.IsNullOrEmpty(userSearchInputString) || userSearchInputString == kitUserSearchDefaultInputString)
            {
                windowData.kitShouldShowConfigList.Clear();
                windowData.kitShouldShowConfigList.AddRange(windowData.KitConfigList);
            } 
            else
            {
                windowData.kitShouldShowConfigList.Clear();
                //var pattern = "[" + userSearchInputString + "]";
                foreach(var kitConfig in windowData.KitConfigList)
                {
                    //if (Regex.IsMatch(kitConfig.Name, pattern) 
                    //    || Regex.IsMatch(kitConfig.Author, pattern) 
                    //    || Regex.IsMatch(kitConfig.Contact, pattern) 
                    //    || Regex.IsMatch(kitConfig.Date, pattern)
                    //    || Regex.IsMatch(kitConfig.HomePage, pattern)
                    //    || Regex.IsMatch(kitConfig.Version, pattern) )
                    if (kitConfig.Name.ToLower().StartsWith(userSearchInputString.ToLower())
                        || kitConfig.Author.ToLower().StartsWith(userSearchInputString.ToLower())
                        )
                    {
                        windowData.kitShouldShowConfigList.Add(kitConfig);
                    } 
                }
            }
        }
        /// <summary>
        /// 添加子项视图到窗口上
        /// </summary>
        /// <param name="config">子项的具体配置数据</param>
        void addItemView(KitConfig config)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(config.DisplayedName, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();

            var buttonName = "安装";
            var isComponentInstalled = window.IsComponentInstalled(config);
            if (isComponentInstalled)
            {
                buttonName = "重新安装";
            }

            if (GUILayout.Button(buttonName, GUILayout.Width(110)))
            {
                var error = window.installComponent(config);
                EditorUtility.DisplayDialog("安装部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ? "安装成功！" : "安装失败: \n" + error, "确定");
                AssetDatabase.Refresh();
            }

            EditorGUI.BeginDisabledGroup(!isComponentInstalled);
            if (GUILayout.Button("卸载", GUILayout.Width(110)))
            {
                //var error = config.Delete();
                //EditorUtility.DisplayDialog("删除部件 '" + config.Name + "' ", string.IsNullOrEmpty(error) ? "删除成功！" : "删除失败: \n" + error, "确定");
                AssetDatabase.Refresh();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }
        /// <summary>
        /// 组件是否已安装
        /// </summary>
        /// <param name="config">组件配置数据</param>
        /// <returns>返回true，表示该组件已安装；否则，组件未安装。</returns>
        bool IsComponentInstalled(KitConfig config)
        {
            if (string.IsNullOrEmpty(config.LocalInstallationResourceDirectory)) return false;
            if (!System.IO.Directory.Exists(config.LocalInstallationResourceDirectory)) return false;
            return true;
        }
        /// <summary>
        /// 安装组件到Unity项目中
        /// </summary>
        /// <param name="kitConfig">组件具体的配置数据</param>
        /// <returns>返回null或字符串，null表示安装成功，字符串则表示安装失败的描述。</returns>
        string installComponent(KitConfig config)
        {
            var installRootDir = System.IO.Path.Combine(windowData.KitComponentInstallRootDirectory, config.Classification);
            var componentInstallDir = System.IO.Path.Combine(installRootDir, config.Name);

            return null;
        }


    }
}