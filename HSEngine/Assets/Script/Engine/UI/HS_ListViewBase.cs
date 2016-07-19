using UnityEngine;
using System.Collections;
using HS.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using HS.Manager;
using UnityEngine.EventSystems;

namespace HS.UI
{
    public class HS_ListViewBase : MonoBehaviour
    {

        protected class Value
        {
            public object data;
            public HS_UIListViewCell cell;
        }

        [SerializeField]
        protected GameObject mCellPrefab;

        [SerializeField]
        protected GameObject mGridContent;

        [SerializeField]
        protected int mCellCount = 1;

        [SerializeField]
        protected string mCellPrefabName = "";

        [SerializeField]
        protected bool mCreateWithUpdate = true;

        protected List<Value> mValues;

        public int cellCount { get { return mValues.Count; } }

        protected ScrollRect mScrollRect;

        protected bool mDirty = false;

        protected int mKeepIndex;

        public delegate void OnInit(HS_ListViewBase listView, HS_UIListViewCell cell, object data);

        public delegate void OnSelectionChanged(HS_ListViewBase listView);

        public delegate void OnCellCreated(HS_ListViewBase listView);

        public OnInit onInit;
        public OnSelectionChanged onSelectionChanged;
        public OnCellCreated onCellCreated;

        void Start()
        {
            mScrollRect = this.gameObject.GetComponent<ScrollRect>();

            if (Application.isPlaying)
            {
                mCellPrefab = HS_ResourceManager.LoadAsset<GameObject>(mCellPrefabName);
            }
        }

        public void Clear()
        {
            if (mValues != null)
            {
                while (mValues.Count > 0)
                {
                    RemoveData(0);
                }
            }
            mKeepIndex = 0;
        }

        void Awake()
        {
            mValues = new List<Value>();
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        internal virtual void OnCellScroll(PointerEventData eventData, HS_UIListViewCell cell)
        {
            //this.mScrollRect.OnScroll(eventData);
        }

        internal virtual void OnCellClick(HS_UIListViewCell cell, GameObject target)
        {

        }

        void LateUpdate()
        {
            if (!mDirty)
            {
                return;
            }
            if (this.mKeepIndex >= this.mValues.Count)
            {
                if (!(this.mKeepIndex == 0 && this.mValues.Count == 0))
                {
                    if (this.onCellCreated != null)
                    {
                        this.onCellCreated(this);
                    }
                }
                this.mDirty = false;
                return;
            }
            this.CreateCell();
            this.mKeepIndex++;
        }

        public void AddData(object data)
        {
            mValues.Add(new Value()
            {
                data = data
            });
            mDirty = true;
            //Ex.Logger.Log("AddData", mCreateWithUpdate, Scheduler.GetTimer());
            if (!mCreateWithUpdate)
            {
                if (mCellPrefab != null)
                {
                    LateUpdate();
                    mDirty = true;
                }
            }
        }

        public void RemoveData(int index)
        {
            if (index >= 0 && index < mValues.Count)
            {
                if (index < this.mKeepIndex)
                {
                    this.mKeepIndex--;
                }
                if (mValues[index].cell != null)
                {
                    GameObject.Destroy(mValues[index].cell.gameObject);
                }
                mValues.RemoveAt(index);
            }
        }

        public void RemoveData(object data)
        {
            RemoveData(GetIndex(data));
        }

        public object GetData(int index)
        {
            if (index >= 0 && index < mValues.Count)
            {
                return mValues[index].data;
            }
            return null;
        }

        public object GetData(HS_UIListViewCell cell)
        {
            for (int i = 0, count = mValues.Count - 1; i <= count; i++)
            {
                Value value = mValues[i];
                if (value.cell.gameObject == cell.gameObject)
                    return value.data;
            }
            return null;
        }

        public HS_UIListViewCell GetCell(int index)
        {
            if (index >= 0 && index < mValues.Count)
            {
                return mValues[index].cell;
            }
            return null;
        }

        public HS_UIListViewCell GetCell(object data)
        {
            int index = GetIndex(data);
            if (index < 0)
                return null;
            return GetCell(index);
        }

        public int GetIndex(object data)
        {
            for (int i = 0, count = mValues.Count - 1; i <= count; i++)
            {
                Value value = mValues[i];
                if (value.data == data)
                    return i;
            }
            return -1;
        }

        public int GetIndex(HS_UIListViewCell cell)
        {
            for (int i = 0, count = mValues.Count - 1; i <= count; i++)
            {
                Value value = mValues[i];
                if (value.cell.gameObject == cell.gameObject)
                    return i;
            }
            return -1;
        }

        private void CreateCell()
        {
            //Ex.Logger.Log("CreateCell", mCreateWithUpdate, Scheduler.GetTimer());
            Value value = this.mValues[this.mKeepIndex];
            GameObject cell = HS_ViewManager.UIAddChild(mGridContent.gameObject, mCellPrefab);
            cell.name = "CellPrefab";
            mValues[mKeepIndex].cell = cell.GetComponent<HS_UIListViewCell>();
            if (this.onInit != null)
            {
                this.onInit(this, value.cell, value.data);
            }
        }

        public void Refresh()
        {
            for (int i = 0; i < this.mValues.Count; i++)
            {
                Value value = this.mValues[i];
                if (this.onInit != null)
                {
                    this.onInit(this, value.cell, value.data);
                }
            }
        }

#if UNITY_EDITOR
        public void RecordCellPrefabName(string name)
        {
            this.mCellPrefabName = name;
        }

        public string GetCellPrefabName()
        {
            return this.mCellPrefabName;
        }

        public GameObject GetCellPrefab()
        {
            return this.mCellPrefab;
        }

        public void SetCellPrefab(GameObject go)
        {
            this.mCellPrefab = go;
        }

        public GameObject GetGridContent()
        {
            return this.mGridContent;
        }

        public void UpdateLayout()
        {
            string gridCellPath = HS_Path.CombinePath("Assets", "SubAssets", "Res", "Prefabs", "UIListCell");
            if (mCellPrefab == null)
            {
                Debug.Log(mCellPrefabName);
                if (mCellPrefabName != "")
                {
                    string cellPrefabPath = HS_Path.CombinePath(gridCellPath, mCellPrefabName + ".prefab");
                    Debug.Log(cellPrefabPath);
                    GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(cellPrefabPath, typeof(GameObject)) as GameObject;
                    mCellPrefab = HS_ViewManager.UIAddChild(mGridContent.gameObject, prefab);
                    mCellPrefab.name = "CellPrefab";
                }
            }
            if (mGridContent == null)
            {
                Debug.LogError("Not specified GridContent");
                return;
            }
            if (mCellPrefab == null)
            {
                Debug.LogError("Not specified CellPrefab");
                return;
            }
            for (int i = mGridContent.transform.childCount - 1; i >= 0; i--)
            {
                Transform t = mGridContent.transform.GetChild(i);
                if (t.gameObject != mCellPrefab)
                    DestroyImmediate(t.gameObject);
            }
            int count = mCellCount - mGridContent.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject cell = HS_ViewManager.UIAddChild(mGridContent.gameObject, mCellPrefab);
                cell.name = "Cell(clone)";
            }
        }
#endif
    }
}

