using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class UILoginView : BaseUILoginView
{

	protected override void OnOpened()
	{
		base.OnOpened ();
	}
	protected override void OnClosed()
	{
		base.OnClosed ();
	}
	protected override void OnStarted ()
	{
		base.OnStarted ();
		scheduler.Timeout(() =>
			{
				HS_ViewManager.Open<UIHomeView>();
				HS_ViewManager.Close<UILoginView>();
			},1);
	}
}
