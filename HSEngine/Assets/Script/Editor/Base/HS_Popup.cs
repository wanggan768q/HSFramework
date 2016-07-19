using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace HS.Edit.Base
{
    /// <summary>
    /// 下拉框
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HS_Popup<T> : HS_EditorControlBase
    {
        private int popup = 0;

        public List<T> Items = new List<T>();

        /// <summary>
        /// 当前选中对象
        /// </summary>
        public T SelectItem { private set; get; }

        /// <summary>
        /// 当前选中的下标
        /// </summary>
        public int SelectItemIndex { private set; get; }

        protected HS_Popup(string name) : base(name)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HS_Popup<T> Create(string name)
        {
            HS_Popup<T> popup = new HS_Popup<T>(name);
            return popup;
        }

        public void Add(T item)
        {
            Items.Add(item);
        }

        public override void OnGUIUpdate()
        {
            List<string> names = new List<string>();
            foreach (T i in Items)
            {
                names.Add(i.ToString());
            }
            popup = EditorGUILayout.Popup(this.Name, popup, names.ToArray(), guiLayout.ToArray());

            if (popup >= 0 && popup < Items.Count)
            {
                SelectItem = Items[popup];
                SelectItemIndex = popup;
            }
        }
    }
}
