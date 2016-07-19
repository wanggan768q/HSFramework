using UnityEngine;
using System.Collections;
using UnityEditor;

namespace HS.Edit.Base
{
    public class HS_Object<T> : HS_EditorControlBase
    {
        public T obj { private set; get; }

        Object _Object = null;
        protected HS_Object(string name) : base(name)
        {

        }

        public override void OnGUIUpdate()
        {
            _Object = EditorGUILayout.ObjectField(_Object, typeof(Object), false);
        }
    }

}
