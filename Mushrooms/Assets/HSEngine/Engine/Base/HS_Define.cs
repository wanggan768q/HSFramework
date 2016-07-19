using UnityEngine;
using System.Collections;


namespace HS.Base
{
    public static class HS_Define
    {
        #region 网络数据包的大小
        public const int SEND_PACKET_MAX_LEN = 1024;
        public const int RECV_PACKET_MAX_LEN = 65536;
        public const int RECV_ONECE_LEN = 2048;
        #endregion

        #region 网络数据 连接/发送/接收 超时时间
        public const int CONNECT_TIMEOUT = 2000; //milli-seconds 毫秒
        public const int SEND_TIMEOUT = 5000;   //milli-seconds 毫秒
        public const int RECV_TIMEOUT = 5000;   //milli-seconds 毫秒
        #endregion

        #region 网络线程超时时间
        public const int SOCK_SELECT_TIMEOUT = 30000;  //micro-seconds 微妙
        public const int SOCK_THREAD_TIMER = 30;       //milli-seconds 毫秒
        #endregion
    }
}


