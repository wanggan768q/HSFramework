using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace HS.Base
{
    public static class HS_Time
    {
        private static System.DateTime _BasicDateTime = new System.DateTime(1970, 1, 1, 8, 0, 0);

        private const int ONEDAY = 86400;
        private const int ONEHOUR = 3600;
        private const int ONEMINUTE = 60;

        /// <summary>
        /// 转换为显示时间.
        /// </summary>
        /// <returns>The to time point.</returns>
        /// <param name="seconds">Seconds.</param>
        public static string ConvertToTimePoint(int seconds)
        {
            System.DateTime t = _BasicDateTime.AddSeconds((double)seconds);
            return t.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 转换为显示时间.
        /// </summary>
        /// <returns>The to local time point.</returns>
        /// <param name="seconds">Seconds.</param>
        public static string ConvertToLocalTimePoint(int seconds)
        {
            System.DateTime t = _BasicDateTime.AddSeconds((double)seconds);
            return t.ToLocalTime().ToString("hh:mm:ss");
        }

        /// <summary>
        /// Unixs 时间戳转换为时间差.
        /// </summary>
        /// <returns>The to time span.</returns>
        /// <param name="now">Now.</param>
        public static System.TimeSpan UnixToTimeSpan(string now)
        {
            string timeStamp = now;
            System.DateTime dtStart = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long l = long.Parse(timeStamp + "0000000");
            return new System.TimeSpan(l);
        }

        /// <summary>
        /// 转换一段时间的显示方式
        /// </summary>
        /// <returns>显示时间.</returns>
        /// <param name="totalSeconds">一段时间.</param>
        /// <param name="showSeconds">If set to <c>true</c> 显示秒.</param>
        /// <param name="showMinutes">If set to <c>true</c> 显示分钟.</param>
        /// <param name="showHours">If set to <c>true</c> 显示小时s.</param>
        /// <param name="showDays">If set to <c>true</c> 显示日期.</param>
        /// <param name="cn">If set to <c>true</c> 是否按中文方式显示.</param>
        /// <param name="showZero">If set to <c>true</c> 为0值时是否显示.</param>
        /// <param name="format">格式化方式 D	十进制	string.Format("{0:D3}", 2)	002.</param>
        public static string ConvertToTimeDuration(int totalSeconds, bool showSeconds = true, bool showMinutes = true, bool showHours = false, bool showDays = false, bool cn = false, bool showZero = true, string format = "D2", bool showHoursLess = false, bool showMinutesLess = false, bool showSecondsLess = false, int limitCount = 4)
        {
            int seconds = totalSeconds % 60;
            totalSeconds /= 60;
            int minutes = totalSeconds % 60;
            totalSeconds /= 60;
            int hours = totalSeconds % 24;
            totalSeconds /= 24;
            int days = totalSeconds % 999999;

            List<string> content = new List<string>();
            string time = "";
            int limit = 0;
            if (showDays)
            {
                if (days > 0 || showZero)
                {
                    if (limit < limitCount)
                    {
                        time = string.Format("{0:" + format + "}", days);
                        if (cn)
                        {
                            content.Add(string.Format("TimeDays", time));
                        }
                        else
                        {
                            content.Add(time);
                        }
                        limit++;
                    }
                }
            }
            if (showHours || (showHoursLess && days == 0))
            {
                if (hours > 0 || showZero)
                {
                    if (limit < limitCount)
                    {
                        time = string.Format("{0:" + format + "}", hours);
                        if (cn)
                        {
                            content.Add(string.Format("TimeHours", time));
                        }
                        else
                        {
                            content.Add(time);
                        }
                        limit++;
                    }
                }
            }
            if (showMinutes || (showMinutesLess && days == 0 && hours == 0))
            {
                if (minutes > 0 || showZero)
                {
                    if (limit < limitCount)
                    {
                        time = string.Format("{0:" + format + "}", minutes);
                        if (cn)
                        {
                            content.Add(string.Format("TimeMinutes", time));
                        }
                        else
                        {
                            content.Add(time);
                        }
                        limit++;
                    }
                }
            }
            if (showSeconds || (showSecondsLess && days == 0 && hours == 0 && minutes == 0))
            {
                if (seconds > 0 || showZero)
                {
                    if (limit < limitCount)
                    {
                        time = string.Format("{0:" + format + "}", seconds);
                        if (cn)
                        {
                            content.Add(string.Format("TimeSeconds", time));
                        }
                        else
                        {
                            content.Add(time);
                        }
                        limit++;
                    }
                }
            }
            if (cn)
            {
                return string.Join("", content.ToArray());
            }
            return string.Join(":", content.ToArray());
        }

        /// <summary>
        /// 转换一段时间的显示方式
        /// </summary>
        /// <returns>显示时间.</returns>
        /// <param name="time">一段时间.</param>
        /// <param name="showSeconds">If set to <c>true</c> 显示秒.</param>
        /// <param name="showMinutes">If set to <c>true</c> 显示分钟.</param>
        /// <param name="showHours">If set to <c>true</c> 显示小时s.</param>
        /// <param name="showDays">If set to <c>true</c> 显示日期.</param>
        /// <param name="cn">If set to <c>true</c> 是否按中文方式显示.</param>
        /// <param name="showZero">If set to <c>true</c> 为0值时是否显示.</param>
        /// <param name="format">格式化方式 D	十进制	string.Format("{0:D3}", 2)	002.</param>
        public static string ConvertToTimeDuration(System.TimeSpan time, bool showSeconds = true, bool showMinutes = true, bool showHours = false, bool showDays = false, bool cn = false, bool showZero = true, string format = "D2", bool showHoursLess = false, bool showMinutesLess = false, bool showSecondsLess = false)
        {
            double d = time.TotalSeconds;
            int totalSeconds = Convert.ToInt32(d);
            return HS_Time.ConvertToTimeDuration(totalSeconds, showSeconds, showMinutes, showHours, showDays, cn, showZero, format, showHoursLess, showMinutesLess, showSecondsLess);

        }

        /// <summary>
        /// 获取当前时间（秒）
        /// </summary>
        /// <returns></returns>
        public static int GetNowSeconds()
        {
            System.TimeSpan duration = System.DateTime.Now - _BasicDateTime;
            return duration.Seconds + duration.Minutes * 60 + duration.Hours * 3600 + duration.Days * 86400;
        }

        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static System.TimeSpan GetTimeSpan(int seconds)
        {
            System.DateTime end = _BasicDateTime.AddSeconds((double)seconds);
            System.TimeSpan duration = end - System.DateTime.Now;
            return duration;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            int unixTime = (int)(time - new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
            return unixTime;
        }

        /// <summary>
        /// 是否是相同日
        /// </summary>
        /// <returns></returns>
        public static bool IsSameTimeByDay(int seconds)
        {
            System.DateTime temptime = _BasicDateTime.AddSeconds((double)seconds);
            return System.DateTime.Now.Date == temptime.Date ? true : false;
        }
    }
}

