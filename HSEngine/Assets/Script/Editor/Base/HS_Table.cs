using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HS.Edit.Base
{
    /// <summary>
    /// 数据表
    /// </summary>
    public class HS_Table<T,T1> : HS_EditorControlBase
    {
        public HS_Table(string name) : base(name)
        {

        }
        //         public List<DataRow> DataRows = new List<DataRow>(); //DataRow 类存放一行的所有数据 
        //         protected HS_Space ecSpace;
        //         protected Dictionary<string, int> columnName_Width = new Dictionary<string, int>(); // 列名和宽度  
        //                                                                                             //事件  
        //         public delegate void OnDeleteRowDel(int ID);
        //         public OnDeleteRowDel OnDeleteRow;
        // 
        //         //数据缓存. 针对每个LevelEnemyOneRow键一个缓存字典, int is LevelEnemyOneRow.ID,  string is  columnName  
        //         protected Dictionary<int, Dictionary<string, ValueCache>> inputDataCache = new Dictionary<int, Dictionary<string, ValueCache>>();
        // 
        //         protected class ValueCache
        //         {
        //             public Type Type;
        //             public object ObjValue;
        //             public ValueType ValueType = ValueType.Assigned;
        // 
        //             public ValueCache(object objValue, Type type)
        //             {
        //                 this.ObjValue = objValue;
        //                 this.Type = type;
        //             }
        //         }
        // 
        //         /// <summary>  
        //         /// 值冲突类型  
        //         /// </summary>  
        //         protected enum ValueType
        //         {
        //             Assigned, //正常值,已经赋值了的  
        //             BeModify, //被修改了还没有赋值的  
        //             Error, //错误数据, 还没有赋值  
        //         }
        // 

        // 
        //         protected Color GetColor(ValueType v)
        //         {
        //             if (v == ValueType.Error)
        //                 return Color.red;
        //             if (v == ValueType.BeModify)
        //                 return Color.yellow;
        // 
        //             return Color.green;
        //         }
        // 
        //         protected void DisplayText<T>(int rowID, T _value, string colName)
        //         {
        //             //找出缓存中对应的项  
        //             ValueCache vc;
        //             if (inputDataCache[rowID].ContainsKey(colName) == true)
        //                 vc = inputDataCache[rowID][colName];
        //             else
        //                 vc = new ValueCache(_value, typeof(T));
        // 
        //             GUI.color = GetColor(vc.ValueType);
        // 
        //             string temp = vc.ObjValue.ToString();
        //             vc.ObjValue = GUILayout.TextField(vc.ObjValue.ToString(), GUILayout.Width(columnName_Width[colName]));
        // 
        //             if (vc.ValueType == ValueType.Assigned && temp != vc.ObjValue.ToString())
        //                 vc.ValueType = ValueType.BeModify;
        // 
        //             //更新缓存  
        //             if (inputDataCache[rowID].ContainsKey(colName) == true)
        //                 inputDataCache[rowID][colName] = vc;
        //             else
        //                 inputDataCache[rowID].Add(colName, vc);
        // 
        //             GUI.color = Color.white;
        //         }
        // 
        //         protected void DisplayText<T>(T _value, string colName)
        //         {
        //             GUILayout.TextField(_value.ToString(), GUILayout.Width(columnName_Width[colName]));
        //         }
        // 
        //         //_value为要赋值的真正数据  
        //         protected static void ConvertData<T>(ValueCache vc, ref T _value, float min = float.MinValue, float max = float.MaxValue)
        //         {
        //             try
        //             {
        //                 //限制必须是数字  
        //                 T temp = (T)Convert.ChangeType(vc.ObjValue, typeof(T));
        //                 float tryFloat = 0;
        //                 bool isFloat = float.TryParse(temp.ToString(), out tryFloat);
        //                 //限制数字的大小, 超过限制就走 catch  
        //                 if (isFloat)
        //                 {
        //                     if(tryFloat > max || tryFloat < min)
        //                     {
        //                         throw new Exception();
        //                     }
        //                 }
        //                 _value = temp;
        //                 vc.ValueType = ValueType.Assigned;
        //             }
        //             catch
        //             {
        //                 vc.ValueType = ValueType.Error;
        //             }
        //         }
        public override void OnGUIUpdate()
        {

        }
    }
}