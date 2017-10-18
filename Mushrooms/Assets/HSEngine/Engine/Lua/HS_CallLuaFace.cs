using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HS.Lua
{
    public class HS_CallLuaFace
    {
        // 调用对象内函数
        public static void CallOnce(LuaTable self, string funcName)
        {
            LuaFunction func = self.GetLuaFunction(funcName);
            func.Call();
            func.Dispose(); 
        }

        // 调用复合 回产生 gc alloc
        [LuaInterface.NoToLua]
        public static object[] CallOnce(LuaTable self, string funcName, params object[] args)
        {
            LuaFunction func = self.GetLuaFunction(funcName);
            object[] ret = func.LazyCall(args);
            func.Dispose();
            return ret;
        }

        // 调用对象成员函数
        public static void CallMemberOnce(LuaTable self, string funcName)
        {
            LuaFunction func = self.GetLuaFunction(funcName);
            if (func == null)
            {
                return;
            }

            func.BeginPCall();
            func.Push(self);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        // 调用多参数
        public static object[] CallMemberOnce(LuaTable self, string funcName, params object[] args)
        {
            LuaFunction func = self.GetLuaFunction(funcName);
            int oldTop = self.GetLuaState().LuaGetTop();
            func.BeginPCall();
            func.Push(self);
            func.PushArgs(args);
            func.PCall();
            object[] rets = func.GetLuaState().CheckObjects(oldTop);
            func.EndPCall();

            func.Dispose();
            return rets;
        }

        public static void CallMemberOnceWithUnity(LuaTable self, string funcName, Object obj)
        {
            LuaFunction func = self.GetLuaFunction(funcName);
            int oldTop = self.GetLuaState().LuaGetTop();
            func.BeginPCall();
            //func.Push(self);
            func.Push(obj);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        public static void CallMemberOnceWithUnity(LuaTable self, string funcName, Object obj, params object[] args)
        {
            LuaFunction func = self.GetLuaFunction(funcName);
            int oldTop = self.GetLuaState().LuaGetTop();
            func.BeginPCall();
            //func.Push(self);
            func.Push(obj);
            func.Push(args);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }

        // 调用对象内函数
        public static void CallOnce(LuaState lua, string funcName)
        {
            LuaFunction func = lua.GetFunction(funcName);
            func.Call();
            func.Dispose();
        }

        // 调用复合 
        [LuaInterface.NoToLua]
        public static object[] CallOnce(LuaState lua, string funcName, params object[] args)
        {
            LuaFunction func = lua.GetFunction(funcName);
            object[] ret = func.LazyCall(args);
            func.Dispose();
            return ret;
        }
    }
}

