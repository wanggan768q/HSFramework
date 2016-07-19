using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using HS.Base;
using HS.IO;

#if UNITY_WP8
using Directory = UnityEngine.Windows.Directory;
#else
#endif

namespace HS.Edit.Build
{
    class HS_ProjectBuild
    {

        private enum FilterEditorDLLModel
        {
            Assets2Backups,
            Backups2Assets,
        }

        public struct ChannelConfig
        {
            public string platform;
            public string packerName;
            public string channel;
            public string defineSymbols;
        }
        public static Dictionary<string, ChannelConfig> S_AllChannelConfig = new Dictionary<string, ChannelConfig>();

        private static string S_RootPath
        {
            get
            {
                return Application.streamingAssetsPath + @"/../..";
            }
        }
        
        public static void LoadConfig()
        {
            if (S_AllChannelConfig.Count > 0)
            {
                return;
            }
            string path = S_RootPath + @"/Config.ini";
            if (!HS_File.Exists(path))
            {
                throw new System.MissingFieldException("配置文件丢失");
            }
            HS_Config.GetInstance().LoadConfigINI(HS_File.ReadAllBytes(path));

            for (int i = 0; i < 100; ++i)
            {
                string section = string.Format("Channel_{0}", i);
                if (!HS_Config.GetInstance().ContainsSection(section))
                {
                    break;
                }
                ChannelConfig config;
                config.platform = HS_Config.GetInstance().GetValue(section, "platform");
                config.packerName = HS_Config.GetInstance().GetValue(section, "packername");
                config.channel = HS_Config.GetInstance().GetValue(section, "channel");
                if (string.IsNullOrEmpty(config.channel))
                {
                    throw new System.NullReferenceException("配置文件 " + section);
                }
                config.defineSymbols = HS_Config.GetInstance().GetValue(section, "definesymbols");
                S_AllChannelConfig.Add(config.channel, config);
            }
        }


        //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
        static string[] GetBuildScenes()
        {
            List<string> names = new List<string>();
            foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
            {
                if (e == null)
                    continue;
                if (e.enabled)
                    names.Add(e.path);
            }
            return names.ToArray();
        }


        static public void BuildForAndroid(HS_EditDefine.HS_ChannelDefine channelDefine)
        {
            LoadConfig();

            string androidPath = Application.dataPath + "/Plugins/Android";
            HS_Directory.CreateDirectory(androidPath);
            try
            {
                HS_Base.SystemDeleteFolder(androidPath);
            }
            catch (System.Exception _e)
            {
                D.LogForce("Ignore: " + _e.ToString());
            }
            string rootPath = S_RootPath;

            ChannelConfig channelConfig = S_AllChannelConfig[channelDefine.ToString()];
            string channel = channelConfig.channel;
            string packerName = channelConfig.packerName;

            HS_Base.SystemCopyDirectory(rootPath + @"/Channel/" + channel + "/Android/", androidPath);

            string lastDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);

            if (!string.IsNullOrEmpty(channelConfig.defineSymbols))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, channelConfig.defineSymbols);
            }
            else
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");
            }
            PlayerSettings.bundleIdentifier = packerName;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            string path = rootPath + "/Client/Android";

            HS_Directory.CreateDirectory(path);
            path += "/" + packerName + ".apk";
            //FilterEditorDLL(FilterEditorDLLModel.Assets2Backups);

            //更新SVN并且拷贝到streamingAssets
            SVNUpdate(RuntimePlatform.Android);
            
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
            //FilterEditorDLL(FilterEditorDLLModel.Backups2Assets);
            if (!string.IsNullOrEmpty(lastDefineSymbols))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, lastDefineSymbols);
            }
            RemoveSVNResources();
            AssetDatabase.Refresh();
            
            System.Diagnostics.Process.Start(HS_Path.GetDirectoryName(path));            
            try
            {
                HS_Base.SystemDeleteFolder(androidPath);
            }
            catch (System.Exception _e)
            {
                D.LogForce("Ignore: " + _e.ToString());
            }
        }

        static public void BuildForWindows(HS_EditDefine.HS_ChannelDefine channelDefine)
        {
            LoadConfig();

            ChannelConfig channelConfig = S_AllChannelConfig[channelDefine.ToString()];
            //string channel = channelConfig.channel;
            string packerName = channelConfig.packerName;
            string rootPath = S_RootPath;

            PlayerSettings.bundleIdentifier = packerName;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            string path = rootPath + "/Client/Windows";

            HS_Directory.CreateDirectory(path);

            try
            {
                HS_Base.SystemDeleteFolder(path);
            }
            catch (System.Exception _e)
            {
                D.LogForce("Ignore: " + _e.ToString());
            }

            string lastDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);

            if (!string.IsNullOrEmpty(channelConfig.defineSymbols))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, channelConfig.defineSymbols);
            }
            else
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "");
            }
            if (!string.IsNullOrEmpty(lastDefineSymbols))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, lastDefineSymbols);
            }

            path += "/" + packerName + ".exe";

            //FilterEditorDLL(FilterEditorDLLModel.Assets2Backups);
            //更新SVN并且拷贝到streamingAssets
            SVNUpdate(RuntimePlatform.WindowsPlayer);
            
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.StandaloneWindows64, BuildOptions.None);
            //FilterEditorDLL(FilterEditorDLLModel.Backups2Assets);
            RemoveSVNResources();
            AssetDatabase.Refresh();
            System.Diagnostics.Process.Start(HS_Path.GetDirectoryName(path));
        }

        private static List<string> s_CopyFolder = new List<string>();
        public static void SVNUpdate(RuntimePlatform platform)
        {
            string platformAssets = HS_PlatformRes.Directory[platform];
            SVNSetiingWindows.Update();
            string streamingAssetsPath = Application.streamingAssetsPath;
            HS_Directory.CreateDirectory(streamingAssetsPath);

            s_CopyFolder.Clear();
            foreach (string p in SVNSetiingWindows.UpdatePaths)
            {
                string folderName = HS_Path.GetCurrentFolder(p);
                string fullFolderName = streamingAssetsPath + @"/" + folderName;
                try
                {
                    HS_Base.SystemDeleteFolder(fullFolderName);
                }
                catch (System.Exception _e)
                {
                    D.LogForce("Ignore: " + _e.ToString());
                }
            }
            AssetDatabase.Refresh();

            foreach (string p in SVNSetiingWindows.UpdatePaths)
            {
                string dstPath = streamingAssetsPath + @"/";
                
                if (p.Contains("Assets"))
                {
                    if (!p.Contains(platformAssets))
                    {
                        continue;
                    }
                    dstPath += platformAssets;
                }
                else
                {
                    dstPath += HS_Path.GetCurrentFolder(p);
                }
                s_CopyFolder.Add(dstPath);
                HS_Base.SystemCopyDirectory(p, dstPath, new string[] { ".svn", ".bat", ".dll" });
            }
            AssetDatabase.Refresh();
            HS_GenerateMD5.GenerateMD5(streamingAssetsPath,"");
        }

        /// <summary>
        /// 移除SVN资源
        /// </summary>
        private static void RemoveSVNResources()
        {
            for (int i = s_CopyFolder.Count - 1; s_CopyFolder.Count > 0 && i >= 0; --i)
            {
                HS_Base.SystemDeleteFolder(s_CopyFolder[i]);
            }
            HS_File.Delete(Application.streamingAssetsPath + @"/Mainfest.data");
        }

        static private void FilterEditorDLL(FilterEditorDLLModel model)
        {
            string rootPath = S_RootPath;
            rootPath = rootPath.Replace("\\", "/");
            string dll = rootPath + "/Assets/Plugins/HSEngineEditor.dll";
            string destDll = rootPath + "/" + HS_Path.GetFileName(dll);

            if (model == FilterEditorDLLModel.Backups2Assets)
            {
                string temp = destDll;
                destDll = dll;
                dll = temp;
            }

            if (HS_File.Exists(dll))
            {
                FileUtil.CopyFileOrDirectory(dll, destDll);
                FileUtil.DeleteFileOrDirectory(dll);
                AssetDatabase.Refresh();
            }
        }
    }

    
}