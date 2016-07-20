using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseUILogoView : HS_ViewBase
{
	protected UnityEngine.UI.Image _logoImage;
	protected UnityEngine.UI.Text _text;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("UILogo");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
		this._logoImage = HS_Base.FindProperty<UnityEngine.UI.Image>(transform, "LogoImage");
		
		this._text = HS_Base.FindProperty<UnityEngine.UI.Text>(transform, "Text");
		this._text.text = "111111111";
	}
}
