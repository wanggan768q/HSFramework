using UnityEngine;
using System.Collections;


namespace HS.Edit.Build
{
    public sealed class HS_EditDefine
    {
        public enum HS_ChannelDefine
        {
            Windows = 0,
            Android = 1,
            XM = 2,
            TestIn=3,
        }
        public static readonly string[] AllChannelName = new string[] { 
            "Windows", 
            "Android",
            "小米",
            "云测"};
    }



}
