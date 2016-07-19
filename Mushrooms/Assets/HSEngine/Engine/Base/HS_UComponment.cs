using UnityEngine;
using System.Collections;

namespace HS.Base
{
    /// <summary>
    /// 自定义组件基础类
    /// </summary>
    public abstract class HS_UComponment
    {
        protected object _Owner;
        protected bool _Enable = true;

        public object Owner
        {
            get
            {
                return _Owner;
            }
        }

        public bool Enable
        {
            get
            {
                return _Enable;
            }
            set
            {
                bool old = _Enable;
                _Enable = value;
                if (_Enable != old)
                {
                    if (null != _Owner)
                    {
                        OnEnable();
                    }
                    else
                    {
                        OnDisable();
                    }
                }
            }
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        public HS_UComponment()
        {

        }

        public HS_UComponment(object owner)
        {
            this._Owner = owner;
        }

        public virtual void OnDestroy()
        {
            this._Owner = null;
        }
    }
}


