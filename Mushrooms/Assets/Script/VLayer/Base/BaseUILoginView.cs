using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class BaseUILoginView : HS_ViewBase
{
	protected UnityEngine.UI.Button _start;
	protected UnityEngine.UI.InputField _inputFieldPName;
	protected UnityEngine.UI.InputField _inputName;
	
	internal override GameObject GetViewPrefab()
	{
		return HS_ResourceManager.LoadAsset<GameObject>("UILogin");
	}
	
	protected override void OnCreated()
	{
		base.OnCreated();
	
		Transform transform = this.transform;
		if (transform == HS_ViewManager.root.transform) return;
		
		this._start = HS_Base.FindProperty<UnityEngine.UI.Button>(transform, "Start");
		this.RegisterButtonClickEvent (this._start);
		
		this._inputFieldPName = HS_Base.FindProperty<UnityEngine.UI.InputField>(transform, "InputFieldPName");
		this.RegisterInputFieldEvent (this._inputFieldPName);
		
		this._inputName = HS_Base.FindProperty<UnityEngine.UI.InputField>(transform, "InputName");
		this.RegisterInputFieldEvent (this._inputName);
	}
}
