using UnityEngine;
using System.Collections;
using HS.Base;
using System;
using UnityEngine.UI;
using HS.Manager;

public class Mushrooms : IResetable
{
    private int _Id;
    private Image _Image;
    private MushroomsCfgElement _MushroomsCfgElement;
    
    /// <summary>
    /// 最终成熟时间
    /// </summary>
    private int RipeTime
    {
        get
        {
            return TimeManager.GetInstance().StartTime + _MushroomsCfgElement.RipeTime;
        }
    }

    /// <summary>
    /// 剩余时间
    /// </summary>
    public int SurplusTime
    {
        get
        {
            return RipeTime - TimeManager.GetInstance().StartTime;
        }
    }


    public Mushrooms()
    {

    }

    public void Init(MushroomsCfgElement e)
    {
        //         scheduler.Timeout(() =>
        //         {
        //             HS_ViewManager.Open<UILoginView>();
        //             HS_ViewManager.Close<UILogoView>();
        //         }, 2);

        GameObject obj = new GameObject();
        _Image = obj.AddComponent<Image>();
        _Image.sprite = HS_ResourceManager.LoadAsset<Sprite>(e.AnimationName);
        obj.AddComponent<MushroomsRipeStateBehaviour>().Init(this);
        obj.AddComponent<MushroomsWitherStateBehaviour>().Init(this); ;
    }

    public void New()
    {
        
    }
    
    public void Rest()
    {
        throw new NotImplementedException();
    }
}
