using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;


public class UIHomeView : BaseUIHomeView
{
	private const float C_MAX_HUMIDITY = 7f;
    protected override void OnOpened()
	{
		base.OnOpened ();
    }
	protected override void OnClosed()
	{
		base.OnClosed ();
	}

	/// <summary>
	/// 设置适度
	/// </summary>
	/// <param name="rate">Rate.</param>
	private void SetHumidity(int rate)
	{
		float r = Mathf.Clamp ((float)rate, 0f, 7f);
		this._humidity.fillAmount = r / C_MAX_HUMIDITY;
	}

	protected override void OnButtonClick (GameObject go)
	{
		Compare (go, this._home, () => {
			D.Log ("主页");
		});

		Compare (go, this._illustrations, () => {
			D.Log ("图鉴");
		});

		Compare (go, this._strengthen, () => {
			D.Log ("增强");
		});

		Compare (go, this._watering, () => {
			D.Log ("浇水");
		});
	}
}
