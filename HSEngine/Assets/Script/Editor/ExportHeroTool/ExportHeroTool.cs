using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace HS.Edit
{
    public class ExportHeroTool
    {
        //[MenuItem("HSTool/Export Hero(仅导出资源)", false, 0)]
        [MenuItem("Assets/HSTool/Export Hero(仅导出资源)", false, 0)]
        public static void ExportHero()
        {
            if (Selection.objects == null) return;
            string savePath = Directory.GetCurrentDirectory() + "/HeroPackage/";
            for (int i = 0; Selection.objects != null && i < Selection.objects.Length; ++i)
            {
                Object obj = Selection.objects[i];
                string strPath = AssetDatabase.GetAssetPath(obj);

                string[] ser = new string[] { strPath };
                string[] allRes = AssetDatabase.GetDependencies(ser);

                List<string> filterRes = new List<string>();
                foreach (string s in allRes)
                {
                    if (s.EndsWith(obj.name + ".FBX"))
                    {
                        string[] txts = Directory.GetFiles(Path.GetDirectoryName(s), "*.txt");
                        foreach (string t in txts)
                        {
                            string txt = t.Replace("\\", "/");
                            filterRes.Add(txt);
                        }
                    }
                    if (!s.EndsWith(".cs"))
                    {
                        filterRes.Add(s);
                    }
                }
                
                if (false == Directory.Exists(savePath))
                {
                    //D.LogForce("创建: " + dir);
                    Directory.CreateDirectory(savePath);
                }

                AssetDatabase.ExportPackage(filterRes.ToArray(), savePath + obj.name + ".unitypackage", ExportPackageOptions.Interactive);
                D.LogForce(obj.name + " 导出完成 ");
            }
            System.Diagnostics.Process.Start(savePath);
        }
    }
}


