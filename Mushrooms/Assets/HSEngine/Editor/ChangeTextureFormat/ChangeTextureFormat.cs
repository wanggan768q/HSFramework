using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


namespace HS.Tool.Edit
{
    public class ChangeTextureFormatTool
    {
        

        /// <summary>
        /// 改变纹理格式
        /// </summary>
        public void ChangeTextureFormat()
        {
            if (Selection.objects == null) return;
            for (int i = 0; Selection.objects != null && i < Selection.objects.Length; ++i)
            {
                Object obj = Selection.objects[i];
                if (obj != null)
                {
                    if (obj is Texture2D)
                    {
                        string path = AssetDatabase.GetAssetPath(obj);
                        this.DisplayProgress("改变纹理格式", path, i, Selection.objects.Length);
                        this.Change(path);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 批量改变纹理格式
        /// </summary>
        public void BatchChangeTextureFormat()
        {
            string[] paths = AssetDatabase.GetAllAssetPaths();
            List<string> pngPaths = new List<string>();
            foreach (string p in paths)
            {
                if (p.Contains("/Atlas/"))
                {
                    if (p.EndsWith(".png"))
                    {
                        pngPaths.Add(p);
                    }
                }
            }
            for (int i = 0; i < pngPaths.Count; ++i)
            {

                this.DisplayProgress("批量改变纹理格式", pngPaths[i], i, pngPaths.Count);
                this.Change(pngPaths[i]);
            }
            EditorUtility.ClearProgressBar();

        }

        private void Change(string path)
        {
            if (path == null || path == "")// || !(obj is Texture2D)
            {
                return;
            }
            string log = path;
            TextureImporter ti = TextureImporter.GetAtPath(path) as TextureImporter;
            ti.textureType = TextureImporterType.Default;
            ti.alphaIsTransparency = true;
            ti.mipmapEnabled = false;
            ti.wrapMode = TextureWrapMode.Clamp;
            ti.filterMode = FilterMode.Trilinear;

            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
            log = log.Insert(0, "[ OK ]\t");
            D.LogForce(log);
        }

        private void DisplayProgress(string title, string info, int index, int maxCount)
        {
            float p = ((float)index + 1f) / (float)maxCount;
            int p1 = (int)(p * 100);
            string str = "[ " + p1 + "% ] " + info;
            EditorUtility.DisplayProgressBar(title, str, p);
        }


        //EditorUtility.ClearProgressBar();
    }
}

