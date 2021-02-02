using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Policy;
using System.Linq;

namespace KSwordKit.Editor.KitManagement
{
    [Serializable]
    public class KitConfig
    {
        /// <summary>
        /// 组件ID
        /// <para>ID在整个开发套件内有唯一性，否在不能导入。</para>
        /// <para>ID一般为组件的名称+版本号</para>
        /// </summary>
        public string ID;
        /// <summary>
        /// 组件名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 组件作者
        /// </summary>
        public string Author;
        /// <summary>
        /// 作者联系方式
        /// </summary>
        public string Contact;
        /// <summary>
        /// 作者的各人主页
        /// </summary>
        public string HomePage;
        /// <summary>
        /// 组件版本号
        /// <para>版本号一般为类似v1.0.0的字符串，版本号请遵循语义化版本号规范 2.0.0:https://semver.org/lang/zh-CN/</para>
        /// </summary>
        public string Version;
        /// <summary>
        /// 组件创建日期
        /// <para>Date值一般为类似2020-02-02的字符串</para>
        /// </summary>
        public string Date;
        /// <summary>
        /// 该部件所依赖的框架内其他组件列表
        /// 列表内容是其他组件ID字符串
        /// <para>根据 `独立无依赖原则`，请尽量保持该项为空。</para>
        /// </summary>
        public List<string> Dependencies;
        /// <summary>
        /// 该组件内特殊文件设置
        /// <para>默认情况下，部件导入项目中时，所有文件会导入到 `Assets/KSwordKit/AllComponents/` 文件夹内。</para>
        /// <para>如果有些特殊文件需要在其他路径下才能正常工作，可以使用该项单独设置。</para>
        /// </summary>
        public List<KitConfigFileSetting> FileSettings;
    }
    [Serializable]
    public class KitConfigFileSetting
    {
        /// <summary>
        /// 该文件在组件内的相对于组件根目录的路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 该文件希望导入的目标位置
        /// </summary>
        public string ImportPath;
    }
}