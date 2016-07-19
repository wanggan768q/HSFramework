using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace HS.Edit.Base
{
    public abstract class HS_EditorControlBase
    {
        public string Name;
        public float W { get; private set; }
        public float H { get; private set; }

        protected List<GUILayoutOption> guiLayout = new List<GUILayoutOption>();

        public HS_EditorControlBase(string name)
        {
            Name = name;
        }

        public void SetSize(float w,float h)
        {
            W = w;
            H = h;
            guiLayout.Add(GUILayout.Width(W));
            guiLayout.Add(GUILayout.Height(H));
        }
        public override string ToString()
        {
            return Name;
        }

        public abstract void OnGUIUpdate();

    }

    
}

