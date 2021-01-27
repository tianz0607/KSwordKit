using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KSwordKit.Editor.Initialize
{
    public class KitInitializeEditor
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            UnityEngine.Debug.Log("KSwordKit 已初始化完毕 ！");
        }
    }
}

