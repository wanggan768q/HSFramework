using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Text;
using System.IO;

namespace HS.IO
{
   /* public class HS_DebugWindows : MonoBehaviour
    {

#if UNITY_STANDALONE_WIN
        Thread _DebugThr = null;
        System.Diagnostics.Process _Process;
        // Use this for initialization
        void Start()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                
                _DebugThr = new Thread(RunCMD);
                _DebugThr.Start();

                Application.RegisterLogCallbackThreaded(HandleLog);
            }
            
        }

        void OnDestroy()
        {
            if (_Process != null)
            {
                _Process.Close();
            }
        }


        //int index = 0;
        void Update()
        {
            //D.Log("INFO   " + index++);
            //D.LogError("Error");
        }

        List<string> outputList = new List<string>();
        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (_Process == null)
            {
                return;
            }
            
            //output.Append("\n[" + lineCount + "]: " + logString);
            //Console.WriteLine(logString);
            lock (outputList)
            {
                string log = string.Format("echo [{0}] [{1}]\t{2}", DateTime.Now.ToString(), type.ToString(), logString);

                //outputList.Add(log);
                _Process.StandardInput.WriteLine(log);
            }
        }

        void RunCMD()
        {
            //例Process
            _Process = new System.Diagnostics.Process();
            _Process.StartInfo.FileName = "cmd.exe";           //确定程序名
            _Process.StartInfo.Arguments = "/k " + "@echo off";    //确定程式命令行
            _Process.StartInfo.UseShellExecute = false;        //Shell的使用
            _Process.StartInfo.RedirectStandardInput = true;   //重定向输入
            //_Process.StartInfo.RedirectStandardOutput = true; //重定向输出
            //_Process.StartInfo.RedirectStandardError = true;   //重定向输出错误
            _Process.StartInfo.CreateNoWindow = false;          //设置置不显示示窗口

            //_Process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(p_OutputDataReceived);
            
            _Process.Start();
        } 
#endif
    }*/

}

