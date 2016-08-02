using UnityEngine;
using System.Collections;
using HS.Base;
using System;

public class TimeManager :  HS_SingletonGameObject<TimeManager>
{
    /// <summary>
    /// 游戏开始时间戳
    /// </summary>
    public int StartTime = 0;


	void Start ()
    {
        StartCoroutine(Time());
	}

    IEnumerator Time()
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
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            int t = StartTime + (int)UnityEngine.Time.realtimeSinceStartup;
            D.Log(HS_Time.ConvertToTimePoint(t));
        }
	}
}
