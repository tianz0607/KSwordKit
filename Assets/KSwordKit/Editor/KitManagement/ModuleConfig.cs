
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Policy;
using System.Linq;

namespace KSwordKit.Editor.KitManagement
{
#if UNITY_EDITOR
    [Serializable]
    public class ImportFileSetting
    {
        /// <summary>
        /// 用于指示 Path 值是文件还是文件目录
        /// </summary>
        public bool IsDir;
        /// <summary>
        /// 该文件在模块内的相对路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 该文件希望导入的目标位置
        /// </summary>
        public string ImportPath;
    }
    [Serializable]
    public class ModuleConfig
    {
        public ModuleConfig(string configPath)
        {

        }

        /// <summary>
        /// 模块名称
        /// <para>名称在整个框架内有唯一性，否在不能导入。</para>
        /// </summary>
        public string Name;
        /// <summary>
        /// 该模块所依赖的框架内其他模块名称列表
        /// <para>根据 `独立无依赖原则`，请尽量保持该项为空。</para>
        /// </summary>
        public List<string> Dependencies;
        /// <summary>
        /// 该模块内特殊文件设置
        /// <para>默认情况下，模块导入项目中时，所有文件会导入到 `Assets/KSwordKit/Contents/List/{模块名称}` 文件夹内。</para>
        /// <para>如果有些特殊文件需要在其他路径下才能正常工作，可以使用该项单独设置。</para>
        /// </summary>
        public List<ImportFileSetting> FileSettings;

        //public string Export(string sourceDir, string destDir, string importConfigPath)
        //{
        //    string error = "";
        //    try
        //    {
        //        Debug.Log(KSwordKitConst.KSwordKitName + ": 正准备导出模块 " + Name + " ...");

        //        if (!System.IO.Directory.Exists(destDir))
        //        {
        //            System.IO.Directory.CreateDirectory(destDir);
        //        }

        //        string[] fileList = System.IO.Directory.GetFiles(sourceDir, "*");
        //        foreach (string f in fileList)
        //        {
        //            // 剔除配置文件的 .meta 文件
        //            if (new System.IO.FileInfo(f).FullName == new System.IO.FileInfo(importConfigPath).FullName + ".meta")
        //                continue;

        //            string fName = f.Substring(sourceDir.Length + 1);
        //            EditorUtility.DisplayProgressBar(KSwordKitConst.KSwordKitName + ": 拷贝模块 " + Name, "正在拷贝：" + fName, UnityEngine.Random.Range(0f, 1));
        //            // 配置文件导出时，命名为 KSwordKit.Contents.Editor.ContentsEditor.ImportConfigFileName 常量
        //            if (new System.IO.FileInfo(f).FullName == new System.IO.FileInfo(importConfigPath).FullName)
        //                System.IO.File.Copy(System.IO.Path.Combine(sourceDir, fName), System.IO.Path.Combine(destDir, KSwordKit.Contents.Editor.ContentsEditor.ImportConfigFileName), true);
        //            else
        //                System.IO.File.Copy(System.IO.Path.Combine(sourceDir, fName), System.IO.Path.Combine(destDir, fName), true);
        //        }
        //        foreach (var dir in System.IO.Directory.GetDirectories(sourceDir, "*"))
        //        {
        //            // 示例代码默认放置在 Example 中
        //            bool isExampleFolderPath = false;
        //            if(ExampleFolderPaths != null)
        //            {
        //                foreach (var examplePath in ExampleFolderPaths)
        //                {
        //                    if (!System.IO.Directory.Exists(examplePath))
        //                        continue;
        //                    if(new System.IO.DirectoryInfo(examplePath).FullName == new System.IO.DirectoryInfo(dir).FullName)
        //                    {
        //                        isExampleFolderPath = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            var destpath = "Example";
        //            var sourceRoot = new System.IO.FileInfo(importConfigPath).Directory;
        //            if (new System.IO.DirectoryInfo(dir).FullName == new System.IO.DirectoryInfo(System.IO.Path.Combine(sourceRoot.FullName, destpath)).FullName)
        //                isExampleFolderPath = true;
        //            if (isExampleFolderPath)
        //            {
        //                if(ExampleFolderPaths != null && ExampleFolderPaths.Count > 1)
        //                {
        //                    destpath = System.IO.Path.Combine(destpath, new System.IO.DirectoryInfo(dir).Name);
        //                }
        //                error += Export(dir, System.IO.Path.Combine(destDir, destpath), importConfigPath);
        //                continue;
        //            }

        //            error += Export(dir, System.IO.Path.Combine(destDir, dir.Substring(sourceDir.Length + 1)), importConfigPath);
        //        }

        //        Debug.Log(KSwordKitConst.KSwordKitName + ": 已导出模块 " + Name);

        //    }
        //    catch (System.Exception e)
        //    {
        //        error += e.Message;
        //    }

        //    EditorUtility.ClearProgressBar();
        //    return error;
        //}
        //public string Import(string sourceDir, string destDir)
        //{
        //    string error = "";
        //    try
        //    {
        //        Debug.Log(KSwordKitConst.KSwordKitName + ": 正准备导入模块 " + Name + " ...");

        //        if (!System.IO.Directory.Exists(destDir))
        //        {
        //            System.IO.Directory.CreateDirectory(destDir);
        //        }

        //        var filerootdirinfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsSourceDiretory, Name));
        //        string[] fileList = System.IO.Directory.GetFiles(sourceDir, "*");
        //        foreach (string f in fileList)
        //        {
        //            string fName = f.Substring(sourceDir.Length + 1);
        //            EditorUtility.DisplayProgressBar(KSwordKitConst.KSwordKitName + ": 拷贝模块 " + Name, "正在拷贝：" + fName, UnityEngine.Random.Range(0f, 1));

        //            // 配置文件导入时，忽略命名为 KSwordKit.Contents.Editor.ContentsEditor.ImportConfigFileName 常量的文件
        //            if (new System.IO.FileInfo(f).FullName == new System.IO.DirectoryInfo(System.IO.Path.Combine(filerootdirinfo.FullName, ContentsEditor.ImportConfigFileName)).FullName)
        //                continue;
        //            // 处理特殊文件
        //            ImportFileSetting importFileSetting = null;
        //            if(FileSettings != null)
        //            {
        //                importFileSetting = FileSettings.Find((_f) => {
        //                    if (_f.IsDir) return false;
        //                    return new System.IO.FileInfo(f).FullName == new System.IO.FileInfo(System.IO.Path.Combine(filerootdirinfo.FullName, _f.Path)).FullName;
        //                });
        //            }
        //            if(importFileSetting != null)
        //            {
        //                var index = importFileSetting.ImportPath.LastIndexOf('/');
        //                if (index != -1)
        //                {
        //                    var filedirpath = importFileSetting.ImportPath.Substring(0, index);
        //                    if (!System.IO.Directory.Exists(filedirpath))
        //                    {
        //                        System.IO.Directory.CreateDirectory(filedirpath);
        //                    }
        //                }
  
        //                System.IO.File.Copy(System.IO.Path.Combine(sourceDir, fName), importFileSetting.ImportPath, true);
        //                continue;
        //            }

        //            System.IO.File.Copy(System.IO.Path.Combine(sourceDir, fName), System.IO.Path.Combine(destDir, fName), true);
        //        }

        //        var exampleDestpath = KSwordKitConst.KSwordKitExamplesDirtory;
        //        var examapleDirName = "Example";
        //        foreach (var dir in System.IO.Directory.GetDirectories(sourceDir, "*"))
        //        {
        //            // 示例代码默认导入到 KSwordKitConst.KSwordKitExamplesDirtory 中
        //            if (new System.IO.DirectoryInfo(dir).FullName == System.IO.Path.Combine(filerootdirinfo.FullName, examapleDirName))
        //            {
        //                error += Import(dir, exampleDestpath);
        //                continue;
        //            }
        //            if(ExampleFolderPaths != null)
        //            {
        //                var examplePath = ExampleFolderPaths.Find((ex) => {
        //                    return new System.IO.DirectoryInfo(dir).FullName == new System.IO.DirectoryInfo(System.IO.Path.Combine(filerootdirinfo.FullName, ex)).FullName;
        //                });
        //                if (!string.IsNullOrEmpty(examplePath))
        //                {
        //                    if(ExampleFolderPaths.Count == 1)
        //                    {
        //                        error += Import(dir, exampleDestpath +"/"+examapleDirName+ Name);
        //                    }
        //                    else
        //                    {
        //                        error += Import(dir, exampleDestpath + "/" + examapleDirName + Name+"/"+ new System.IO.DirectoryInfo(dir).Name);
        //                    }
        //                    continue;
        //                }
        //            }

        //            // 处理特殊文件
        //            ImportFileSetting importFileSetting = null;
        //            if(FileSettings != null)
        //            {
        //                importFileSetting = FileSettings.Find((f) => {
        //                    if (!f.IsDir) return false;
        //                    return new System.IO.DirectoryInfo(dir).FullName == new System.IO.DirectoryInfo(System.IO.Path.Combine(filerootdirinfo.FullName, f.Path)).FullName;
        //                });
        //            }

        //            if (importFileSetting != null)
        //            {
        //                error += Import(dir, importFileSetting.ImportPath);
        //                continue;
        //            }

        //            error += Import(dir, System.IO.Path.Combine(destDir, dir.Substring(sourceDir.Length + 1)));
        //        }

        //        Debug.Log(KSwordKitConst.KSwordKitName + ": 已导入模块 " + Name);
        //    }
        //    catch (System.Exception e)
        //    {
        //        error += e.Message;
        //    }

        //    EditorUtility.ClearProgressBar();
        //    return error;
        //}
        //public string Delete()
        //{
        //    string error = "";
        //    try
        //    {
        //        Debug.Log(KSwordKitConst.KSwordKitName + ": 正准备删除模块 " + Name + " ...");

        //        var cPath = System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsDirectory, Name);
        //        var spath = System.IO.Path.Combine(KSwordKitConst.KSwordKitContentsSourceDiretory, Name);
        //        var epath = System.IO.Path.Combine(KSwordKitConst.KSwordKitExamplesDirtory, "Example" + Name);

        //        if (System.IO.Directory.Exists(cPath))
        //            FileUtil.DeleteFileOrDirectory(cPath);
        //        if (System.IO.Directory.Exists(epath))
        //            FileUtil.DeleteFileOrDirectory(epath);
        //        if (FileSettings != null)
        //            foreach (var item in FileSettings)
        //            {
        //                if (item.IsDir)
        //                {
        //                    if (System.IO.Directory.Exists(item.ImportPath))
        //                        FileUtil.DeleteFileOrDirectory(item.ImportPath);
        //                }
        //                else
        //                {
        //                    if (System.IO.File.Exists(item.ImportPath))
        //                        FileUtil.DeleteFileOrDirectory(item.ImportPath);
        //                }
        //            }

        //        AssetDatabase.Refresh();
        //        Debug.Log(KSwordKitConst.KSwordKitName + ": 已删除模块 " + Name);
        //    }
        //    catch(System.Exception e)
        //    {
        //        error = e.Message;
        //    }
        //    return error;
        //}
    }
#endif
}
