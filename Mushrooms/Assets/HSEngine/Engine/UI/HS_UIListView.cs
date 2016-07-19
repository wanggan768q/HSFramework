using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace HS.UI
{

    public class HS_UIListView : HS_ListViewBase
    {

        [SerializeField]
        private bool mCancelable = false;

        [SerializeField]
        private int mMaxSelection = 1;

        private List<int> mSelections;

        public int selectedIndex
        {
            get
            {
                if (mSelections.Count > 0)
                {
                    return mSelections[0];
                }
                return -1;
            }
            set
            {
                /*
             * int oldSelectIndex = -1;
             * if (mSelections.Count > 0) {
                oldSelectIndex = mSelections[0];
                mSelections[0] = value;
            }
            if (oldSelectIndex >= 0) {
                UIListViewCell cellSelected = GetCell(oldSelectIndex);
                cellSelected.state = UIListViewCell.State.Normal;
                onDeselected(this, oldSelectIndex);
            }*/
                if (mSelections.Count > 0 && mSelections[0] >= 0 && mSelections[0] != value && value < 0)
                {
                    HS_UIListViewCell cellSelected = GetCell(mSelections[0]);
                    cellSelected.state = HS_UIListViewCell.State.Normal;
                    mSelections.RemoveAt(0);
                }

                if (value >= 0)
                {
                    HS_UIListViewCell cell = GetCell(value);
                    ScrollTo(value);
                    OnCellClick(cell, cell.gameObject);
                }
            }
        }

        public int[] selectedIndexs
        {
            get
            {
                return mSelections.ToArray();
            }
        }

        public delegate void OnTouchEvent(HS_UIListView listView, HS_UIListViewCell cell, GameObject target);

        public delegate void OnSelectEvent(HS_UIListView listView, int dataIndex);


        public OnTouchEvent onClick;
        public OnSelectEvent onSelected;
        public OnSelectEvent onDeselected;

        private float time = 0.5f;
        private Ease ease = Ease.OutExpo;
        private Tweener tweener;

        internal override void OnCellClick(HS_UIListViewCell cell, GameObject target)
        {
            if (cell.state == HS_UIListViewCell.State.Disable)
            {
                return;
            }
            base.OnCellClick(cell, target);
            if (onClick != null)
            {
                onClick(this, cell, target);
            }
            if (cell != null)
            {
                int index = GetIndex(cell);
                int changeState = 0;
                if (mSelections.IndexOf(index) >= 0)
                {
                    if (mCancelable || mMaxSelection != 1)
                    {
                        changeState = 1;
                        mSelections.Remove(index);
                    }
                }
                else
                {
                    if (mMaxSelection == 1)
                    {
                        changeState = 2;
                        for (int i = mSelections.Count - 1; i >= 0; i--)
                        {
                            int slelectIndex = mSelections[i];
                            HS_UIListViewCell obj = GetCell(slelectIndex);
                            if (obj != null)
                            {
                                obj.state = HS_UIListViewCell.State.Normal;
                            }
                            if (onDeselected != null)
                            {
                                onDeselected(this, slelectIndex);
                            }
                        }
                        mSelections.Clear();
                        mSelections.Add(index);
                    }
                    else
                    {
                        if (mMaxSelection == 0 || mSelections.Count < mMaxSelection)
                        {
                            changeState = 2;
                            mSelections.Add(index);
                        }
                    }
                }
                if (changeState > 0)
                {
                    cell.state = changeState == 2 ? HS_UIListViewCell.State.Selected : HS_UIListViewCell.State.Normal;
                    if (changeState == 2)
                    {
                        cell.state = HS_UIListViewCell.State.Selected;
                        if (onSelected != null)
                        {
                            onSelected(this, index);
                        }
                    }
                    else
                    {
                        cell.state = HS_UIListViewCell.State.Normal;
                        if (onDeselected != null)
                        {
                            onDeselected(this, index);
                        }
                    }
                    if (onSelectionChanged != null)
                    {
                        onSelectionChanged(this);
                    }
                }
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            mSelections = new List<int>();
        }

        public void ScrollTo(int index, int itemRowCount = 1, bool reserve = false)
        {
            int itemCount = this.mGridContent.transform.childCount;
            if (index < 0 || index > itemCount - 1)
            {
                Debug.LogError("Scroll to wrong index:" + " " + index);
                return;
            }
            if (itemRowCount > 1)
            {
                itemCount = Mathf.CeilToInt(itemCount / itemRowCount);
                index = Mathf.FloorToInt(index / itemRowCount);
            }
            float step = 1;
            if (itemCount > 2)
            {

                step = (float)index / (float)itemCount;
                if (reserve)
                {
                    step = 1 - step;
                }
            }
            step = Mathf.Clamp01(step);
            tweener = DOTween.To(() => ScrollValue, (v) => ScrollValue = v, step, time).SetEase(ease);
        }


        public float ScrollValue
        {
            get
            {
                if (this.mScrollRect == null)
                {
                    return 0.0f;
                }
                return this.mScrollRect.horizontal ? this.mScrollRect.horizontalNormalizedPosition : this.mScrollRect.verticalNormalizedPosition;
            }

            set
            {
                if (this.mScrollRect == null)
                {
                    return;
                }
                if (this.mScrollRect.horizontal)
                {
                    this.mScrollRect.horizontalNormalizedPosition = value;
                }
                else
                {
                    this.mScrollRect.verticalNormalizedPosition = value;
                }
            }
        }
    }
}

