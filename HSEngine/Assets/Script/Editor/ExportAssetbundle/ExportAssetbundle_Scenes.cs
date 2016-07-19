using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using HS.IO;


namespace HS.Edit
{
    public class ExportAssetbundle_Scenes : ExportAssetbundleBase
    {

        

        private List<string> _IgnoreGameObjectName = null;
        private List<string> IgnoreGameObject
        {
            get
            {
                if (_IgnoreGameObjectName == null)
                {
                    _IgnoreGameObjectName = new List<string>();
                    //_IgnoreGameObjectName.Add("FightCamera");
                    //_IgnoreGameObjectName.Add("SceneLight");
                }
                return _IgnoreGameObjectName;
            }
        }

        public void ExportAssetbundle(Object[] selectionGameObjects)
        {
            if (selectionGameObjects == null || selectionGameObjects.Length == 0)
            {
                return;
            }
            foreach (var v in _BuildTargetMap)
            {
                foreach (Object obj in selectionGameObjects)
                {
                    AssetDatabase.Refresh();
                    string assetPath = AssetDatabase.GetAssetPath(obj);
                    bool isOpenSucceed = EditorApplication.OpenScene(assetPath);
                    if (isOpenSucceed)
                    {
                        Dictionary<string, GameObject> ignorePrefabDic = new Dictionary<string, GameObject>();
                        try
                        {
                            foreach (string ignoreObjName in IgnoreGameObject)
                            {
                                GameObject deleteObj = GameObject.Find(ignoreObjName);
                                if (deleteObj != null)
                                {
                                    GameObject prefab = PrefabUtility.GetPrefabParent(deleteObj) as GameObject;
                                    ignorePrefabDic.Add(deleteObj.name, prefab);
                                    GameObject.DestroyImmediate(deleteObj);
                                }
                            }
                        }
                        catch (System.Exception _e)
                        {
                            D.LogError(_e.ToString());
                        }

                        EditorApplication.SaveScene();

                        string locationPathName = RootPath + "/" + v.Value + "/" + obj.name + ".assetbundle";
                        HS_Directory.CreateDirectory(Path.GetDirectoryName(locationPathName));
                        string[] scenes = { assetPath };
                        AssetDatabase.Refresh();
                        string b = BuildPipeline.BuildPlayer(scenes, locationPathName, v.Key, BuildOptions.BuildAdditionalStreamedScenes);
                        Debug.Log("打包完成 => " + b);
                        AssetDatabase.Refresh();

                        try
                        {
                            foreach (var ignoreObj in ignorePrefabDic)
                            {
                                GameObject ins = PrefabUtility.InstantiatePrefab(ignoreObj.Value) as GameObject;
                                ins.name = ignoreObj.Key;
                            }
                        }
                        catch (System.Exception _e)
                        {
                            D.LogError(_e.ToString());
                        }


                        EditorApplication.SaveScene();
                    }
                }
            }
            this.OpenFolder();
            EditorUtility.DisplayDialog("提示", "打包完成", "确认");
        }


    }


}

