using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System;
using System.IO;
using System.Xml.Serialization;
using HS.Base;
using HS.Net.TCP;

namespace HS.Net.TCP
{
    public class HS_SocketManager : HS_SingletonGameObject<HS_SocketManager>
    {
        private Thread _Thread = null;

        private bool _IsRun = false;

        private Dictionary<Socket, HS_SocketStream> _SocketMap = new Dictionary<Socket, HS_SocketStream>();

        private Queue<SocketMsg> _AllMessage = new Queue<SocketMsg>();

        private ISocketManagerHandler _ISocketManagerHandler = null;
        public HS.Net.TCP.ISocketManagerHandler ISocketManagerHandler
        {
            set
            {
                _ISocketManagerHandler = value;
            }
        }

        struct SocketMsg
        {
            public HS_SocketStream socket;
            public HS_MsgWrapper msg;
        }

        void Awake()
        {
            D.Log("创建网络管理器");
        }

        void Start()
        {
            _IsRun = true;
            _Thread = new Thread(new ThreadStart(this.Run));
            _Thread.Start();
        }

        void OnDestroy()
        {
            //Instance = null;
            this.Shutdown();
        }

        public void Shutdown()
        {
            _IsRun = false;
        }

        public void AddSock(HS_SocketStream socket)
        {
            if (socket == null || socket.NativeSocket == null) return;

            RemoveSock(socket);
            lock (((ICollection)_SocketMap).SyncRoot)
            {
                _SocketMap.Add(socket.NativeSocket, socket);
            }
        }

        public void RemoveSock(HS_SocketStream socket)
        {
            if (socket == null || socket.NativeSocket == null) return;

            lock (((ICollection)_SocketMap).SyncRoot)
            {
                if (_SocketMap.ContainsKey(socket.NativeSocket))
                    _SocketMap.Remove(socket.NativeSocket);
            }
        }

        void Update()
        {
            if (_ISocketManagerHandler != null)
            {
                if (_AllMessage.Count > 0)
                {
                    lock (((ICollection)_AllMessage).SyncRoot)
                    {

                        SocketMsg msg = _AllMessage.Dequeue();
                        _ISocketManagerHandler.Handler(msg.socket, msg.msg);
                    }
                }
            }
        }

        void OnApplicationQuit()
        {
            if(S_Instance != null)
            {
                Shutdown();
                Destory();
            }
        }

        private void Run()
        {
            D.Log("网络管理器 -> 启动线程");
            while (_IsRun)
            {
                try
                {
                    long interval = DateTime.Now.Ticks;

                    lock (((ICollection)_SocketMap).SyncRoot)
                    {
                        foreach (HS_SocketStream s in _SocketMap.Values)
                        {
                            if (s.NativeSocket == null)
                            {
                                continue;
                            }

                            if (s.ConnectFinished)
                            {
                                s.OnRecv();
                                s.OnSend();
                                HS_MsgWrapper msg = s.GetMsg();
                                if (msg != null)
                                {
                                    lock (((ICollection)_AllMessage).SyncRoot)
                                    {
                                        SocketMsg socketMsg;
                                        socketMsg.socket = s;
                                        socketMsg.msg = msg;
                                        _AllMessage.Enqueue(socketMsg);
                                    }
                                }
                            }
                            else
                            {
                                string log = string.Format("{0} 正在连接中....", s.Address);
                                D.Log(log);
                            }
                        }
                    }


                    interval = System.DateTime.Now.Ticks - interval;
                    int sleep = HS_Define.SOCK_THREAD_TIMER - (int)(interval / 10000);
                    if (sleep > 0)
                    {
                        Thread.Sleep(1);
                    }
                }
                catch (System.Exception ex)
                {
                    if (!(ex is System.Threading.ThreadAbortException))
                    {
                        if (_IsRun)
                        {
                            D.LogException(ex);
                        }
                    }
                }
            }
        }
    }
}


