using UnityEngine;
using System.Collections;
using HS.Base;
using System.Collections.Generic;
using UnityEngine.UI;
using HS.Manager;

namespace HS.UI
{

    public abstract class HS_ViewBase : MonoBehaviour
    {

        public static Color COLOR_GREEN = new Color(0.49f, 0.89f, 0.36f, 1f);
        public static Color COLOR_RED = new Color(1f, 0.35f, 0.35f, 1f);

        private HS_Scheduler.Proxy mScheduler;

        /// <summary>
        /// Scheduler manager.
        /// Close the view will automatically clean up scheduling tasks
        /// </summary>
        public HS_Scheduler.Proxy scheduler
        {
            get
            {
                if (mScheduler == null)
                {
                    mScheduler = new HS_Scheduler.Proxy();
                }
                return mScheduler;
            }
        }

        /****************************************************\  
        | Unity MonoBehaviour Methods                        |
        \****************************************************/
        void Awake()
        {
            OnCreated();
        }

        void Start()
        {
            OnStarted();
        }

        void OnEnable()
        {
        }

        void OnDisable()
        {

        }

        void OnDestroy()
        {
        }

        /******************************************************\
        | View Life Circle Methods                             |
        \******************************************************/
        virtual protected void OnCreated()
        {
        }

        virtual protected void OnStarted()
        {
        }

        virtual protected void OnOpened()
        {
        }

        virtual protected void OnClosed()
        {
        }

        /*********************************************************\
        | UI Event Handlers                                       |
        \*********************************************************/
        virtual protected void OnButtonClick(GameObject go)
        {

        }

        virtual protected void OnValueChange(GameObject go, float floatValue, int intValue, bool boolValue, string stringValue)
        {

        }

        // UIListView
        virtual protected void OnListViewInit(HS_ListViewBase listView, HS_UIListViewCell cell, object data)
        {
        }

        virtual protected void OnListViewClick(HS_UIListView listView, HS_UIListViewCell cell, GameObject target)
        {
        }

        virtual protected void OnListViewSelected(HS_UIListView listView, int dataIndex)
        {
        }

        virtual protected void OnListViewDeselected(HS_UIListView listView, int dataIndex)
        {
        }

        virtual protected void OnCellCreated(HS_ListViewBase listView)
        {
        }

        /*********************************************************\
        | These variables and methods for Auto-Created Script     |
        \*********************************************************/
        private Dictionary<Canvas, int> mUICanvans = new Dictionary<Canvas, int>();

        virtual internal GameObject GetViewPrefab()
        {
            return null;
        }

        /*******************************************************************\
        | Internal Methods (Don't call these methods)                       |
        \*******************************************************************/

        protected void RegisterButtonClickEvent(Button btn)
        {
            btn.onClick.AddListener(delegate {
                HS_SoundManager.GetInstance().PlaySound("MenuSelect");
                OnButtonClick(btn.gameObject);
            });
        }

        protected void RegisterSliderEvent(Slider slider)
        {
            slider.onValueChanged.AddListener(delegate (float value) {
                this.OnValueChange(slider.gameObject, value, 0, false, "");
            });
        }

        protected void RegisterToggleEvent(Toggle toggle)
        {
            toggle.onValueChanged.AddListener(delegate (bool value) {
                this.OnValueChange(toggle.gameObject, 0, 0, value, "");
            });

        }

        protected void RegisterDropDownEvent(Dropdown dropdown)
        {
            dropdown.onValueChanged.AddListener(delegate (int value) {
                this.OnValueChange(dropdown.gameObject, 0, value, false, "");
            });

        }

        protected void RegisterInputFieldEvent(InputField inputField)
        {
            inputField.onValueChanged.AddListener(delegate (string value) {
                this.OnValueChange(inputField.gameObject, 0, 0, false, value);
            });

        }

        static internal void InternalOpened(HS_ViewBase view)
        {
            view.OnOpened();
        }

        static internal void InternalClosed(HS_ViewBase view)
        {
            view.OnClosed();
            if (view.mScheduler != null)
            {
                view.mScheduler.Destroy();
                view.mScheduler = null;
            }
        }
    }
}


