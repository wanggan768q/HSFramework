using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using HS.UI;
using System;
using UnityEngine.UI;

namespace HS.Lua
{
    public class HS_LuaBehaviour : HS_ViewBase
    {
        private LuaTable _LuaTable;

        public void Attach(LuaTable luaScript)
        {
            this._LuaTable = luaScript;
        }

        override protected void OnCreated()
        {
            base.OnCreated();
            //HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnCreated");
        }

        override protected void OnStarted()
        {
            base.OnStarted();
            HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnStarted");
        }

        override protected void OnOpened()
        {
            base.OnOpened();
            HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnOpened");
        }

        override protected void OnClosed()
        {
            base.OnClosed();
            HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnClosed");
        }


        override protected void OnButtonClick(GameObject go, Button button)
        {
            base.OnButtonClick(go, button);
            //HS_CallLuaFace.CallMemberOnceWithUnity(_LuaTable, "OnButtonClick",go);
            LuaFunction func = _LuaTable.GetLuaFunction("OnButtonClick");
            int oldTop = _LuaTable.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(go);
            func.Push(button);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        override protected void OnValueChange(GameObject go, float floatValue, int intValue, bool boolValue, string stringValue)
        {
            base.OnValueChange(go, floatValue, intValue, boolValue, stringValue);
            //HS_CallLuaFace.CallMemberOnceWithUnity(_LuaTable, "OnValueChange", go, floatValue, intValue, boolValue, stringValue);
            LuaFunction func = _LuaTable.GetLuaFunction("OnValueChange");
            int oldTop = _LuaTable.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(go);
            func.Push(floatValue);
            func.Push(intValue);
            func.Push(boolValue);
            func.Push(stringValue);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        override protected void OnListViewInit(HS_ListViewBase listView, HS_UIListViewCell cell, object data)
        {
            base.OnListViewInit(listView, cell, data);
            //HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnListViewInit", listView, cell, data);
            LuaFunction func = _LuaTable.GetLuaFunction("OnListViewInit");
            int oldTop = _LuaTable.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(listView);
            func.Push(cell);
            func.Push(data);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        override protected void OnListViewClick(HS_UIListView listView, HS_UIListViewCell cell, GameObject target)
        {
            base.OnListViewClick(listView, cell, target);
            //HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnListViewClick", listView, cell, target);
            LuaFunction func = _LuaTable.GetLuaFunction("OnListViewClick");
            int oldTop = _LuaTable.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(listView);
            func.Push(cell);
            func.Push(target);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        override protected void OnListViewSelected(HS_UIListView listView, int dataIndex)
        {
            base.OnListViewSelected(listView, dataIndex);
            //HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnListViewSelected", listView, dataIndex);
            LuaFunction func = _LuaTable.GetLuaFunction("OnListViewSelected");
            int oldTop = _LuaTable.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(listView);
            func.Push(dataIndex);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        override protected void OnListViewDeselected(HS_UIListView listView, int dataIndex)
        {
            base.OnListViewDeselected(listView, dataIndex);
            //HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnListViewDeselected", listView, dataIndex);
            LuaFunction func = _LuaTable.GetLuaFunction("OnListViewDeselected");
            int oldTop = _LuaTable.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(listView);
            func.Push(dataIndex);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        override protected void OnCellCreated(HS_ListViewBase listView)
        {
            base.OnCellCreated(listView);
            //HS_CallLuaFace.CallMemberOnce(_LuaTable, "OnCellCreated", listView);
            LuaFunction func = _LuaTable.GetLuaFunction("OnCellCreated");
            int oldTop = _LuaTable.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(listView);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        override internal GameObject GetViewPrefab()
        {
            object[] re = HS_CallLuaFace.CallMemberOnce(_LuaTable, "GetViewPrefab",null);
            if (re == null || re.Length == 0)
            {
                Debugger.LogError("GetViewPrefab");
                return null;
            }
            return re[0] as GameObject;
        }

        override internal string GetViewPrefabName()
        {
            object[] re = HS_CallLuaFace.CallMemberOnce(_LuaTable, "GetViewPrefabName", null);
            if (re == null || re.Length == 0)
            {
                Debugger.LogError("GetViewPrefabName");
                return null;
            }
            return re[0] as string;
        }

        //         override public void RegisterButtonClickEvent(Button btn)
        //         {
        //             base.RegisterButtonClickEvent(btn);
        //         }
        // 
        //         override public void RegisterSliderEvent(Slider slider)
        //         {
        //             base.RegisterSliderEvent(slider);
        //         }
        // 
        //         override public void RegisterToggleEvent(Toggle toggle)
        //         {
        //             base.RegisterToggleEvent(toggle);
        //         }
        // 
        //         override public void RegisterDropDownEvent(Dropdown dropdown)
        //         {
        //             base.RegisterDropDownEvent(dropdown);
        //         }
        // 
        //         override public void RegisterInputFieldEvent(InputField inputField)
        //         {
        //             base.RegisterInputFieldEvent(inputField);
        //            }
    }
}

