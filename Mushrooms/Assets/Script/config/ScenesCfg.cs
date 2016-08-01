using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using HS.IO;
using LitJson;


public class ScenesCfgElement
{
	/// <summary>
	/// 场景ID
	/// </summary>
	public int Id;

	/// <summary>
	/// 场景图片名字(大图片)
	/// </summary>
	public string BigImageName;

	/// <summary>
	/// 场景图片名字(横条图)
	/// </summary>
	public string SmallImageName;

	/// <summary>
	/// 购买价格
	/// </summary>
	public int BuyPrice;

	/// <summary>
	/// 可以生长哪些蘑菇	例：1|2|3	就是可以生长蘑菇编号为 1 2 3的蘑菇
	/// </summary>
	public string GrowthMushroomsList;


	public bool IsValidate = false;
	public ScenesCfgElement()
	{
		Id = 0;
		IsValidate = false;

	}
};


public class ScenesCfgTable
{

	private ScenesCfgTable()
	{
		_MapElements = new Dictionary<int, ScenesCfgElement>();
		_EmptyItem = new ScenesCfgElement();
		_VecAllElements = new List<ScenesCfgElement>();
	}
	private Dictionary<int, ScenesCfgElement> _MapElements = null;
	private List<ScenesCfgElement>	_VecAllElements = null;
	private ScenesCfgElement _EmptyItem = null;
	private static ScenesCfgTable _SInstance = null;

	public static ScenesCfgTable Instance
	{
		get
		{
			if( _SInstance != null )
				return _SInstance;	
			_SInstance = new ScenesCfgTable();
			return _SInstance;
		}
	}

	public ScenesCfgElement GetElement(int key)
	{
		if( _MapElements.ContainsKey(key) )
			return _MapElements[key];
		return _EmptyItem;
	}

	public int GetElementCount()
	{
		return _MapElements.Count;
	}
	public bool HasElement(int key)
	{
		return _MapElements.ContainsKey(key);
	}

  public List<ScenesCfgElement> GetAllElement(Predicate<ScenesCfgElement> matchCB = null)
	{
        if( matchCB==null || _VecAllElements.Count == 0)
            return _VecAllElements;
        return _VecAllElements.FindAll(matchCB);
	}

	public bool Load()
	{
		
		string strTableContent = "";
		if(HS_ByteRead.ReadCsvFile("ScenesCfg.json", out strTableContent ) )
			return LoadCsv( strTableContent );
		byte[] binTableContent = null;
		if( !HS_ByteRead.ReadBinFile("ScenesCfg.bin", out binTableContent ) )
		{
			Debug.Log("配置文件[ScenesCfg.bin]未找到");
			return false;
		}
		return LoadBin(binTableContent);
	}


	public bool LoadBin(byte[] binContent)
	{
		_MapElements.Clear();
		_VecAllElements.Clear();
		int nCol, nRow;
		int readPos = 0;
		readPos += HS_ByteRead.ReadInt32Variant( binContent, readPos, out nCol );
		readPos += HS_ByteRead.ReadInt32Variant( binContent, readPos, out nRow );
		List<string> vecLine = new List<string>(nCol);
		List<int> vecHeadType = new List<int>(nCol);
        string tmpStr;
        int tmpInt;
		for( int i=0; i<nCol; i++ )
		{
            readPos += HS_ByteRead.ReadString(binContent, readPos, out tmpStr);
            readPos += HS_ByteRead.ReadInt32Variant(binContent, readPos, out tmpInt);
            vecLine.Add(tmpStr);
            vecHeadType.Add(tmpInt);
		}
		if(vecLine.Count != 5)
		{
			Debug.Log("ScenesCfg.json中列数量与生成的代码不匹配!");
			return false;
		}
		if(vecLine[0]!="Id") { Debug.Log("ScenesCfg.json中字段[Id]位置不对应"); return false; }
		if(vecLine[1]!="BigImageName") { Debug.Log("ScenesCfg.json中字段[BigImageName]位置不对应"); return false; }
		if(vecLine[2]!="SmallImageName") { Debug.Log("ScenesCfg.json中字段[SmallImageName]位置不对应"); return false; }
		if(vecLine[3]!="BuyPrice") { Debug.Log("ScenesCfg.json中字段[BuyPrice]位置不对应"); return false; }
		if(vecLine[4]!="GrowthMushroomsList") { Debug.Log("ScenesCfg.json中字段[GrowthMushroomsList]位置不对应"); return false; }


		for(int i=0; i<nRow; i++)
		{
			ScenesCfgElement member = new ScenesCfgElement();
			readPos += HS_ByteRead.ReadInt32Variant(binContent, readPos, out member.Id );
			readPos += HS_ByteRead.ReadString(binContent, readPos, out member.BigImageName );
			readPos += HS_ByteRead.ReadString(binContent, readPos, out member.SmallImageName );
			readPos += HS_ByteRead.ReadInt32Variant(binContent, readPos, out member.BuyPrice );
			readPos += HS_ByteRead.ReadString(binContent, readPos, out member.GrowthMushroomsList );

			member.IsValidate = true;
			_VecAllElements.Add(member);
			_MapElements[member.Id] = member;
		}
		return true;
	}
	public bool LoadCsv(string strContent)
	{
		if( strContent.Length == 0 )
			return false;
		_MapElements.Clear();
		_VecAllElements.Clear();
		int contentOffset = 0;
		List<string> vecLine;
		vecLine = HS_ByteRead.readCsvLine( strContent, ref contentOffset );
		if(vecLine.Count != 5)
		{
			Debug.Log("ScenesCfg.json中列数量与生成的代码不匹配!");
			return false;
		}
		if(vecLine[0]!="Id") { Debug.Log("ScenesCfg.json中字段[Id]位置不对应"); return false; }
		if(vecLine[1]!="BigImageName") { Debug.Log("ScenesCfg.json中字段[BigImageName]位置不对应"); return false; }
		if(vecLine[2]!="SmallImageName") { Debug.Log("ScenesCfg.json中字段[SmallImageName]位置不对应"); return false; }
		if(vecLine[3]!="BuyPrice") { Debug.Log("ScenesCfg.json中字段[BuyPrice]位置不对应"); return false; }
		if(vecLine[4]!="GrowthMushroomsList") { Debug.Log("ScenesCfg.json中字段[GrowthMushroomsList]位置不对应"); return false; }


		while(true)
		{
			vecLine = HS_ByteRead.readCsvLine( strContent, ref contentOffset );
			if((int)vecLine.Count == 0 )
				break;
			if((int)vecLine.Count != (int)5)
			{
				return false;
			}
			ScenesCfgElement member = new ScenesCfgElement();
			member.Id = Convert.ToInt32(vecLine[0]);
			member.BigImageName = vecLine[1];
			member.SmallImageName = vecLine[2];
			member.BuyPrice = Convert.ToInt32(vecLine[3]);
			member.GrowthMushroomsList = vecLine[4];

			member.IsValidate = true;
			_VecAllElements.Add(member);
			_MapElements[member.Id] = member;
		}
		return true;
	}

	public bool LoadJson(string strContent)
	{
	    JsonData jsonData = JsonMapper.ToObject(strContent);
	    for (int i = 0; i < jsonData.Count; ++i)
	    {
	    	JsonData jd = jsonData[i];
	    	if(jd.Keys.Count != 5)
            {
                Debug.Log("ScenesCfg.json中列数量与生成的代码不匹配!");
                return false;
            }
            
	       ScenesCfgElement member = new ScenesCfgElement();
			member.Id = (int)jd["Id"];
			member.BigImageName = (string)jd["BigImageName"];
			member.SmallImageName = (string)jd["SmallImageName"];
			member.BuyPrice = (int)jd["BuyPrice"];
			member.GrowthMushroomsList = (string)jd["GrowthMushroomsList"];


	        member.IsValidate = true;
            _VecAllElements.Add(member);
            _MapElements[member.Id] = member;
	    }
	    return true;
	}
};
