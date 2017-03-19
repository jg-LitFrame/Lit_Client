using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public class Singleton<T> where T : class,new()
    {
        private static T _inst = default(T);

        protected Singleton() { 
            Init();
        }

        public static T GetInstance()
        {
            if(_inst == null)
            {
                _inst = new T();
                Singleton<T> sg = _inst as Singleton<T>;
               
            }
            return _inst;
        }

        public virtual void Init() { }
    }

    public class SingletonBehaviour<T> : LitBehaviour where T : LitBehaviour
    {
        private static T _inst = default(T);

        private void Awake()
        {
            if(_inst != null)
            {
                GameObject.DestroyImmediate(_inst);
                LitLogger.ErrorFormat("Can not Repeat Create {0}", typeof(T).Name);
            }
            _inst = this as T;
            (_inst as SingletonBehaviour<T>).Init();
        }
        public static T GetInstance()
        {
            if(_inst == null)
                LitLogger.ErrorFormat("{0} Not Init in FaceMgr", typeof(T).Name);
            return _inst;
        }

        protected virtual void Init() { }
    }
}
