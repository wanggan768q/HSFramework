using UnityEngine;
using System.Collections;

namespace HS.Manager
{
    public class HS_ResourceManager : MonoBehaviour
    {
        public static T LoadAsset<T>(string name) where T : UnityEngine.Object
        {
            string[] assets = UnityEditor.AssetDatabase.FindAssets(name + " t:Object", new string[] { "Assets/SubAssets/Res" });
            string assetPath = "";
            if (assets.Length >= 1)
            {
                int pathCount = 0;
                for (int i = 0; i < assets.Length; i++)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[i]);
                    if (System.IO.Path.GetFileNameWithoutExtension(path) != name)
                    {
                        continue;
                    }
                    pathCount++;
                    assetPath = path;
                    //Debug.Log(path);
                }
                if (pathCount > 1)
                {
                    Debug.LogError("More than 1 resource in assets" + name);
                }

            }

            //			if(Singleton<AssetBundleManager>.Instance.LoadAsset<T>(name)!=null)
            //				return Singleton<AssetBundleManager>.Instance.LoadAsset<T>(name);
            //			else
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
    }

}
