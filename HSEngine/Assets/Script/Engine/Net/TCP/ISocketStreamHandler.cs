using UnityEngine;
using System.Collections;


namespace HS.Net.TCP
{
    public interface ISocketStreamHandler
    {
        /// <summary>
        /// 连接成功通知
        /// </summary>
        /// <param name="isSucceed">是否成功</param>
        /// <param name="ip"></param>
        /// <param name="prot"></param>
        void OnConnectCompleted(bool isSucceed,string ip,int prot);

        /// <summary>
        /// 连接完成通知
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="prot"></param>
        void OnConnectFinished(string ip, int prot);

        /// <summary>
        /// 接受到数据处理
        /// </summary>
        /// <param name="data">大于8 byte -> 4:防伪码 ->4:packer size</param>
        /// <returns>可处理的闭包,返回NULL则需要下次重新处理</returns>
        HS_MsgWrapper OnHandleRecv(byte[] data, int dateSize);

        /// <summary>
        /// 关闭回调
        /// </summary>
        /// <param name="isSucceed">是否关闭成功</param>
        /// <param name="ip"></param>
        /// <param name="prot"></param>
        void OnClose(bool isSucceed, string ip, int prot);


        /// <summary>
        /// 网络中断
        /// </summary>
        void OnDisconnect(HS_SocketStream socket);
    }
}

