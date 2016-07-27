using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;

public class UILogoView : BaseUILogoView
{
	protected override void OnCreated()
	{
		base.OnCreated ();
	}
	protected override void OnStarted()
	{
        scheduler.Timeout(() =>
        {
            HS_ViewManager.Open<UIHomeView>();
            HS_ViewManager.Close<UILogoView>();
        },5);
	}
	protected override void OnOpened()
	{
		base.OnOpened ();
	}
	protected override void OnClosed()
	{
		base.OnClosed ();
	}
}
