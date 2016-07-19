using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using HS.Base;
using System;


namespace HS.Net.TCP
{
    public class HS_TelnetManager : HS.Base.HS_SingletonGameObject<HS_TelnetManager>
    {

        string _Ip = null;
        float _IntervalTime = 1;
        int _Prot = -1;
        public bool Unimpeded { get; private set; }

        public HS_TelnetManager()
        {
            Unimpeded = true;
        }

        public void OnStart(string ip, int prot,float intervalTime)
        {
            _Ip = ip;
            _Prot = prot;
            _IntervalTime = intervalTime;
#if UNITY_WP8
            throw new NotImplementedException("WP8 Not supported");
#else
            OnRestart();
#endif
        }

        public void OnRestart()
        {
            if (_Ip == null)
            {
                throw new System.NullReferenceException("Never first start");
            }
            Unimpeded = true;
            StartCoroutine("T");
        }

        IEnumerator T()
        {
            Telnet telnet = new Telnet(_Ip, _Prot);
            telnet.Connect();
            yield return new WaitForSeconds(1);
            if (!telnet.Unimpeded)
            {
                D.LogError("连接服务器异常: " + _Ip + ":" + _Prot);
                Unimpeded = false;
                OnStop();
                yield break;
            }
            Unimpeded = true;
            telnet = null;
            yield return new WaitForSeconds(_IntervalTime);
            StartCoroutine("T");
        }

        public void OnStop()
        {
            StopCoroutine("T");
        }

        void OnDestroy()
        {
            OnStop();
        }
        
    }

    internal class Telnet
    {
        Socket _Socket = null;
        string _Ip = null;
        int _Prot = -1;

        public bool Unimpeded { get; private set; }

        public Telnet(string ip, int prot)
        {
            _Ip = ip;
            _Prot = prot;
        }

        /// <summary>        
        /// 启动socket 进行telnet操作        
        /// </summary>       
        public void  Connect()
        {
#if !UNITY_WP8
            try
            {
                // Try a blocking connection to the server
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _Socket.Connect(_Ip, _Prot);
                Unimpeded = _Socket.Poll(HS_Define.CONNECT_TIMEOUT, SelectMode.SelectWrite);
                _Socket.Close();
            }
            catch (Exception)
            {
                Unimpeded = false;
            }
#endif
        }

    }
}

