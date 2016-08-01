using UnityEngine;
using System.Collections;
using HS.Manager;

public class Main : MonoBehaviour {

    void Awake()
    {
        D.EnableLog = true;
    }

	void Start () {
        //HS_ViewManager.Open<UILogoView>();
        StartCoroutine(Load());
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
