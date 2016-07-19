using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


namespace HS.Tool.Edit
{
    public class ChangeAudioClip2DFormatTool
    {
        public void ChangeAudioClip2DFormat()
        {
            if (Selection.objects == null) return;
            for (int i = 0; Selection.objects != null && i < Selection.objects.Length; ++i)
            {
                Object obj = Selection.objects[i];
                if (obj != null)
                {
                    if (obj is AudioClip)
                    {
                        string path = AssetDatabase.GetAssetPath(obj);
                        this.DisplayProgress("改变音效为2D", path, i, Selection.objects.Length);
                        this.Change(path);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }

        

        private void Change(string path)
        {
            if (path == null || path == "")
            {
                return;
            }
            string log = path;
            AudioImporter ai = TextureImporter.GetAtPath(path) as AudioImporter;
            ai.threeD = false;

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

