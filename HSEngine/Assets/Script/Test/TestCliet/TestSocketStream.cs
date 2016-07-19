using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HS.Net.TCP;
using System;

public class TestSocketStream : MonoBehaviour, ISocketStreamHandler, ISocketManagerHandler
{

    private string _IP_LoginServer = "10.2.9.223";
    private int _Port_LoginServer = 2443;


    HS_SocketStream _Socket = null;

    #region System Message
    void Awake()
    {
        _log = "Awake";
        HS_SocketManager.GetInstance().ISocketManagerHandler = this;
        _Socket = new HS_SocketStream();
        _Socket.Connect(_IP_LoginServer, _Port_LoginServer, HS_SocketStream.HS_ConnectType.HTTP, this);
        HS_SocketManager.GetInstance().AddSock(_Socket);
    }


    #endregion

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            //HS_SocketStream s = CreateLoginServer();
            //_Socket.NativeSocket.SendTimeout = 1;
            byte[] buf = new byte[] { 10, 6, 8, 1, 16, 1, 24, 0, 18, 24, 10, 22, 10, 9, 103, 122, 108, 49, 50, 51, 52, 53, 54, 18, 9, 103, 122, 108, 49, 50, 51, 52, 53, 54 };
            
            HS_MsgWrapper msg = new HS_MsgWrapper(1, 1, buf);
            _Socket.Send(msg);
        }
        
    }


    void ISocketStreamHandler.OnClose(bool isSucceed, string ip, int prot)
    {
        _log = string.Format("关闭连接状态:  {0}:{1}  {2}", ip, prot, (isSucceed ? "成功" : "失败"));
        D.Log(_log);
    }

    void ISocketStreamHandler.OnConnectCompleted(bool isSucceed, string ip, int prot)
    {
        _log = string.Format("连接状态:  {0}:{1}  {2}", ip, prot, (isSucceed ? "成功" : "失败"));
        D.Log(_log);
    }

    void ISocketStreamHandler.OnConnectFinished(string ip, int prot)
    {
        D.Log(string.Format("连接完成:  {0}:{1}", ip, prot));
    }

    bool isRevc = true;
    HS_MsgWrapper ISocketStreamHandler.OnHandleRecv(byte[] data, int dateSize)
    {
        //D.Log("收到数据 <" + dateSize + ">");

        HS_MsgWrapper msg = new HS_MsgWrapper(1, 1, data);

        if (isRevc)
        {
            isRevc = false;
            foreach(byte b in data)
            {
                _Recv += b + ",";
            }
        }

        return msg;
    }

   

    void ISocketManagerHandler.Handler(HS_SocketStream socket, HS_MsgWrapper msg)
    {
        
    }


    void ISocketStreamHandler.OnDisconnect(HS_SocketStream socket)
    {
        D.Log("连接断开");
    }


    string _log = "";
    string _Recv = "";
    void OnGUI()
    {
        if (GUILayout.Button("------------发送 ------", GUILayout.Height(200)))
        {
            byte[] buf = new byte[] { 10, 6, 8, 1, 16, 1, 24, 0, 18, 24, 10, 22, 10, 9, 103, 122, 108, 49, 50, 51, 52, 53, 54, 18, 9, 103, 122, 108, 49, 50, 51, 52, 53, 54 };

            HS_MsgWrapper msg = new HS_MsgWrapper(1, 1, buf);
            _Socket.Send(msg);
        }  
        GUILayout.Label(_log);
        GUILayout.Label(_Recv);
    }





}
