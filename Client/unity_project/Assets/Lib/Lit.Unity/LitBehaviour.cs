using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity
{
    public class LitBehaviour : MonoBehaviour {

        #region 协程封装
        public Coroutine WaitFor(float delay, _D_Void voidFunc)
        {
            if (voidFunc == null) return null;
            Coroutine c = null;
            if (delay <= 1e-4) voidFunc();
            else if (enabled) { c = StartCoroutine(_WaitFor(delay, voidFunc)); }
            return c;
        }

        public Coroutine WaitFor(_D_OuterBool condition, _D_Void voidFunc)
        {
            if (voidFunc == null) return null;
            Coroutine c = null;
            if (enabled) { c = StartCoroutine(_WaitFor(condition, voidFunc)); }
            return c;
        }

        public Coroutine WaitForFrame(int frame, _D_Void voidFunc)
        {
            if (voidFunc == null) return null;
            Coroutine c = null;
            if (enabled) { c = StartCoroutine(_WaitForFrame(frame, voidFunc)); }
            return c;
        }

        public Coroutine WaitForCondition(_D_OuterBool condition, _D_Void voidFunc, float timeSpan = 0.1f, float timeLimit = 0)
        {
            if (voidFunc == null) return null;
            Coroutine c = null;
            if (enabled) { c = StartCoroutine(_WaitFor(condition, voidFunc, timeSpan, timeLimit)); }
            return c;
        }

        public Coroutine RepeatEvery(float delay, _D_Void voidFunc, bool rightNow)
        {
            if (delay <= 0) throw new Exception("You can not repeat every '0' sec");
            else
            {
                if (rightNow) voidFunc();
                if (gameObject.activeInHierarchy) return StartCoroutine(_RepeatEvery(delay, voidFunc));
                else return null;
            }
        }

        public IEnumerator _WaitFor(float delay, _D_Void voidFunc)
        {
            yield return new WaitForSeconds(delay);
            voidFunc();
        }

        public IEnumerator _WaitFor(_D_OuterBool condition, _D_Void voidFunc)
        {
            while (!condition()) yield return 0;
            voidFunc();
        }

        public IEnumerator _WaitFor(_D_OuterBool condition, _D_Void voidFunc, float timeSpan, float timelimit)
        {
            float tStrat = Time.time;
            while (!condition() && (timelimit <= 0 || Time.time - tStrat < timelimit)) yield return new WaitForSeconds(timeSpan);
            voidFunc();
        }

        public IEnumerator _WaitForFrame(int delayframe, _D_Void voidFunc)
        {
            int frameCount = Time.frameCount;
            while (Time.frameCount < frameCount + delayframe) yield return 0;
            voidFunc();
        }

        public IEnumerator _WaitForFrameAndTime(int delayframe, float delaytime, _D_Void voidFunc)
        {
            int frameCount = Time.frameCount;
            yield return new WaitForSeconds(delaytime); //先等时间
            while (Time.frameCount < frameCount + delayframe) yield return 0;
            voidFunc();
        }

        public IEnumerator _RepeatEvery(float delay, _D_Void voidFunc)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                voidFunc();
            }
        }

        public IEnumerator _RepeatEveryFrame(int delayframe, _D_Void voidFunc)
        {
            while (true)
            {
                int frameCount = Time.frameCount;
                while (Time.frameCount < frameCount + delayframe) yield return 0;
                voidFunc();
            }
        }

        public IEnumerator _RepeatEvery(float delay, _D_Void voidFunc, _D_OuterBool pred)
        {
            while (pred())
            {
                yield return new WaitForSeconds(delay);
                voidFunc();
            }
        }

        public IEnumerator _RepeatEveryFrame(int delayframe, _D_Void voidFunc, _D_OuterBool pred)
        {
            while (pred())
            {
                yield return delayframe;
                voidFunc();
            }
        }

        public IEnumerator _RepeatEvery(float delay, Action voidFunc, int times)
        {
            for (int i = 0; i < times; i++)
            {
                yield return new WaitForSeconds(delay);
                voidFunc();
            }
        }

        public IEnumerator _RepeatEveryFrame(int delayframe, Action voidFunc, int times)
        {
            for (int i = 0; i < times; i++)
            {
                yield return delayframe;
                voidFunc();
            }
        }

        public IEnumerator _RepeatEveryFrame(int delayframe, Action<int> timesFunc, int times)
        {
            for (int i = 0; i < times; i++)
            {
                yield return delayframe;
                timesFunc(i + 1);
            }
        }
        #endregion

        #region 获得组件相关封装
        public T GetOrAddComponent<T>() where T : Component
        {
            T t = GetComponent<T>();
            if (t == default(T))
                t = gameObject.AddComponent<T>();
            return t;
        }
        public bool isAttach<T>() where T : Component{
            T t = GetComponent<T>();
            return t != default(T);
        }

        public virtual void RemoveOtherComps()
        {
            Component[] cs = GetComponents<Component>();
            for (int i = 0; i < cs.Length; i++)
            {
                if (!(cs[i] is Transform) && cs[i].GetType() != GetType())
                    GameObject.DestroyImmediate(cs[i]);
            }
        }
        #endregion

    }
}
