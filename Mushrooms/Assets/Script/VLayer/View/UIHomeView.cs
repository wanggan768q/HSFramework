using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HS.Base;
using HS.UI;
using HS.Manager;


public class UIHomeView : BaseUIHomeView
{
    //最大湿度
    private const int C_MAX_HUMIDITY = 7;

    private int _CurrentHumidity;

    protected override void OnOpened()
    {
        base.OnOpened();
    }
    protected override void OnClosed()
    {
        base.OnClosed();
    }

    /// <summary>
    /// 设置适度
    /// </summary>
    /// <param name="rate">Rate.</param>
    private void SetHumidity(int rate)
    {
        _CurrentHumidity = rate;
        float r = Mathf.Clamp((float)rate, 0f, C_MAX_HUMIDITY);
        this._humidity.fillAmount = r / C_MAX_HUMIDITY;
    }

    /// <summary>
    /// 当前湿度
    /// </summary>
    public float CurrentHumidity
    {
        get
        {
            return Mathf.Clamp01(this._humidity.fillAmount);
        }
    }



    protected override void OnButtonClick(GameObject go)
    {
        Compare(go, this._home, () =>
        {
            D.Log("主页");
            return;
        });

        Compare(go, this._illustrations, () =>
        {
            D.Log("图鉴");
            return;
        });

        Compare(go, this._strengthen, () =>
        {
            D.Log("增强");
            return;
        });

        Compare(go, this._watering, () =>
        {
            D.Log("浇水");
            return;
        });
    }

    void Update()
    {
        //TimeManager.GetInstance().StartTIme
    }

}
