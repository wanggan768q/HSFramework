using UnityEngine;
using System.Collections;
using System.Net.Sockets;


namespace HS.Net.TCP
{
    public class HS_MsgWrapper
    {
        private int _Opcode = -1;
        private byte[] _Value = null;
        private int _Model = -1;
        private int _Cmd = -1;
        private Socket _Handler = null;
        private object _Extend = null;

        public Socket Handler { get { return _Handler; } }

        /// <summary>
        /// 操作码(仅适用简单操作码)
        /// </summary>
        public int Opcode { get { return _Opcode; } }

        /// <summary>
        /// 模块
        /// </summary>
        public int Model { get { return _Model; } }
        /// <summary>
        /// 指令
        /// </summary>
        public int CMD { get { return _Cmd; } }

        public byte[] Value { get { return _Value; } }

        public object Extend { get { return _Extend; } }

        public HS_MsgWrapper(int opcode, byte[] value)
        {
            this._Opcode = opcode;

            if (value == null)
                this._Value = new byte[0];
            else
                this._Value = value;
        }

        public HS_MsgWrapper(int model,int cmd, byte[] value)
        {
            this._Model = model;
            this._Cmd = cmd;

            if (value == null)
                this._Value = new byte[0];
            else
                this._Value = value;
        }

        public HS_MsgWrapper(int model, int cmd, object extend)
        {
            this._Model = model;
            this._Cmd = cmd;
            this._Extend = extend;
        }

        public HS_MsgWrapper(Socket socket,int maxBufSize)
        {
            _Handler = socket;
            this._Value = new byte[maxBufSize];
        }
    }
}

