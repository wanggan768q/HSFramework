using UnityEngine;
using System.Collections;
using UnityEditor;


namespace HS.Edit.Base
{
    public class HS_GUITool
    {

        #region Widget
        public static bool BeginFoldOut(string text, bool foldOut, bool endSpace = true)
        {

            text = "<b><size=11>" + text + "</size></b>";
            if (foldOut)
            {
                text = "\u25BC " + text;
            }
            else
            {
                text = "\u25BA " + text;
            }

            if (!GUILayout.Toggle(true, text, "dragtab"))
            {
                foldOut = !foldOut;
            }

            if (!foldOut && endSpace) GUILayout.Space(5f);

            return foldOut;
        }

        public static void BeginGroup(int padding = 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(padding);
            EditorGUILayout.BeginHorizontal("As TextArea", GUILayout.MinHeight(10f));
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        public static void EndGroup(bool endSpace = true)
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();

            if (endSpace)
            {
                GUILayout.Space(10f);
            }
        }


        public static bool Toggle(string text, bool value, bool leftToggle = false, int width = -1, bool bold = false)
        {


            if (value) GUI.backgroundColor = Color.green; else GUI.backgroundColor = Color.red;

            GUIStyle boldStyle = new GUIStyle("toggle");
            boldStyle.fontStyle = FontStyle.Bold;

            if (leftToggle)
            {
                if (width == -1)
                    value = EditorGUILayout.ToggleLeft(text, value);
                else
                    value = GUILayout.Toggle(value, text, boldStyle, GUILayout.Width(width));
            }
            else
            {
                if (width == -1)
                    value = EditorGUILayout.Toggle(text, value);
                else
                    value = GUILayout.Toggle(value, text, boldStyle, GUILayout.Width(width));
            }

            GUI.backgroundColor = Color.white;


            return value;

        }

        public static bool BeginToogleGroup(string text, bool value, int indent = 0)
        {
            if (value) GUI.backgroundColor = Color.green; else GUI.backgroundColor = Color.red;
            value = EditorGUILayout.BeginToggleGroup(text, value);
            GUI.backgroundColor = Color.white;

            if (value)
                HS_GUITool.BeginGroup(indent);

            return value;
        }

        public static void EndToggleGroup(bool value, bool space = false)
        {
            if (value)
                HS_GUITool.EndGroup(space);

            EditorGUILayout.EndToggleGroup();
        }


        static public bool Button(string label, Color color, int width, bool leftAligment = false, int height = 0)
        {

            GUI.backgroundColor = color;
            GUIStyle buttonStyle = new GUIStyle("Button");

            if (leftAligment)
                buttonStyle.alignment = TextAnchor.MiddleLeft;

            if (height == 0)
            {
                if (GUILayout.Button(label, buttonStyle, GUILayout.Width(width)))
                {
                    GUI.backgroundColor = Color.white;
                    return true;
                }
            }
            else
            {
                if (GUILayout.Button(label, buttonStyle, GUILayout.Width(width), GUILayout.Height(height)))
                {
                    GUI.backgroundColor = Color.white;
                    return true;
                }
            }
            GUI.backgroundColor = Color.white;

            return false;
        }
        #endregion
    }
}

