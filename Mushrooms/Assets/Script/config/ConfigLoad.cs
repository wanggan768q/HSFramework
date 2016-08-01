using UnityEngine;
using System.Collections;
using HS.Base;

public class ConfigLoad : HS_SingletonGameObject<ConfigLoad> {

	private string textContent;

	private int fileCount = 2;

	public delegate void ConfigLoadProgress(float f);

    public event ConfigLoadProgress configLoadProgress;

	public IEnumerator LoadConfig () {

		yield return StartCoroutine(LoadData("MushroomsCfg.json"));
		MushroomsCfgTable.Instance.LoadJson(textContent);
		Progress(1);
		yield return StartCoroutine(LoadData("ScenesCfg.json"));
		ScenesCfgTable.Instance.LoadJson(textContent);
		Progress(2);


		yield return true;
	}

    IEnumerator LoadData (string name) {

		string path = HS_Base.GetStreamingAssetsFilePath(name, "json");
	
		WWW www = new WWW(path);
		yield return www;

		textContent = www.text;
		yield return true;
	}

	void Progress(int index)
    {
        if (configLoadProgress != null)
        {
            configLoadProgress((float)index / (float)fileCount);
        }
    }
}
