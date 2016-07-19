using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using HS.Edit.Build;


namespace HS.Tool.Edit
{
    //% (Windows上为ctrl, OS X上为cmd), # (shift), & (alt)
    public class HS_BuildPacker
    {
        [MenuItem("HSTool/Build/Windows x64 &w", false, 0)]
        public static void S_BuildPacker_Windows()
        {
            HS_ProjectBuild.BuildForWindows(HS_EditDefine.HS_ChannelDefine.Windows);
        }

        [MenuItem("HSTool/Build/Android &a", false, 1)]
        public static void S_BuildPacker_Default()
        {
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.Android);
        }

        [MenuItem("HSTool/Build/Windows x64 - Android  &#b", false, 2)]
        public static void S_BuildPacker_WindowsDefault()
        {
            S_BuildPacker_Windows();
            S_BuildPacker_Default();
        }

        [MenuItem("HSTool/Build/小米", false, 3)]
        public static void S_BuildPacker_XM()
        {
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.XM);
        }

        [MenuItem("HSTool/Build/云测", false, 4)]
        public static void S_BuildPacker_TestIn()
        {
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.TestIn);
        }


        public void BuildAndroid(HS_EditDefine.HS_ChannelDefine channel)
        {
            HS_CMDParameter.projectName = (int)channel + "";
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.Android);
        }

    }
}

