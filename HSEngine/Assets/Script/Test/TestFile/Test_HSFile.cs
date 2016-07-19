using UnityEngine;
using System.Collections;
using HS.IO;

public class Test_HSFile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HS_File.ReadAllText(@"F:/Work247/DotaClient/StreamingAssets/Mainfest.data");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
