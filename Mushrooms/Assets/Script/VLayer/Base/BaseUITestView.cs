using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseUITestView : HS_ViewBase
{
	protected HS.UI.HS_UIListView _uIScrollRect;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("UITest");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
		this._uIScrollRect = HS_Base.FindProperty<HS.UI.HS_UIListView>(transform, "UIScrollRect");
		this._uIScrollRect.onInit += OnListViewInit;
		this._uIScrollRect.onCellCreated += OnCellCreated;
		this._uIScrollRect.onClick += OnListViewClick;
		this._uIScrollRect.onSelected += OnListViewSelected;
		this._uIScrollRect.onDeselected += OnListViewDeselected;
	}
	
	#region UIScrollRect
	protected static class TVUIScrollRect
	{
		public class Cell
		{
			public UnityEngine.UI.Image _image;
			public UnityEngine.UI.Text _index;
			public UnityEngine.UI.Text _data;
		}
		
		static public Cell Get(HS_UIListViewCell cell)
		{
			Transform t = cell.transform;
			Cell obj = new Cell();
			obj._image = HS_Base.FindProperty<UnityEngine.UI.Image>(t, "Image");
			obj._index = HS_Base.FindProperty<UnityEngine.UI.Text>(t, "Index");
			obj._data = HS_Base.FindProperty<UnityEngine.UI.Text>(t, "Data");
			return obj;
		}
	}
	#endregion
}
