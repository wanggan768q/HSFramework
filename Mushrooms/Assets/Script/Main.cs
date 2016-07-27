using UnityEngine;
using System.Collections;
using HS.Manager;

public class Main : MonoBehaviour {

    void Awake()
    {
        D.EnableLog = true;
    }

	void Start () {
        HS_ViewManager.Open<UILogoView>();
    }
	
	void Update () {
	
	}
}
