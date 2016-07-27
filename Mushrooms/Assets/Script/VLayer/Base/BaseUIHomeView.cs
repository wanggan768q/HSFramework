using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseUIHomeView : HS_ViewBase
{
	protected UnityEngine.RectTransform _diamondInfo;
	protected UnityEngine.UI.Text _diamond;
	protected UnityEngine.UI.Button _home;
	protected UnityEngine.UI.Button _watering;
	protected UnityEngine.RectTransform _weather;
	protected HS.UI.HS_UIListView _humidityList;
	protected UnityEngine.UI.Button _strengthen;
	protected UnityEngine.UI.Button _illustrations;
	protected HS.UI.HS_UIListView _mushroomsDescriptionList;
	protected HS.UI.HS_UIListView _mushroomsSpeciesList;
	protected UnityEngine.RectTransform _goldInfo;
	protected UnityEngine.UI.Text _gold;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("UIHome");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
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
		
		this._strengthen = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Strengthen");
		this.RegisterButtonClickEvent (this._strengthen);
		
		this._illustrations = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Illustrations");
		this.RegisterButtonClickEvent (this._illustrations);
		
		this._mushroomsDescriptionList = HS_Base.FindProperty<HS.UI.HS_UIListView>(transform, "MushroomsDescriptionList");
		this._mushroomsDescriptionList.onInit += OnListViewInit;
		this._mushroomsDescriptionList.onCellCreated += OnCellCreated;
		this._mushroomsDescriptionList.onClick += OnListViewClick;
		this._mushroomsDescriptionList.onSelected += OnListViewSelected;
		this._mushroomsDescriptionList.onDeselected += OnListViewDeselected;
		
		this._mushroomsSpeciesList = HS_Base.FindProperty<HS.UI.HS_UIListView>(transform, "MushroomsSpeciesList");
		this._mushroomsSpeciesList.onInit += OnListViewInit;
		this._mushroomsSpeciesList.onCellCreated += OnCellCreated;
		this._mushroomsSpeciesList.onClick += OnListViewClick;
		this._mushroomsSpeciesList.onSelected += OnListViewSelected;
		this._mushroomsSpeciesList.onDeselected += OnListViewDeselected;
		
		this._goldInfo = HS_Base.FindProperty<UnityEngine.RectTransform>(transform, "GoldInfo");
		
		this._gold = HS_Base.FindProperty<UnityEngine.UI.Text>(transform, "GoldInfo/Gold");
		this._gold.text = "New Text";
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
	
	#region MushroomsDescriptionList
	protected static class TVMushroomsDescriptionList
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
	
	#region MushroomsSpeciesList
	protected static class TVMushroomsSpeciesList
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
