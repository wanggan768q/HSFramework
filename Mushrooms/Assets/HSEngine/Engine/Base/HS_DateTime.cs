using UnityEngine;
using System.Collections;
using System;

namespace HS.Base
{
    public class HS_DateTime
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"> 会在后面添加 "0000"</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToString(long startTime, string format)
        {
            string strTime = string.Empty;
#if UNITY_EDITOR && !UNITY_WP8
            DateTime data = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan times = new TimeSpan(long.Parse(startTime + "0000"));
            strTime = data.Add(times).ToString(format);
#elif UNITY_WP8
            DateTime data = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            TimeSpan times = new TimeSpan(long.Parse(startTime + "0000"));
            strTime = data.Add(times).ToString(format);
#endif
            return strTime;
        }
    }
}

