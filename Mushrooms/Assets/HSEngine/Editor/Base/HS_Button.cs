using UnityEngine;
using System.Collections;
using HS.Edit.Base;
using System;

namespace HS.Edit.Base
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class HS_Button : HS_EditorControlBase
    {
        private System.Action OnClick = null;
        protected HS_Button(string name) : base(name)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="onClick">事件触发</param>
        /// <returns>Button 实例</returns>
        public static HS_Button Create(string name, System.Action onClick)
        {
            HS_Button but = new HS_Button(name);
            but.OnClick = onClick;
            return but;
        }

        public override void OnGUIUpdate()
        {
            if (GUILayout.Button(this.Name, guiLayout.ToArray()))
            {
                if (OnClick != null)
                {
                    OnClick();
                }
            }
        }
    }
}

