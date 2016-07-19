using UnityEngine;
using System.Collections;
using UnityEditor;

namespace HS.Edit.Base
{
    /// <summary>
    /// 文本框
    /// </summary>
    public class HS_Text : HS_EditorControlBase
    {
        public string Text { private set; get; }

        public HS_Text(string name, string text) : base(name)
        {
            this.Text = text;
        }

        public static HS_Text Create(string name, string text)
        {
            HS_Text t = new HS_Text(name, text);
            return t;
        }

        public override void OnGUIUpdate()
        {
            Text = EditorGUILayout.TextField(this.Name, Text, guiLayout.ToArray());
        }
    }
}