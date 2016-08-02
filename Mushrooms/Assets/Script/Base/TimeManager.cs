using UnityEngine;
using System.Collections;
using HS.Base;
using System;

public class TimeManager :  HS_SingletonGameObject<TimeManager>
{
    /// <summary>
    /// 游戏开始时间戳
    /// </summary>
    public int StartTime
    {
        get;
        private set;
    }

    /// <summary>
    /// 当前真实网络时间
    /// </summary>
    public int TotalTime
    {
        get
        {
            return StartTime + (int)UnityEngine.Time.realtimeSinceStartup;
        }
    }
    
    void Start ()
    {
        UpdateStartTime((int startTime)=> 
        {
            D.Log("最新时间戳: " + startTime + " => " + HS_Time.ConvertToTimePoint(startTime));
        });
    }

    IEnumerator Time(System.Action<int> action)
    {
        WWW w = new WWW("http://www.baidu.com/");
        yield return w;

        if(!string.IsNullOrEmpty(w.error))
        {
            D.LogError("获取时间错误");
            yield return true;
        }
        string date = w.responseHeaders["DATE"];
        DateTime t;
        if(DateTime.TryParse(date,out t))
        {
            StartTime = HS_Time.ConvertDateTimeInt(t);
            if(action != null)
            {
                action(StartTime);
            } 
        }
    }

    public void UpdateStartTime(System.Action<int> action)
    {
        StartCoroutine(Time(action));
    }

    
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            int t = StartTime + (int)UnityEngine.Time.realtimeSinceStartup;
            D.Log(HS_Time.ConvertToTimePoint(t));
        }
	}
}
