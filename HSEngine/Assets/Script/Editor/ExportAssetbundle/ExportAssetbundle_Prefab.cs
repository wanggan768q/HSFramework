using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using HS.IO;

namespace HS.Edit
{
    public class ExportAssetbundle_Prefab : ExportAssetbundleBase
    {
        public void ExportAssetbundle(Object[] selectionGameObjects)
        {
            if (selectionGameObjects == null || selectionGameObjects.Length == 0)
            {
                return;
            }
            AssetDatabase.SaveAssets();

            foreach (var v in _BuildTargetMap)
            {
                foreach (Object obj in selectionGameObjects)
                {
                    if (obj is GameObject || obj is Texture2D)
                    {
                        AssetDatabase.Refresh();
                        string locationPathName = RootPath + "/" + v.Value + "/" + obj.name + ".assetbundle";
                        HS_Directory.CreateDirectory(HS_Path.GetDirectoryName(locationPathName));
                        BuildPipeline.BuildAssetBundle(obj, null, locationPathName, BuildAssetBundleOptions.CollectDependencies, v.Key);
                    }
                    else
                    {
                        D.LogForce(obj.name + " 不是预制体");
                    }
                }
            }
            this.OpenFolder();
            EditorUtility.DisplayDialog("提示", "打包完成", "确认");
        }
    }


}

