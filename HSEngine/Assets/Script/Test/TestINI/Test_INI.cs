using UnityEngine;
using System.Collections;
using HS.IO;

public class Test_INI : MonoBehaviour
{

    public TextAsset configFile;
    // Use this for initialization
    void Start()
    {
        //HS_Config.GetInstance().LoadConfigINI(configFile);
        HS_File.WriteAllBytes("c://asdsa//asda//asda//a.txt", new byte[] { 1, 1 });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
