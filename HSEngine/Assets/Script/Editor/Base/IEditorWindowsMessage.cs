using UnityEngine;
using System.Collections;

namespace HS.Edit.Base
{
    public interface IEditorWindowsMessage
    {
        void OnDestroy();
        void OnFocus();
        void OnGUI();
        void OnHierarchyChange();
        void OnInspectorUpdate();
        void OnLostFocus();
        void OnProjectChange();
        void OnSelectionChange();
        void Update();
    }
}


