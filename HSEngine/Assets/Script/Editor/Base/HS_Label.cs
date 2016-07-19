using UnityEngine;
using System.Collections;
using UnityEditor;

namespace HS.Edit.Base
{
    /// <summary>
    /// 文本框
    /// </summary>
    public class HS_Label : HS_EditorControlBase
    {
        public string Text;

        protected HS_Label(string name) : base(name)
        {
            Text = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HS_Label Create(string name)
        {
            HS_Label label = new HS_Label(name);
            return label;
        }

        public override void OnGUIUpdate()
        {
            EditorGUILayout.LabelField(this.Text, guiLayout.ToArray());
        }
    }
}

