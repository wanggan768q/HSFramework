using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseUILoadingView : HS_ViewBase
{
	protected UnityEngine.UI.Image _image;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("UILoading");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
		this._image = HS_Base.FindProperty<UnityEngine.UI.Image>(transform, "Image");
	}
}
