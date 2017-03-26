using UnityEngine;
using System.Collections;

namespace Lit.Unity.UI
{
    public static partial class UIUtils
    {
        public static void WaitFor(LitLua lit, string func_name, float delay)
        {
            RegistTimer(lit, func_name, 0, 1, delay);
        }

        public static LuaTimerEvent RepeatedCall(LitLua lit, string func_name, float interval)
        {
            return RegistTimer(lit, func_name, interval, -1, 0);
        }

        public static LuaTimerEvent RepeatedCall(LitLua lit, string func_name, float interval, float delay)
        {
            return RegistTimer(lit, func_name, interval, -1, delay);
        }

        public static LuaTimerEvent RegistTimer(LitLua lit, string func_name, float interval, int callTimes, float delay)
        {
            LuaTimerEvent lte = new LuaTimerEvent(lit, func_name, interval, callTimes, delay);
            FaceMgr.timerMgr.RegistTimer(lte);
            return lte;
        }


    }
}
