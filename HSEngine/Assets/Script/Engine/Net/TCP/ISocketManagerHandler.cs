using UnityEngine;
using System.Collections;

namespace HS.Net.TCP
{
    public interface ISocketManagerHandler
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        void Handler(HS_SocketStream socket,HS_MsgWrapper msg);
    }
}
