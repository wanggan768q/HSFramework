using UnityEngine;
using System.Collections;
using HS.Net.TCP;

public class Test_Ping : MonoBehaviour {

    //Ping ping;
	// Use this for initialization
	void Start ()
    {
        //ping = new Ping("123.56.122.228");
        //HS_PingManager.GetInstance().OnStart("10.2.9.189", 1);
        HS_TelnetManager.GetInstance().OnStart("10.2.9.189", 2443,1);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Debug.Log(string.Format("IP:{0} , {1} , {2}", ping.ip, ping.time, ping.isDone));
	}
}
