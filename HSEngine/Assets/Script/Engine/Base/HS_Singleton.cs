using UnityEngine;
using System.Collections;

namespace HS.Base
{
    /// <summary>
    /// 单例抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HS_Singleton<T> : System.Object where T : HS_Singleton<T>, new()
    {
        private static T S_Instance;

        protected static T Instance
        {
            get
            {
                if(null == S_Instance)
                {
                    S_Instance = new T();
                }
                return S_Instance;
            }
        }

        public static T GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 销毁实例
        /// </summary>
        public virtual void Destory()
        {
            S_Instance = null;
        }
    }

}
