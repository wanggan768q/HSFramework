using UnityEngine;
using System.Collections;
using UnityEditor;
using HS.IO;

public class GameTool
{

    [MenuItem("GameTool/Batch Set Packing Tag", false)]
    public static void SetPackingTagBatch()
    {
        string resPath = HS_Path.CombinePath("Assets", "SubAssets", "Raw", "UI");
        string[] allFolders = AssetDatabase.GetSubFolders(resPath);
        for (int i = 0; i < allFolders.Length; i++)
        {
            string[] pathDatas = HS_Path.SplitPath(allFolders[i]);
            string tagName = pathDatas[pathDatas.Length - 1];
            string[] allAssets = AssetDatabase.FindAssets("t:Sprite", new string[] { allFolders[i] });
            for (int j = 0; j < allAssets.Length; j++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(allAssets[j]);
                TextureImporter ti = TextureImporter.GetAtPath(assetPath) as TextureImporter;
                FormatTexture(ti, tagName);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Done!");
    }

    private static void FormatTexture(TextureImporter ti, string tagName)
    {
        ti.spritePackingTag = tagName;
        ti.mipmapEnabled = false;
        ti.SetPlatformTextureSettings("iPhone", 2048, TextureImporterFormat.ARGB32);
        ti.SaveAndReimport();
    }

    [MenuItem("GameTool/Export Sprite", false)]
    public static void ExportSprite()
    {
        string sourcePath = HS_Path.CombinePath("Assets", "SubAssets", "Raw", "Sprite");
        string targetPath = HS_Path.CombinePath("Assets", "SubAssets", "Res", "Prefabs", "Sprite");
        string[] allFolders = AssetDatabase.GetSubFolders(sourcePath);
        for (int i = 0; i < allFolders.Length; i++)
        {
            string[] pathDatas = HS_Path.SplitPath(allFolders[i]);
            string tagName = pathDatas[pathDatas.Length - 1];
            string[] allAssets = AssetDatabase.FindAssets("t:Sprite", new string[] { allFolders[i] });
            for (int j = 0; j < allAssets.Length; j++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(allAssets[j]);
                TextureImporter ti = TextureImporter.GetAtPath(assetPath) as TextureImporter;
                FormatTexture(ti, tagName);

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                GameObject go = new GameObject(sprite.name);
                go.AddComponent<SpriteRenderer>().sprite = sprite;

                if (!AssetDatabase.IsValidFolder(HS_Path.CombinePath(targetPath, tagName)))
                {
                    AssetDatabase.CreateFolder(targetPath, tagName);
                }
                string prefabPath = HS_Path.CombinePath(HS_Path.CombinePath(targetPath, tagName), sprite.name + ".prefab");
                PrefabUtility.CreatePrefab(prefabPath, go);
                GameObject.DestroyImmediate(go);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Done!");
    }

    [MenuItem("GameTool/Export Music", false)]
    public static void ExportMusic()
    {
        string sourcePath = HS_Path.CombinePath("Assets", "SubAssets", "Raw", "Music");
        string targetPath = HS_Path.CombinePath("Assets", "SubAssets", "Res", "Prefabs", "Music");

        string[] allAssets = AssetDatabase.FindAssets("t:Object", new string[] { sourcePath });
        for (int i = 0; i < allAssets.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(allAssets[i]);
            AudioClip prefab = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
            if (prefab != null)
            {
                string prefabPath = HS_Path.CombinePath(targetPath, HS_Path.GetFileName(assetPath));
                AssetDatabase.CopyAsset(assetPath, prefabPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Done!");
    }
}
