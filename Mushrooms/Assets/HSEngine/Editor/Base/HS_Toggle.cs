using UnityEngine;
using System.Collections;
using UnityEditor;

namespace HS.Edit.Base
{
    /// <summary>
    /// 多选框
    /// </summary>
    public class HS_Toggle : HS_EditorControlBase
    {
        public delegate void OnClickDelegate(bool isbool);
        public OnClickDelegate OnClick;

        private HS_Label _Label;

        protected bool _Now = false;
        protected bool _Last = false;

        protected HS_Toggle(string name,bool isSelected) : base(name)
        {
            _Now = isSelected;
            _Last = isSelected;
            _Label = HS_Label.Create(name);
        }

        public static HS_Toggle Create(string name, bool isSelected)
        {
            HS_Toggle t = new HS_Toggle(name, isSelected);
            return t;
        }

        public override void OnGUIUpdate()
        {
            GUILayout.BeginHorizontal();
            _Now = EditorGUILayout.Toggle(_Now, GUILayout.Width(30), GUILayout.Height(20));
            _Label.OnGUIUpdate();
            GUILayout.EndHorizontal();

            if (_Now != _Last)
            {
                if (OnClick != null)
                {
                    OnClick(_Now);
                }
                _Last = _Now;
            }
        }
    }
}