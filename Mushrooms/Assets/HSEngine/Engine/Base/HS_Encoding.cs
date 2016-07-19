using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace HS.Base
{
    public class HS_Encoding
    {
        /// <summary>
        /// 获取utf8字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string UTF8(byte[] bytes)
        {
#if UNITY_WP8
            UTF8Encoding coding = new UTF8Encoding();
            int charCount = coding.GetCharCount(bytes);
            Char[] chars = new Char[charCount];
            int charsDecodedCount = coding.GetChars(bytes, 0, bytes.Length, chars, 0);
            return chars.ToString();
#else
            return System.Text.Encoding.UTF8.GetString(bytes);
#endif
        }
        
        /// <summary>
        /// 获取Unicode字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Unicode(byte[] bytes)
        {

#if UNITY_WP8
            UnicodeEncoding coding = new UnicodeEncoding();
            int charCount = coding.GetCharCount(bytes);
            Char[] chars = new Char[charCount];
            int charsDecodedCount = coding.GetChars(bytes, 0, bytes.Length, chars, 0);
            return chars.ToString();
#else
            return System.Text.Encoding.Unicode.GetString(bytes);
#endif
        }
    }
}
