using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class GameArchive : HS.Base.HS_Singleton<GameArchive>
{
    [Serializable]
    public struct GameData
    {
        int lastTime;   //最后的时间戳
        int Humidity;   //湿度
        List<MushroomInfo> MushroomInfoList;

        [Serializable]
        struct MushroomInfo
        {
            int Id;
            string ImageName;
            int RipeTime;
            RectTransform transform;
        }
    }
    public GameData Data;

    private const string C_KEY = "Game";

    private bool SerializeObjToStr(System.Object obj, out string serializedStr)
    {
        bool serializeOk = false;
        serializedStr = "";
        try
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            serializedStr = System.Convert.ToBase64String(memoryStream.ToArray());

            serializeOk = true;
        }
        catch
        {
            serializeOk = false;
        }

        return serializeOk;
    }

    private bool DeserializeStrToObj(string serializedStr, out object deserializedObj)
    {
        bool deserializeOk = false;
        deserializedObj = null;

        try
        {
            byte[] restoredBytes = System.Convert.FromBase64String(serializedStr);
            MemoryStream restoredMemoryStream = new MemoryStream(restoredBytes);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            deserializedObj = binaryFormatter.Deserialize(restoredMemoryStream);

            deserializeOk = true;
        }
        catch
        {
            deserializeOk = false;
        }

        return deserializeOk;
    }


    public void Read()
    {
        if(!PlayerPrefs.HasKey(C_KEY))
        {
            return;
        }
        string s = PlayerPrefs.GetString(C_KEY);
        object obj;
        if(DeserializeStrToObj(s,out obj))
        {
            Data = (GameData)obj;
        }
    }

    public void Save()
    {
        string s = string.Empty;
        SerializeObjToStr(Data, out s);
        PlayerPrefs.SetString(C_KEY, s);
    }

}
