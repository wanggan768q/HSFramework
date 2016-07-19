using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseGameView : HS_ViewBase
{
	protected UnityEngine.UI.Image _bg;
	protected UnityEngine.UI.Text _text;
	protected UnityEngine.UI.Button _button;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("Game");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
		this._bg = HS_Base.FindProperty<UnityEngine.UI.Image>(transform, "Bg");
		
		this._text = HS_Base.FindProperty<UnityEngine.UI.Text>(transform, "Text");
		this._text.text = "New Text";
		
		this._button = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Button");
		this.RegisterButtonClickEvent (this._button);
	}
}
