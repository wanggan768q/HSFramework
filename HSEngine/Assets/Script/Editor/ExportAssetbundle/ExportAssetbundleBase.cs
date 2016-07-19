using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using HS.IO;

namespace HS.Edit
{
    public class ExportAssetbundleBase
    {
        protected Dictionary<BuildTarget, string> _BuildTargetMap = new Dictionary<BuildTarget, string>();
        public ExportAssetbundleBase()
        {
            _BuildTargetMap.Add(BuildTarget.StandaloneWindows, "WAssets");
            //_BuildTargetMap.Add(BuildTarget.StandaloneWindows64, "WAssets");

            _BuildTargetMap.Add(BuildTarget.StandaloneOSXIntel64, "MAssets");

            _BuildTargetMap.Add(BuildTarget.iOS, "IAssets");

            _BuildTargetMap.Add(BuildTarget.Android, "AAssets");

            _BuildTargetMap.Add(BuildTarget.WP8Player, "WP8Assets");
        }

        protected string RootPath
        {
            get
            {
                string path = Application.streamingAssetsPath + @"/../../StreamingAssets";
                HS_Directory.CreateDirectory(path);
                return path;
            }
        }

        protected void OpenFolder()
        {
            System.Diagnostics.Process.Start(RootPath);
        }
    }

}
