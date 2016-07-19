using UnityEngine;
using System.Collections;
using UnityEditor;
using HS.Edit.Build;
using HS.Tool.Edit;
using HS.IO;
using System.IO;

namespace HS.Edit
{
    //% (Windows上为ctrl, OS X上为cmd), # (shift), & (alt)
    public class HSToolMenu
    {
        [MenuItem("HSTool/初始化开发目录", false, 0)]
        public static void Init()
        {
            string root = Directory.GetCurrentDirectory();
            //代码目录
            HS_Directory.CreateDirectory(root + "/Assets/Script/Sprite");
            HS_Directory.CreateDirectory(root + "/Assets/Script/VLayer/Base");
            HS_Directory.CreateDirectory(root + "/Assets/Script/VLayer/View");

            //资源目录
            HS_Directory.CreateDirectory("Assets/SubAssets/Res/Prefabs/UI");

            AssetDatabase.Refresh();
        }

        #region HSTool

        #region 打包
        [MenuItem("HSTool/Build/Windows x64 &w", false, 0)]
        public static void BuildPacker_Windows()
        {
            HS_ProjectBuild.BuildForWindows(HS_EditDefine.HS_ChannelDefine.Windows);
        }

        [MenuItem("HSTool/Build/Android &a", false, 1)]
        public static void BuildPacker_Default()
        {
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.Android);
        }

        [MenuItem("HSTool/Build/Windows x64 - Android  &#b", false, 2)]
        public static void S_BuildPacker_WindowsDefault()
        {
            BuildPacker_Windows();
            BuildPacker_Default();
        }

        [MenuItem("HSTool/Build/小米", false, 3)]
        public static void BuildPacker_XM()
        {
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.XM);
        }

        [MenuItem("HSTool/Build/云测", false, 4)]
        public static void BuildPacker_TestIn()
        {
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.TestIn);
        }

        public void BuildAndroid(HS_EditDefine.HS_ChannelDefine channel)
        {
            HS_CMDParameter.projectName = (int)channel + "";
            HS_ProjectBuild.BuildForAndroid(HS_EditDefine.HS_ChannelDefine.Android);
        }
        #endregion
        

        [MenuItem("HSTool/BatchChangeTextureFormat (批量改变纹理格式)", false, 1)]
        public static void BatchChangeTextureFormat()
        {
            ChangeTextureFormatTool tool = new ChangeTextureFormatTool();
            tool.BatchChangeTextureFormat();
        }

        [UnityEditor.MenuItem("HSTool/ImportUnitypackageFolder", false, 2)]
        public static void ImportUnitypackageFolder()
        {
            EditorWindow.GetWindow<ImportUnitypackage>( "批量导入Unitypackage");
        }
        
        // [MenuItem("HSTool/Find/FindPrefabChinese (查到指定目录下所有包含汉字的预制体)", false, 3)]
        // public static void FindPrefabChinese()
        // {
        //     FindPrefabChinese tool = EditorWindow.GetWindow<FindPrefabChinese>(true, "查到指定目录下所有包含汉字的预制体");
        //     tool.Find();
        // }

        [UnityEditor.MenuItem("HSTool/SVNSetiingWindows", false, 4)]
        public static void SVNSetiingWindows()
        {
            EditorWindow.GetWindow<SVNSetiingWindows>(true, "SVN相关设置");
        }

        #endregion












        #region Assets

        //[MenuItem("HSTool/Export Assetbundle/Scene (打包场景)", false, 0)]
        [MenuItem("Assets/HSTool/Export Assetbundle/Prefab (打包模型、图片)", false, 0)]
        public static void ExportPrefab()
        {
            if (Selection.objects == null) return;

            ExportAssetbundle_Prefab export = new ExportAssetbundle_Prefab();
            export.ExportAssetbundle(Selection.objects);
        }

        //[MenuItem("HSTool/Export Assetbundle/Scene (打包场景)", false, 0)]
        [MenuItem("Assets/HSTool/Export Assetbundle/Scene (打包场景)", false, 0)]
        public static void ExportScene()
        {
            if (Selection.objects == null) return;

            ExportAssetbundle_Scenes export = new ExportAssetbundle_Scenes();
            export.ExportAssetbundle(Selection.objects);
        }

        [MenuItem("Assets/HSTool/ChangeTextureFormat (改变纹理格式)", false, 0)]
        public static void ChangeTextureFormat()
        {
            ChangeTextureFormatTool tool = new ChangeTextureFormatTool();
            tool.ChangeTextureFormat();
        }

        

        [MenuItem("Assets/HSTool/BatchChangeAudioClip2D (批量改变音效为2D)", false, 4)]
        public static void ChangeAudioClip2D()
        {
            ChangeAudioClip2DFormatTool tool = new ChangeAudioClip2DFormatTool();
            tool.ChangeAudioClip2DFormat();
        }
        #endregion


        #region Find
        
        #endregion

    }
}


