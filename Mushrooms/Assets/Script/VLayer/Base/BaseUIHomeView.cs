using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseUIHomeView : HS_ViewBase
{
	protected UnityEngine.RectTransform _goldInfo;
	protected UnityEngine.UI.Text _gold;
	protected UnityEngine.RectTransform _diamondInfo;
	protected UnityEngine.UI.Text _diamond;
	protected UnityEngine.UI.Button _home;
	protected UnityEngine.UI.Button _watering;
	protected UnityEngine.RectTransform _weather;
	protected HS.UI.HS_UIListView _humidityList;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("UIHome");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
		this._goldInfo = HS_Base.FindProperty<UnityEngine.RectTransform>(transform, "GoldInfo");
		
		this._gold = HS_Base.FindProperty<UnityEngine.UI.Text>(transform, "GoldInfo/Gold");
		this._gold.text = "New Text";
		
		this._diamondInfo = HS_Base.FindProperty<UnityEngine.RectTransform>(transform, "DiamondInfo");
		
		this._diamond = HS_Base.FindProperty<UnityEngine.UI.Text>(transform, "DiamondInfo/Diamond");
		this._diamond.text = "New Text";
		
		this._home = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Home");
		this.RegisterButtonClickEvent (this._home);
		
		this._watering = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Watering");
		this.RegisterButtonClickEvent (this._watering);
		
		this._weather = HS_Base.FindProperty<UnityEngine.RectTransform>(transform, "Weather");
		
		this._humidityList = HS_Base.FindProperty<HS.UI.HS_UIListView>(transform, "HumidityList");
		this._humidityList.onInit += OnListViewInit;
		this._humidityList.onCellCreated += OnCellCreated;
		this._humidityList.onClick += OnListViewClick;
		this._humidityList.onSelected += OnListViewSelected;
		this._humidityList.onDeselected += OnListViewDeselected;
	}
	
	#region HumidityList
	protected static class TVHumidityList
	{
		public class Cell
		{
		}
		
		static public Cell Get(HS_UIListViewCell cell)
		{
			Transform t = cell.transform;
			Cell obj = new Cell();
			return obj;
		}
	}
	#endregion
}
