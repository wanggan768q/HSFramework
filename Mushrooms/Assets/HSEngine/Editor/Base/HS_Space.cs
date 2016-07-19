using UnityEngine;
using System.Collections;


namespace HS.Edit.Base
{
    /// <summary>
    /// 空白
    /// </summary>
    public class HS_Space : HS_EditorControlBase
    {
        float pixels;

        protected HS_Space(float pix) : base("")
        {
            pixels = pix;
        }

        public static HS_Space Create(float pix)
        {
            HS_Space space = new HS_Space(pix);
            return space;
        }

        public override void OnGUIUpdate()
        {
            GUILayout.Space(pixels);
        }
    }
}