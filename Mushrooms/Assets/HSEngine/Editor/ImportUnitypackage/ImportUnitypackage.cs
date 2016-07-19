using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


namespace HS.Edit
{
    public class ImportUnitypackage : EditorWindow
    {
        static string _UnitypackageFolderPath;
        private void OnGUI()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("选择路径"))
            {
                _UnitypackageFolderPath = EditorUtility.OpenFolderPanel("选择路径", "", "");
            }
            GUILayout.Label("选择路径:" + _UnitypackageFolderPath, EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (!string.IsNullOrEmpty(_UnitypackageFolderPath))
            {
                string[] files = Directory.GetFiles(_UnitypackageFolderPath, "*.unitypackage");
                
                GUILayout.Space(10);
                if (GUILayout.Button("开始导入"))
                {
                    foreach (string file in files)
                    {
                        string s = file.Replace("\\", "/");
                        AssetDatabase.ImportPackage(s,false);
                    }
                    _UnitypackageFolderPath = null;
                    EditorUtility.DisplayDialog("提示", "批量导入Unitypackage完成", "确认");
                }
            }
        }

    }
}

