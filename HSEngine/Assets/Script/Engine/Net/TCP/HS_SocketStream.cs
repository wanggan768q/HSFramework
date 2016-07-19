#define UNITY_WP8
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using HS.Base;
using System.Timers;
using System.Threading;

namespace HS.Net.TCP
{
    public class HS_SocketStream
    {
        private Socket _Socket = null;
        private string _ErrorText = string.Empty;
        private bool _ConnectFinished = false;
        private int _SockErrorCode = 0;
        private volatile int _SocketPendingSend = 0;
        private string IP { get; set; }
        private int Prot { get; set; }
        public bool EnableSocketTimeTout { get; set; }

        public string Address
        {
            get
            {
                if (_Socket == null)
                {
                    throw new NullReferenceException("连接还没有建立");
                }
                string s = string.Empty;

                s = string.Format("{0}:{1} - {2}/{3} - SOT:{4} / ROT:{5}", IP, Prot, _ConnectType.ToString(), SocketMode.ToString(), HS_Define.SEND_TIMEOUT, HS_Define.RECV_TIMEOUT);

                return s;
            }
        }
        
        private static ManualResetEvent _ConnectSignal;

        private const int SECURITYCODE = -1860168940;
        private ISocketStreamHandler IHandler { get; set; }

        private Queue<HS_MsgWrapper> _SendQueue = new Queue<HS_MsgWrapper>();
        public Queue<HS_MsgWrapper> _RecvQueue = new Queue<HS_MsgWrapper>();
        private Queue<TempBuffer> _RecvdQueue = new Queue<TempBuffer>();


        private int _RecvSize = 0;
        private byte[] _RecvBuffer = new byte[HS_Define.RECV_PACKET_MAX_LEN];
        private byte[] _SendBuffer = new byte[HS_Define.SEND_PACKET_MAX_LEN];



        public enum HS_ConnectType
        {
            HTTP = 0x01,
            TCP,
        }

        /// <summary>
        /// 网络模式
        /// </summary>
        public enum SocketStreamMode
        {
            Async = 0x01,
            Sync,
        }

        public static SocketStreamMode SocketMode { get; private set; }
        private HS_ConnectType _ConnectType = HS_ConnectType.TCP;
        public HS.Net.TCP.HS_SocketStream.HS_ConnectType ConnectType
        {
            get { return _ConnectType; }
        }

        public bool ConnectFinished
        {
            get { return _ConnectFinished; }
            set { _ConnectFinished = value; }
        }

        private byte[] _C2JSecurityCodeBuf = null;
        /// <summary>
        /// 安全码BUF
        /// </summary>
        public byte[] C2JSecurityCodeBuf
        {
            get
            {
                if (_C2JSecurityCodeBuf == null)
                {
                    _C2JSecurityCodeBuf = Int2bytes(SECURITYCODE);
                }
                return _C2JSecurityCodeBuf;
            }
        }




        /// <summary>
        /// 当前套接字
        /// </summary>
        public Socket NativeSocket
        {
            get
            {
                return _Socket;
            }
        }

        /// <summary>
        /// 当前网络连接状态
        /// </summary>
        public bool ConnectedState
        {
            get
            {
                if (_Socket != null && _Socket.Connected)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get
            {
                return _ErrorText;
            }
        }


        class TempBuffer
        {
            public byte[] Buffer
            {
                get;
                set;
            }

            public int Size
            {
                get;
                set;
            }
        }



        public void Connect(string ip, int port, HS_ConnectType type, ISocketStreamHandler handler)
        {
            if (handler == null)
            {
                D.LogError("handler Can't be NULL");
                return;
            }
            //SocketMode = Application.platform == RuntimePlatform.IPhonePlayer ? SocketStreamMode.Sync : SocketStreamMode.Async;

            //SocketMode = SocketStreamMode.Sync;
            this.IHandler = handler;
            _ConnectType = type;
            _SockErrorCode = 0;
            _ErrorText = string.Empty;
            _ConnectFinished = false;

            IP = ip;
            Prot = port;

            this.Reconnection();
        }

        void TimeoutCheck(int milliseconds,System.Action action)
        {
            if(_ConnectSignal != null)
            {
                bool b = _ConnectSignal.WaitOne(milliseconds);
                if(!b)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        void Reconnection()
        {
            try
            {
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress address = IPAddress.Parse(IP);
                IPEndPoint hostEntry = new IPEndPoint(address, Prot);
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = hostEntry;


                _ConnectSignal = new ManualResetEvent(false);
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);
                _ConnectSignal.Reset();
                _Socket.ConnectAsync(socketEventArg);

               
                TimeoutCheck(HS_Define.CONNECT_TIMEOUT, () =>
                {
                    D.LogErrorForce("Connect Timeout");
                    this.OnConnectFinished();
                });
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode != 10035)
                {
                    _Socket.Close();
                    _SockErrorCode = ex.ErrorCode;
                    _ErrorText = ex.Message;
                    D.LogErrorForce(ex.ToString());
                }
                this.OnConnectFinished();
            }
        }

        private void OnConnectFinished()
        {
            _ConnectFinished = true;
            this.IHandler.OnConnectFinished(IP, Prot);
        }


        #region OnConnectCompleted

        private void OnConnectCompleted(object s, SocketAsyncEventArgs e)
        {
            _ConnectSignal.Set();
            if (this._Socket.Connected)
            {
                this.IHandler.OnConnectCompleted(true, IP, Prot);
                this.OnConnectFinished();
                this.StartReceive();
            }
            else
            {
                D.LogErrorForce("Connect Failed.");
                this.IHandler.OnConnectCompleted(false, IP, Prot);
                this.OnConnectFinished();
            }
        }
        #endregion OnConnectCompleted


        #region StartReceive/OnReceiveComplete

        private void OnReceiveComplete(object s, SocketAsyncEventArgs e)
        {
            string message = e.SocketError.ToString();
            bool success = (e.SocketError == SocketError.Success);
            int recvdSize = e.BytesTransferred;

            byte[] buf = e.Buffer;
            if (recvdSize > 0)
            {
                lock (((ICollection)_RecvdQueue).SyncRoot)
                {
                    TempBuffer tmpbuf = new TempBuffer();
                    tmpbuf.Buffer = buf;
                    tmpbuf.Size = recvdSize;
                    _RecvdQueue.Enqueue(tmpbuf);
                }
                this.StartReceive();
            }
        }

        private void StartReceive()
        {
            if (this.NativeSocket.Connected)
            {
                byte[] readyRecvOnceBuf = new byte[HS_Define.RECV_ONECE_LEN];
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = _Socket.RemoteEndPoint;
                socketEventArg.SetBuffer(readyRecvOnceBuf, 0, readyRecvOnceBuf.Length);
                socketEventArg.UserToken = null;
                socketEventArg.AcceptSocket = null;
                socketEventArg.Completed += OnReceiveComplete;

                if (!_Socket.ReceiveAsync(socketEventArg))
                {
                    OnReceiveComplete(this, socketEventArg);
                }
            }
        }

        #endregion StartReceive/OnReceiveComplete


        /// <summary>
        /// 只是关闭连接
        /// </summary>
        public void Close()
        {
            if (_Socket == null) return;
            try
            {
                if(ConnectedState)
                {
                    _Socket.Close();
                    _Socket = null;
                }
                this.IHandler.OnClose(true, IP, Prot);
            }
            catch (SocketException se)
            {
                D.LogErrorForce(se);
                this.IHandler.OnClose(false, IP, Prot);
            }

        }

        /// <summary>
        /// 关闭连接并且清楚数据
        /// </summary>
        public void Destroy()
        {
            Clear();
            if (_Socket != null)
            {
                Close();
            }
        }

        /// <summary>
        /// 清楚收发数据
        /// </summary>
        public void Clear()
        {
            _SockErrorCode = 0;

            lock (((ICollection)_SendQueue).SyncRoot)
            {
                _SendQueue.Clear();
            }
            lock (((ICollection)_RecvQueue).SyncRoot)
            {
                _RecvQueue.Clear();
            }

            lock (((ICollection)_RecvBuffer).SyncRoot)
            {
                _RecvSize = 0;
                Array.Clear(_RecvBuffer, 0, _RecvBuffer.Length);
            }
            lock (((ICollection)_SendBuffer).SyncRoot)
            {
                Array.Clear(_SendBuffer, 0, _SendBuffer.Length);
            }
        }

        public void OnDisconnect(HS_SocketStream socket)
        {
            if (IHandler != null)
            {
                IHandler.OnDisconnect(socket);
            }
        }

        public void Send(HS_MsgWrapper msg)
        {
            lock (((ICollection)_SendQueue).SyncRoot)
            {
                _SendQueue.Enqueue(msg);
            }
        }
        public HS_MsgWrapper GetMsg()
        {
            lock (((ICollection)_RecvQueue).SyncRoot)
            {
                if (_RecvQueue.Count <= 0) return null;

                return _RecvQueue.Dequeue();
            }
        }
        //get it will clear it
        public int GetSockErrorCode()
        {
            int c = _SockErrorCode;
            _SockErrorCode = 0;
            return c;
        }

        private void OnRecvAsync()
        {
            lock (((ICollection)_RecvdQueue).SyncRoot)
            {
                while (_RecvdQueue.Count > 0)
                {
                    TempBuffer tmpbuf = _RecvdQueue.Peek();
                    if (tmpbuf == null)
                        break;
                    lock (((ICollection)_RecvBuffer).SyncRoot)
                    {
                        if (tmpbuf.Size > (_RecvBuffer.Length - _RecvSize))
                            break;
                        tmpbuf = _RecvdQueue.Dequeue();
                        Array.Copy(tmpbuf.Buffer, 0, _RecvBuffer, _RecvSize, tmpbuf.Size);
                        _RecvSize += tmpbuf.Size;
                    }
                }
            }


            while (true)
            {
                if (_RecvSize < 8) break;
                //D.Log("接受数据大于 8 -> " + _RecvSize);

                lock (((ICollection)_RecvBuffer).SyncRoot)
                {
                    Array.Clear(_SecurityCodeBuf, 0, 4);
                    Array.Clear(_PackerSizeBuf, 0, 4);

                    Array.Copy(_RecvBuffer, 0, _SecurityCodeBuf, 0, 4);
                    int securityCode = Bytes2Int(_SecurityCodeBuf);

                    if (SECURITYCODE != securityCode)
                    {
                        this.Close();
                        D.LogError("Illegal links ..............." + SECURITYCODE + ":" + securityCode);
                        return;
                    }

                    Array.Copy(_RecvBuffer, 4, _PackerSizeBuf, 0, 4);
                    int packerSize = Bytes2Int(_PackerSizeBuf);

                    if (_RecvSize - 8 < packerSize)
                    {
                        D.Log("The data of the subcontract");
                        break;
                    }
                    byte[] value = new byte[packerSize];
                    Array.Copy(_RecvBuffer, 8, value, 0, packerSize);

                    HS_MsgWrapper msg = this.IHandler.OnHandleRecv(value, packerSize);

                    if (null == msg)
                    {
                        break;
                    }

                    lock (((ICollection)_RecvQueue).SyncRoot)
                    {
                        _RecvQueue.Enqueue(msg);
                    }

                    lock (((ICollection)_RecvBuffer).SyncRoot)
                    {
                        //_RecvSize -= len;
                        _RecvSize -= packerSize + 8;
                        if (_RecvSize > 0)
                        {
                            Array.Copy(_RecvBuffer, packerSize + 8, _RecvBuffer, 0, _RecvSize);
                        }
                    }

                    if (_ConnectType == HS_ConnectType.HTTP)
                    {
                        this.Close();
                    }
                }

            }
        }


        private byte[] _SecurityCodeBuf = new byte[4];
        private byte[] _PackerSizeBuf = new byte[4];

        public virtual void OnRecv()
        {
            if (_Socket == null) return;
            if (_Socket.Connected == false)
            {
                this.OnDisconnect(this);
                return;
            }
            OnRecvAsync();
        }

        // 转换为Java格式的字节数组
        public static byte[] Int2bytes(int n)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((n & 0xFF000000) >> 24);
            result[1] = (byte)((n & 0x00FF0000) >> 16);
            result[2] = (byte)((n & 0x0000FF00) >> 8);
            result[3] = (byte)((n & 0x000000FF));
            return result;
        }

        public static int Bytes2Int(byte[] n)
        {
            if (n == null || n.Length < 4)
            {
                return -1;
            }
            byte[] result = new byte[4];
            result[0] = n[3];
            result[1] = n[2];
            result[2] = n[1];
            result[3] = n[0];

            int v = BitConverter.ToInt32(result, 0);
            result = null;

            return v;
        }

        private void OnSendAsync()
        {
            int count = 0;
            int totalLen = 0;
            while (true)
            {
                HS_MsgWrapper msg = null;
                int len = 0;
                lock (((ICollection)_SendQueue).SyncRoot)
                {
                    if (_SendQueue.Count <= 0)
                        break;

                    msg = _SendQueue.Peek();

                    if (totalLen + len > HS_Define.SEND_PACKET_MAX_LEN)
                    {
                        D.LogError("To send data is greater than the specified number of bytes");
                        break;
                    }

                    msg = _SendQueue.Dequeue();
                }
                if (msg == null) break;

                lock (((ICollection)_SendBuffer).SyncRoot)
                {
                    //                     int val = IPAddress.HostToNetworkOrder(len);
                    //                     Array.Copy(BitConverter.GetBytes(val), 0, _SendBuffer,
                    //                                totalLen, 4);
                    //                     val = IPAddress.HostToNetworkOrder(msg.Opcode);
                    //                     Array.Copy(BitConverter.GetBytes(val), 0, _SendBuffer,
                    //                                totalLen + 4, 4);
                    //                     Array.Copy(msg.Value, 0, _SendBuffer, totalLen + 8, len - 8);

                    byte[] describeBuf = HS_Base.ConsolidationByteArray(C2JSecurityCodeBuf, Int2bytes(msg.Value.Length));
                    byte[] dataBuf = HS_Base.ConsolidationByteArray(describeBuf, msg.Value);
                    len = dataBuf.Length;
                    Array.Copy(dataBuf, 0, _SendBuffer, totalLen, len);
                }

                totalLen += len;
                count++;
            }
            if (totalLen > 0)
            {
                try
                {
                    byte[] buf = new byte[totalLen];
                    Array.Copy(_SendBuffer, buf, totalLen);

                    SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                    socketEventArg.RemoteEndPoint = _Socket.RemoteEndPoint;
                    socketEventArg.SetBuffer(buf, 0, totalLen);
                    socketEventArg.UserToken = null;
                    socketEventArg.AcceptSocket = null;
                    socketEventArg.Completed += OnSendCompleted;

                    if (!_Socket.SendAsync(socketEventArg))
                    {
                        OnSendCompleted(this, socketEventArg);
                    }


                    ++_SocketPendingSend;
                    D.Log("发送次数 + ");
                }
                catch (SocketException _e)
                {
                    this._Socket.Close();
                    D.LogError("SocketError:" + _e.Message);
                    //Logger.LogError("SocketError:" + _e.Message);
                }
                catch (ObjectDisposedException _e)
                {
                    this.OnDisconnect(this);
                    this._Socket.Close();
                    D.LogError("SocketError:" + _e.Message);
                }
            }
            //_Socket.Send(_SendBuffer, 0, totalLen, SocketFlags.None);
        }


        public virtual void OnSend()
        {
            if (_Socket == null) return;
            if (_Socket.Connected == false)
            {
                this.OnDisconnect(this);
                return;
            }
            if (_SocketPendingSend > 0) return;

            this.OnSendAsync();
        }

        private void OnSendCompleted(object s, SocketAsyncEventArgs e)
        {
            bool success = (e.SocketError == SocketError.Success);

            byte[] buf = e.Buffer;
            int bytes = e.BytesTransferred;
            D.Log("发送字节数 <" + bytes + ">");
            if (!success)
            {
                //Logger.LogError("OnSendCompleted Error:" + error.ToString());
                --_SocketPendingSend;
                D.LogError("OnSendCompleted Error:" + e.SocketError.ToString());
                this._Socket.Close();
                return;
            }

            if (bytes < buf.Length)
            {
                D.LogWarning("SocketWarning: Send:" + buf.Length.ToString() + ",Sent :" + bytes.ToString());

                byte[] tmpbuf = new byte[buf.Length - bytes];
                Array.Copy(buf, bytes, tmpbuf, 0, buf.Length - bytes);
                try
                {
                    SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                    socketEventArg.RemoteEndPoint = _Socket.RemoteEndPoint;
                    socketEventArg.SetBuffer(buf, 0, tmpbuf.Length);
                    socketEventArg.UserToken = null;
                    socketEventArg.AcceptSocket = null;
                    socketEventArg.Completed += OnSendCompleted;

                    if (!_Socket.SendAsync(socketEventArg))
                    {
                        OnSendCompleted(this, socketEventArg);
                    }

                }
                catch (SocketException _e)
                {
                    --_SocketPendingSend;
                    this._Socket.Close();
                    D.LogError("SocketError:" + _e.Message);
                }
            }
            else
            {
                --_SocketPendingSend;
            }

            /*
            if(bytes != total)
                Logger.LogError("OnSendCompleted Error," + bytes.ToString() + " But " + total.ToString());
            else
                Logger.Log("OnSendCompleted Sent " + bytes.ToString());
            */
        }
    }
}