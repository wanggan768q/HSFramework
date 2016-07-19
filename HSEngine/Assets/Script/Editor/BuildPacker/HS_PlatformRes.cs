using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HS.Edit.Build
{
    public class HS_PlatformRes
    {
        public static Dictionary<RuntimePlatform, string> Directory = new Dictionary<RuntimePlatform, string>()
        {
            { RuntimePlatform.WindowsPlayer,"WAssets"},
            { RuntimePlatform.OSXPlayer,"MAssets"},
            { RuntimePlatform.Android,"AAssets"},
            { RuntimePlatform.IPhonePlayer,"IAssets"},
            { RuntimePlatform.WP8Player,"WP8Assets"},
        };

    }
}