using UnityEngine;
using System.Collections;

public class Test_WP8 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        LitJson.JsonData jd = new LitJson.JsonData();
        jd["TestJson"] = 1;
        GUILayout.Label(jd.ToJson());
    }



    // Update is called once per frame
    void Update()
    {
#if (UNITY_WP8 || UNITY_METRO)
#endif
    }
}
