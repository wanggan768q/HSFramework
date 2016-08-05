using UnityEngine;
using System.Collections;
using UnityEditor;
using HS.UI;
using System.Reflection;

[CustomEditor(typeof(HS_UIListView), true)]
public class UIListViewInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(3f);
        serializedObject.Update();

        EditorUtil.DrawProperty("GridContent", serializedObject, "mGridContent");
        //EditorUtil.DrawProperty("CellPrefabName", serializedObject, "mCellPrefabName");

        EditorUtil.DrawProperty("CellPrefab", serializedObject, "mCellPrefab");
        EditorUtil.DrawProperty("CellCount", serializedObject, "mCellCount");

        EditorUtil.DrawProperty("Cancelable", serializedObject, "mCancelable");
        EditorUtil.DrawProperty("MaxSelection", serializedObject, "mMaxSelection");
        EditorUtil.DrawProperty("CreateWithUpdate", serializedObject, "mCreateWithUpdate");

        if (GUILayout.Button("Preview"))
        {
            ((HS_UIListView)target).UpdateLayout();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public class EditorUtil
    {

        public static void ShowObj(System.Object sys_obj)
        {
            PropertyInfo[] ps = sys_obj.GetType().GetProperties();
            for (int i = 0; i < ps.Length; i++)
            {
                EditorGUILayout.LabelField(ps[i].Name, ps[i].GetValue(sys_obj, null).ToString());
            }
        }

        /// <summary>
        /// Helper function that draws a serialized property.
        /// </summary>

        static public SerializedProperty DrawProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(null, serializedObject, property, false, options);
        }

        /// <summary>
        /// Helper function that draws a serialized property.
        /// </summary>

        static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(label, serializedObject, property, false, options);
        }

        /// <summary>
        /// Helper function that draws a serialized property.
        /// </summary>

        static public SerializedProperty DrawPaddedProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(null, serializedObject, property, true, options);
        }

        /// <summary>
        /// Helper function that draws a serialized property.
        /// </summary>

        static public SerializedProperty DrawPaddedProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(label, serializedObject, property, true, options);
        }

        /// <summary>
        /// Helper function that draws a serialized property.
        /// </summary>

        static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
        {
            SerializedProperty sp = serializedObject.FindProperty(property);

            if (sp != null)
            {
                if (padding)
                    EditorGUILayout.BeginHorizontal();

                if (label != null)
                    EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
                else
                    EditorGUILayout.PropertyField(sp, options);

                if (padding)
                {
                    GUILayout.Space(18f);
                    EditorGUILayout.EndHorizontal();
                }
            }
            return sp;
        }
    }
}