using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HS.Net.TCP
{
    public class HS_PingManager : HS.Base.HS_SingletonGameObject<HS_PingManager>
    {

        string _Ip = null;
        float _IntervalTime = 1;

        /// <summary>
        /// 网络是否畅通
        /// </summary>
        public bool Unimpeded { get; private set; }

        public HS_PingManager()
        {
            Unimpeded = true;
        }
        
        /// <summary>
        /// 启动 Ping
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="intervalTime"></param>
        public void OnStart(string ip, float intervalTime)
        {
            _Ip = ip;
            _IntervalTime = intervalTime;
            OnRestart();
        }

        public void OnRestart()
        {
            if (_Ip == null)
            {
                throw new System.NullReferenceException("Never first start");
            }
            Unimpeded = true;
            StartCoroutine("P");
        }

        IEnumerator P()
        {
            //@ WP8 临时修改一下
            yield break;
            /*
            Ping ping = new Ping(_Ip);
            yield return new WaitForSeconds(1);
            if (ping.isDone && ping.time == -1)
            {
                D.LogError("网络中断");
                Unimpeded = false;
                OnStop();
                ping.DestroyPing();
                yield break;
            }
            Unimpeded = true;
            ping.DestroyPing();
            yield return new WaitForSeconds(_IntervalTime);
            StartCoroutine("P");
            */
        }


        public void OnStop()
        {
            StopCoroutine("P");
        }

        void OnDestroy()
        {
            OnStop();
        }

    }

}
