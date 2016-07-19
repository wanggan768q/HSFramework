using UnityEngine;
using System.Collections;


namespace HS.Edit.Base
{
    /// <summary>
    /// Box
    /// </summary>
    public class HS_Area : HS_EditorControlBase
    {
        private HS_Box _Box;
        private bool _IsAddBox = false;
        protected HS_Area(string name, float w,float h,bool isAddBox) : base(name)
        {
            base.SetSize(w, h);
            _IsAddBox = isAddBox;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="boxName">默认为 ""</param>
        /// <returns></returns>
        public static HS_Area Create(string name,float w,float h,bool isAddBox)
        {
            HS_Area area = new HS_Area(name, w,h,isAddBox);
            return area;
        }

        public void EndArea()
        {
            GUILayout.EndArea();
        }

        public override void OnGUIUpdate()
        {
            float offsetX = 0;
            float offsetY = 0;
            if (_IsAddBox)
            {
                if(_Box == null)
                {
                    _Box = HS_Box.Create("DefaultAreaBox");
                }
                offsetX = 5;
                offsetY = 5;
                _Box.SetSize(W, H);
                _Box.OnGUIUpdate();
            }
            GUILayout.BeginArea(new Rect(offsetX, offsetY, W, H));
        }
    }
}

