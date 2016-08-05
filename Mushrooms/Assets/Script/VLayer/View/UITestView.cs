using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class UITestView : BaseUITestView
{
    struct CellData
    {
        public int index;
        public int data;
    }

	protected override void OnOpened()
	{
		base.OnOpened ();

        for(int i=0;i<1000;++i)
        {
            CellData cell;
            cell.index = i;
            cell.data = i % 10;
            this._uIScrollRect.AddData(cell);
        }

	}
	protected override void OnClosed()
	{
		base.OnClosed ();
	}

    override protected void OnValueChange(GameObject go, float floatValue, int intValue, bool boolValue, string stringValue)
    {
        //throw new NotImplementedException();
    }

    override protected void OnListViewInit(HS_ListViewBase listView, HS_UIListViewCell cell, object data)
    {
        CellData cd = (CellData)data;
        TVUIScrollRect.Cell c = TVUIScrollRect.Get(cell);
        c._index.text = cd.index.ToString();
        c._data.text = cd.data.ToString();
        if (cd.index == 0)
        {
            c._image.color = Color.green;
        }
        else if(cd.index == 9)
        {
            c._image.color = Color.yellow;
        }
    }
}
