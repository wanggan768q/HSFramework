using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HS.Base
{
    public class HS_ObjectPool<T> where T : class,IResetable,new()
    {
        private Stack<T> _ObjectStack = new Stack<T>();

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public T New()
        {
            T t = null;
            if(_ObjectStack.Count > 0)
            {
                t = _ObjectStack.Pop();
            }
            else
            {
                t = new T();
            }
            t.New();
            return t;
        }

        /// <summary>
        /// 回收一个实例
        /// </summary>
        /// <param name="t"></param>
        public void Recycle(T t)
        {
            t.Rest();
            _ObjectStack.Push(t);
        }
    }

    public interface IResetable
    {
        void New();
        void Rest();
    }
}

