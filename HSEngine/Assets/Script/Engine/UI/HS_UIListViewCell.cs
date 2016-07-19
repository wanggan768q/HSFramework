using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HS.UI
{
    public class HS_UIListViewCell : MonoBehaviour, IPointerClickHandler
    {

        internal enum State
        {
            Normal,
            Selected,
            Disable
        }

        [HideInInspector]
        public int dataIndex = 0;

        [SerializeField]
        private GameObject mNormalState = null;

        [SerializeField]
        private GameObject mSelectedState = null;

        [SerializeField]
        private GameObject mDisabledState = null;

        private bool mDirty = false;
        private State mState = State.Normal;
        private GameObject[] mStateObjects = null;
        private HS_ListViewBase mGridView = null;

        internal State state
        {
            get
            {
                return mState;
            }
            set
            {
                if (mState == value)
                    return;
                mState = value;
                mDirty = true;
            }
        }

        /*public void OnScroll (PointerEventData eventData)
        {
            Logger.Log("OnScroll Cell");
            //mGridView.OnCellScroll(eventData, this);
        }*/

        public void OnPointerClick(PointerEventData eventData)
        {

            mGridView.OnCellClick(this, null);
        }

        void Start()
        {
            mStateObjects = new GameObject[] { mNormalState, mSelectedState, mDisabledState };
            mGridView = this.transform.parent.parent.parent.GetComponent<HS_ListViewBase>();
            mDirty = true;

            foreach (Button btn in this.transform.GetComponentsInChildren<Button>(true))
            {
                Button btnTemp = btn;
                btn.onClick.AddListener(delegate () {
                    if (btnTemp.interactable)
                    {
                        mGridView.OnCellClick(this, btnTemp.gameObject);
                    }
                });
            }
        }

        void Update()
        {
            if (mDirty)
            {
                mDirty = false;
                GameObject go = mStateObjects[(int)state];
                for (int i = 0; i < 3; i++)
                {
                    GameObject obj = mStateObjects[i];
                    if (obj != null && obj != go)
                    {
                        obj.SetActive(false);
                    }
                }
                if (go != null)
                {
                    go.SetActive(true);
                }
            }
        }
    }
}
