using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseUIHomeView : HS_ViewBase
{
	protected UnityEngine.UI.Button _home;
	protected UnityEngine.UI.Button _watering;
	protected UnityEngine.UI.Button _strengthen;
	protected UnityEngine.UI.Button _illustrations;
	protected UnityEngine.UI.Image _weather;
	protected UnityEngine.RectTransform _goldInfo;
	protected UnityEngine.UI.Text _gold;
	protected UnityEngine.RectTransform _diamondInfo;
	protected UnityEngine.UI.Text _diamond;
	protected UnityEngine.UI.Image _humidity;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("UIHome");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
		this._home = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Home");
		this.RegisterButtonClickEvent (this._home);
		
		this._watering = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Watering");
		this.RegisterButtonClickEvent (this._watering);
		
		this._strengthen = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Strengthen");
		this.RegisterButtonClickEvent (this._strengthen);
		
		this._illustrations = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Illustrations");
		this.RegisterButtonClickEvent (this._illustrations);
		
		this._weather = HS_Base.FindProperty<UnityEngine.UI.Image>(transform, "Weather");
		
		this._goldInfo = HS_Base.FindProperty<UnityEngine.RectTransform>(transform, "GoldInfo");
		
		this._gold = HS_Base.FindProperty<UnityEngine.UI.Text>(transform, "GoldInfo/Gold");
		this._gold.text = "1234567890";
		
		this._diamondInfo = HS_Base.FindProperty<UnityEngine.RectTransform>(transform, "DiamondInfo");
		
		this._diamond = HS_Base.FindProperty<UnityEngine.UI.Text>(transform, "DiamondInfo/Diamond");
		this._diamond.text = "1234567890";
		
		this._humidity = HS_Base.FindProperty<UnityEngine.UI.Image>(transform, "Humidity");
	}
}
