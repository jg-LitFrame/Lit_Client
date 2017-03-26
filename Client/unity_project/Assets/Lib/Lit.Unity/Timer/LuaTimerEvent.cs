using UnityEngine;
using System.Collections;
namespace Lit.Unity
{
    public class LuaTimerEvent : TimerEvent{

        private LitLua target;
        private string func_name;

        public LuaTimerEvent(LitLua target, string func_name, 
            float interval, int execTimes,float delay)
        {
            this.target = target;
            this.func_name = func_name;
            base.handler = DoHandler;
            base.interval = interval;
            base.execTimes = execTimes;
            base.lastTrrigerTime = Time.time - interval + delay;
        }

        public override bool CheckValid()
        {
            return base.CheckValid() 
                && target != null 
                && target.gameObject.activeInHierarchy;
        }
        public void DoHandler(){
            target.CallLuaFunc(func_name);
        }
    }
}
