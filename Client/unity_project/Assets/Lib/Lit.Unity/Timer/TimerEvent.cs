using UnityEngine;
namespace Lit.Unity
{
    public class TimerEvent
    {

        public _D_Void handler;
        public int execTimes = 1;
        public float interval = 1;
        public float delay = 0;
        public float lastTrrigerTime = -10000;

        private bool outDate = false;
        /// <summary>
        /// 创建计时器事件
        /// </summary>
        /// <param name="onHandler"> 触发执行事件</param>
        /// <param name="interval"> 触发间隔，interval == -1：逐帧触发， interval > 0 : 间隔时间</param>
        /// <param name="execTimes"> 触发次数，execTimes > 0 有效触发次数 , execTimes == -1 无限触发, execTimes == 0 事件过期 </param>
        /// <param name="delay"> 延迟开始时间 </param>
        public TimerEvent(_D_Void onHandler, float interval, int execTimes ,float delay = 0)
        {
            this.handler = onHandler;
            this.interval = interval;
            this.execTimes = execTimes;
            lastTrrigerTime = Time.time - interval + delay;
            outDate = false;
        }
        public TimerEvent() { }

        public void Stop()
        {
            outDate = true;
        }

        public virtual bool CheckValid()
        {
            if (execTimes == 0 || execTimes < -1)
            {
                outDate = true;
            }
            return !outDate;
        }

        public virtual void Tick()
        {
            if (isTrrigerTime())
            {
                if (execTimes != -1) //无限循环模式
                    --execTimes;
                lastTrrigerTime = Time.time;
                if (handler != null) handler();
            }
        }

        private bool isTrrigerTime()
        {
            if (interval == -1) return true;
            if (lastTrrigerTime + interval <= Time.time)
            {
                return true;
            }
            return false;
        }

    }

}
