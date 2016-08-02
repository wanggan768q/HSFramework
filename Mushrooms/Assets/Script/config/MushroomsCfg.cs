using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using HS.IO;
using LitJson;


public class MushroomsCfgElement
{
	/// <summary>
	/// 蘑菇ID
	/// </summary>
	public int Id;

	/// <summary>
	/// 动画名字，帧动画
	/// </summary>
	public string AnimationName;

	/// <summary>
	/// 成熟时间(单位秒)
	/// </summary>
	public int RipeTime;

	/// <summary>
	/// 出售价格
	/// </summary>
	public int SalePrice;

	/// <summary>
	/// 测试bool
	/// </summary>
	public bool isOk;


	public bool IsValidate = false;
	public MushroomsCfgElement()
	{
		Id = 0;
		IsValidate = false;

	}
};


public class MushroomsCfgTable
{

	private MushroomsCfgTable()
	{
		_MapElements = new Dictionary<int, MushroomsCfgElement>();
		_EmptyItem = new MushroomsCfgElement();
		_VecAllElements = new List<MushroomsCfgElement>();
	}
	private Dictionary<int, MushroomsCfgElement> _MapElements = null;
	private List<MushroomsCfgElement>	_VecAllElements = null;
	private MushroomsCfgElement _EmptyItem = null;
	private static MushroomsCfgTable _SInstance = null;

	public static MushroomsCfgTable Instance
	{
		get
		{
			if( _SInstance != null )
				return _SInstance;	
			_SInstance = new MushroomsCfgTable();
			return _SInstance;
		}
	}

	public MushroomsCfgElement GetElement(int key)
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

  public List<MushroomsCfgElement> GetAllElement(Predicate<MushroomsCfgElement> matchCB = null)
	{
        if( matchCB==null || _VecAllElements.Count == 0)
            return _VecAllElements;
        return _VecAllElements.FindAll(matchCB);
	}

	public bool Load()
	{
		
		string strTableContent = "";
		if(HS_ByteRead.ReadCsvFile("MushroomsCfg.json", out strTableContent ) )
			return LoadCsv( strTableContent );
		byte[] binTableContent = null;
		if( !HS_ByteRead.ReadBinFile("MushroomsCfg.bin", out binTableContent ) )
		{
			Debug.Log("配置文件[MushroomsCfg.bin]未找到");
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
			Debug.Log("MushroomsCfg.json中列数量与生成的代码不匹配!");
			return false;
		}
		if(vecLine[0]!="Id") { Debug.Log("MushroomsCfg.json中字段[Id]位置不对应"); return false; }
		if(vecLine[1]!="AnimationName") { Debug.Log("MushroomsCfg.json中字段[AnimationName]位置不对应"); return false; }
		if(vecLine[2]!="RipeTime") { Debug.Log("MushroomsCfg.json中字段[RipeTime]位置不对应"); return false; }
		if(vecLine[3]!="SalePrice") { Debug.Log("MushroomsCfg.json中字段[SalePrice]位置不对应"); return false; }
		if(vecLine[4]!="isOk") { Debug.Log("MushroomsCfg.json中字段[isOk]位置不对应"); return false; }


		for(int i=0; i<nRow; i++)
		{
			MushroomsCfgElement member = new MushroomsCfgElement();
			readPos += HS_ByteRead.ReadInt32Variant(binContent, readPos, out member.Id );
			readPos += HS_ByteRead.ReadString(binContent, readPos, out member.AnimationName );
			readPos += HS_ByteRead.ReadInt32Variant(binContent, readPos, out member.RipeTime );
			readPos += HS_ByteRead.ReadInt32Variant(binContent, readPos, out member.SalePrice );
			readPos += HS_ByteRead.ReadBool(binContent, readPos, out member.isOk );

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
			Debug.Log("MushroomsCfg.json中列数量与生成的代码不匹配!");
			return false;
		}
		if(vecLine[0]!="Id") { Debug.Log("MushroomsCfg.json中字段[Id]位置不对应"); return false; }
		if(vecLine[1]!="AnimationName") { Debug.Log("MushroomsCfg.json中字段[AnimationName]位置不对应"); return false; }
		if(vecLine[2]!="RipeTime") { Debug.Log("MushroomsCfg.json中字段[RipeTime]位置不对应"); return false; }
		if(vecLine[3]!="SalePrice") { Debug.Log("MushroomsCfg.json中字段[SalePrice]位置不对应"); return false; }
		if(vecLine[4]!="isOk") { Debug.Log("MushroomsCfg.json中字段[isOk]位置不对应"); return false; }


		while(true)
		{
			vecLine = HS_ByteRead.readCsvLine( strContent, ref contentOffset );
			if((int)vecLine.Count == 0 )
				break;
			if((int)vecLine.Count != (int)5)
			{
				return false;
			}
			MushroomsCfgElement member = new MushroomsCfgElement();
			member.Id = Convert.ToInt32(vecLine[0]);
			member.AnimationName = vecLine[1];
			member.RipeTime = Convert.ToInt32(vecLine[2]);
			member.SalePrice = Convert.ToInt32(vecLine[3]);
			member.isOk= Convert.ToBoolean(vecLine[4]);

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
                Debug.Log("MushroomsCfg.json中列数量与生成的代码不匹配!");
                return false;
            }
            
	        MushroomsCfgElement member = new MushroomsCfgElement();
			member.Id = (int)jd["Id"];
			member.AnimationName = (string)jd["AnimationName"];
			member.RipeTime = (int)jd["RipeTime"];
			member.SalePrice = (int)jd["SalePrice"];
			member.isOk = (bool)jd["isOk"];


	        member.IsValidate = true;
            _VecAllElements.Add(member);
            _MapElements[member.Id] = member;
	    }
	    return true;
	}
};
