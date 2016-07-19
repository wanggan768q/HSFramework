using UnityEngine;
using System.Collections;

namespace HS.Edit.Base
{
    /// <summary>
    /// Box
    /// </summary>
    public class HS_Box : HS_EditorControlBase
    {
        private string _boxName;
        protected HS_Box(string name,string boxName) : base(name)
        {
            _boxName = boxName;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="name"></param>
       /// <param name="boxName">默认为 ""</param>
       /// <returns></returns>
        public static HS_Box Create(string name,string boxName="")
        {
            HS_Box box = new HS_Box(name, boxName);
            return box;
        }

        public override void OnGUIUpdate()
        {
            GUILayout.Box(_boxName, guiLayout.ToArray());
        }
    }
}

