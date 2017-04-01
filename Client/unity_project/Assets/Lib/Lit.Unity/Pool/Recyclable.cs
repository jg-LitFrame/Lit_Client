using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.Unity
{
    public  abstract class Recyclable<T> where T : Recyclable<T>, new()
    {
        private static LinkedList<T> pool = new LinkedList<T>();

        private bool _active = false;
        /// <summary>
        /// 此处Active表示是否已经被GC
        /// </summary>
        public bool Active { get { return _active; } }

        public virtual void GC()
        {
            if (_active)
            {
                pool.AddLast(this as T);
                this.OnDisable();
                _active = false;
            }else
            {
                LitLogger.ErrorFormat("Obj has been GC <{0}>", this);
            }
        }

        public static void ClearAll()
        {
            pool.Clear();
        }


        public static T New()
        {
            T t = null;
            if (pool.Count > 0)
            {
                t = pool.First as T;
                pool.RemoveFirst();
            }
            if (t == null)
            {
                t = new T();
                t.OnCreate();
            }
            t._active= true;
            t.OnEnable();
            return t;
        }


        /// <summary>
        /// G:初次被创建的时候调用
        /// </summary>
        public virtual void OnCreate() { }

        /// <summary>
        /// /G:变为可用状态时候调用
        /// </summary>
        public abstract void OnEnable();

        /// <summary>
        /// G:变为不可用状态时候调用
        /// </summary>
        public abstract void OnDisable();



    }
}
