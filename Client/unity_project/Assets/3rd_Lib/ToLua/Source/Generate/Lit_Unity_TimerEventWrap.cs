﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Lit_Unity_TimerEventWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Lit.Unity.TimerEvent), typeof(System.Object));
		L.RegFunction("Stop", Stop);
		L.RegFunction("CheckValid", CheckValid);
		L.RegFunction("Tick", Tick);
		L.RegFunction("New", _CreateLit_Unity_TimerEvent);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("handler", get_handler, set_handler);
		L.RegVar("execTimes", get_execTimes, set_execTimes);
		L.RegVar("interval", get_interval, set_interval);
		L.RegVar("delay", get_delay, set_delay);
		L.RegVar("lastTrrigerTime", get_lastTrrigerTime, set_lastTrrigerTime);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLit_Unity_TimerEvent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				Lit.Unity.TimerEvent obj = new Lit.Unity.TimerEvent();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(Lit.Unity._D_Void), typeof(float), typeof(int), typeof(float)))
			{
				Lit.Unity._D_Void arg0 = null;
				LuaTypes funcType1 = LuaDLL.lua_type(L, 1);

				if (funcType1 != LuaTypes.LUA_TFUNCTION)
				{
					 arg0 = (Lit.Unity._D_Void)ToLua.CheckObject(L, 1, typeof(Lit.Unity._D_Void));
				}
				else
				{
					LuaFunction func = ToLua.ToLuaFunction(L, 1);
					arg0 = DelegateFactory.CreateDelegate(typeof(Lit.Unity._D_Void), func) as Lit.Unity._D_Void;
				}

				float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 4);
				Lit.Unity.TimerEvent obj = new Lit.Unity.TimerEvent(arg0, arg1, arg2, arg3);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: Lit.Unity.TimerEvent.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Stop(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)ToLua.CheckObject(L, 1, typeof(Lit.Unity.TimerEvent));
			obj.Stop();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckValid(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)ToLua.CheckObject(L, 1, typeof(Lit.Unity.TimerEvent));
			bool o = obj.CheckValid();
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Tick(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)ToLua.CheckObject(L, 1, typeof(Lit.Unity.TimerEvent));
			obj.Tick();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_handler(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			Lit.Unity._D_Void ret = obj.handler;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index handler on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_execTimes(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			int ret = obj.execTimes;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index execTimes on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_interval(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			float ret = obj.interval;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index interval on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_delay(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			float ret = obj.delay;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index delay on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lastTrrigerTime(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			float ret = obj.lastTrrigerTime;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index lastTrrigerTime on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_handler(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			Lit.Unity._D_Void arg0 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg0 = (Lit.Unity._D_Void)ToLua.CheckObject(L, 2, typeof(Lit.Unity._D_Void));
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 2);
				arg0 = DelegateFactory.CreateDelegate(typeof(Lit.Unity._D_Void), func) as Lit.Unity._D_Void;
			}

			obj.handler = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index handler on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_execTimes(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.execTimes = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index execTimes on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_interval(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.interval = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index interval on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_delay(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.delay = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index delay on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lastTrrigerTime(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Lit.Unity.TimerEvent obj = (Lit.Unity.TimerEvent)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.lastTrrigerTime = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index lastTrrigerTime on a nil value" : e.Message);
		}
	}
}
