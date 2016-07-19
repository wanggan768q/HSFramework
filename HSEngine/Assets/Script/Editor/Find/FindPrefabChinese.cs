using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using HS.Edit.Base;

namespace HS.Tool.Edit
{
    public class FindPrefabChinese: EditorWindow
    {
        /*
        private Object _Object;
        private HashSet<string> _Paths = new HashSet<string>();

        public void Find()
        {
            Image[] objs = Resources.FindObjectsOfTypeAll<Image>();
            foreach (Image obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj.gameObject);
                if(!string.IsNullOrEmpty(path))
                {
                    _Paths.Add(path);
                }
            }
        }
        
        Vector2 _scrollPos;

        private void OnGUI()
        {
            HS_Label.Create("所有路径：").OnGUIUpdate();
            EditorGUILayout.BeginHorizontal();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(500), GUILayout.Height(500));
            HS_GUITool.BeginGroup();
            {
                foreach (string path in _Paths)
                {
                    HS_Button.Create(path, () =>
                     {
                         Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
                     }).OnGUIUpdate();
                }
            }
            HS_GUITool.EndGroup();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }
        */
    }
}

