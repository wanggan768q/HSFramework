using UnityEngine;
using System.Collections;
using HS.Manager;

public class Main : MonoBehaviour {

    void Awake()
    {
        D.EnableLog = true;

        TimeManager.GetInstance();

        GameArchive.GetInstance().Read();
    }

	void Start () {
        //HS_ViewManager.Open<UILogoView>();
        HS_ViewManager.Open<UITestView>();
        //StartCoroutine(Load());
    }
	

    IEnumerator Load()
    {
        ConfigLoad.GetInstance().configLoadProgress += (float f) =>
        {
            D.Log(f);
        };
        yield return ConfigLoad.GetInstance().LoadConfig();
        ConfigLoad.GetInstance().Destory();
    }

	void Update () {
	
	}
}
