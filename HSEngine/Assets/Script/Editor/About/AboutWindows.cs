using UnityEngine;
using System.Collections;
using UnityEditor;



namespace HS.Edit
{
    public class AboutWindows : EditorWindow
    {
        [MenuItem("HSTool/About",false,1000)]
        static void Init()
        {
            EditorWindow.GetWindow<AboutWindows>(true, "关于");
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label("版本: v0.2");
            GUILayout.Label("问题反馈: ambitiongxb@foxmail.com");
            GUILayout.Label("最后修改时间: 2016年7月14日16:01:59");
            GUILayout.Space(10);
        }
    }
}
