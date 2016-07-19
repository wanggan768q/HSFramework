using UnityEngine;
using System.Collections;

namespace HS.Base
{
    /// <summary>
    /// 单例GameObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HS_SingletonGameObject<T> : MonoBehaviour where T : HS_SingletonGameObject<T>
    {
        protected static T S_Instance;

        protected static T Instance
        {
            get
            {
                if (!S_Instance)
                {
                    T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];
                    string objectName = "S_" + typeof(T).Name;
                    if (managers.Length != 0)
                    {
                        if (managers.Length == 1)
                        {
                            S_Instance = managers[0];
                            S_Instance.gameObject.name = objectName;
                            return S_Instance;
                        }
                        else
                        {
                            D.LogForce("You have more than one " + typeof(T).Name + " in the scene. You only need 1, it's a singleton!");
                            foreach (T manager in managers)
                            {
                                Destroy(manager.gameObject);
                            }
                        }
                    }
                    GameObject gO = new GameObject(objectName, typeof(T));
                    S_Instance = gO.GetComponent<T>();
                    DontDestroyOnLoad(gO);
                }
                return S_Instance;
            }
//             set
//             {
//                 S_Instance = value as T;
//             }
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
            GameObject.Destroy(S_Instance.gameObject);
            S_Instance = null;
        }
    }
}


