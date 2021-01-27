

namespace KSwordKit.Const
{
    /// <summary>
    /// KSwordKit中的所有常量
    /// <para>但不包括框架每个模块内的常量，框架内的模块内部使用的常量由模块自己定义和使用。</para>
    /// <para>这里的常量【请勿修改】，以防止程序出错。</para>
    /// </summary>
    public class ConfigurationConst
    {
        /// <summary>
        /// KSwordKit框架名字
        /// </summary>
        public const string KitName = "KSwordKit";
        /// <summary>
        /// KSwordKit框架安装根目录
        /// <para>此目录指示了框架的安装的根目录，也是框架的工作目录</para>
        /// <para>该目录在项目 `Assets` 中，框架内导入的所有部件也会出现在该目录内，参与项目程序的最终编译。</para>
        /// </summary>
        public const string KitInstallationDirectory = "Assets/KSwordKit";
        /// <summary>
        /// KSwordKit框架已被应用到项目中的所有模块所在的根目录
        /// <para>此目录指示了在点击 `导入模块` 时，模块将被安装的位置。</para>
        /// </summary>
        public const string KitContentDirectory = "Assets/KSwordKit/Content";
        /// <summary>
        /// KSwordKit框架包含的所有模型所在的根目录
        /// <para>此目录指示了在用户执行 `导入部件` 或 `导出部件` 时, 将此处存取模块数据。</para>
        /// <para>该目录在项目根目录下，不会出现在 `Assets` 目录中，避免被Unity编辑器编译。</para>
        /// </summary>
        public const string KitLocalResourceDirectory = ".KSwordKitLocalResource";
        /// <summary>
        /// KSwordKit框架检查更新地址
        /// </summary>
        public const string KitCheckForUpdates = "https://raw.githubusercontent.com/keenlovelife/KSwordKit/master/.VersionInformation.json";
        /// <summary>
        /// KSwordKit框架更新时使用的URL前缀
        /// </summary>
        public const string KitUpdateURLPrefix = "https://github.com/keenlovelife/KSwordKit/releases/tag";
    }
}
