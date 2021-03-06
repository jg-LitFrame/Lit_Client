﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Lit_Unity_LitLuaWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Lit.Unity.LitLua), typeof(Lit.Unity.LitBehaviour));
		L.RegFunction("GetParent", GetParent);
		L.RegFunction("CatchEvent", CatchEvent);
		L.RegFunction("RegisterLifeEvent", RegisterLifeEvent);
		L.RegFunction("isRegisterEvent", isRegisterEvent);
		L.RegFunction("GetEventEntity", GetEventEntity);
		L.RegFunction("TryHandleEvent", TryHandleEvent);
		L.RegFunction("GenerateEvent", GenerateEvent);
		L.RegFunction("OnDisable", OnDisable);
		L.RegFunction("OnEnable", OnEnable);
		L.RegFunction("RegistEvent", RegistEvent);
		L.RegFunction("InitUID", InitUID);
		L.RegFunction("InitLifeEvent", InitLifeEvent);
		L.RegFunction("InitLuaFile", InitLuaFile);
		L.RegFunction("LoadLuafile", LoadLuafile);
		L.RegFunction("CallLuaFunc", CallLuaFunc);
		L.RegFunction("UID_Register", UID_Register);
		L.RegFunction("UID_Contains", UID_Contains);
		L.RegFunction("UID_Get", UID_Get);
		L.RegFunction("UID_Delete", UID_Delete);
		L.RegFunction("UID_Clear", UID_Clear);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetParent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			Lit.Unity.LitLua o = obj.GetParent();
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CatchEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			Lit.Unity.LitLua.CatchEvent();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegisterLifeEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			Lit.Unity.EventEntity arg0 = (Lit.Unity.EventEntity)ToLua.CheckObject(L, 2, typeof(Lit.Unity.EventEntity));
			obj.RegisterLifeEvent(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isRegisterEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			Lit.Unity.LitEventType arg0 = (Lit.Unity.LitEventType)ToLua.CheckObject(L, 2, typeof(Lit.Unity.LitEventType));
			bool o = obj.isRegisterEvent(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEventEntity(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			Lit.Unity.LitEventType arg0 = (Lit.Unity.LitEventType)ToLua.CheckObject(L, 2, typeof(Lit.Unity.LitEventType));
			Lit.Unity.EventEntity o = obj.GetEventEntity(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryHandleEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			obj.TryHandleEvent();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GenerateEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			Lit.Unity.LitEventType arg0 = (Lit.Unity.LitEventType)ToLua.CheckObject(L, 2, typeof(Lit.Unity.LitEventType));
			object arg1 = ToLua.ToVarObject(L, 3);
			obj.GenerateEvent(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDisable(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			obj.OnDisable();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnEnable(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			obj.OnEnable();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegistEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			Lit.Unity.EventEntity arg0 = (Lit.Unity.EventEntity)ToLua.CheckObject(L, 2, typeof(Lit.Unity.EventEntity));
			obj.RegistEvent(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitUID(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			string arg0 = ToLua.CheckString(L, 2);
			obj.InitUID(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitLifeEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			Lit.Unity.EventEntity arg0 = (Lit.Unity.EventEntity)ToLua.CheckObject(L, 2, typeof(Lit.Unity.EventEntity));
			obj.InitLifeEvent(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitLuaFile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			string arg0 = ToLua.CheckString(L, 2);
			obj.InitLuaFile(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadLuafile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.CheckObject(L, 1, typeof(Lit.Unity.LitLua));
			string arg0 = ToLua.CheckString(L, 2);
			obj.LoadLuafile(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallLuaFunc(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(Lit.Unity.LitLua), typeof(string)))
			{
				Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				obj.CallLuaFunc(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(Lit.Unity.LitLua), typeof(string), typeof(object)))
			{
				Lit.Unity.LitLua obj = (Lit.Unity.LitLua)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				obj.CallLuaFunc(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Lit.Unity.LitLua.CallLuaFunc");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UID_Register(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			Lit.Unity.LitLua arg1 = (Lit.Unity.LitLua)ToLua.CheckUnityObject(L, 2, typeof(Lit.Unity.LitLua));
			Lit.Unity.LitLua.UID_Register(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UID_Contains(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			bool o = Lit.Unity.LitLua.UID_Contains(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UID_Get(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			Lit.Unity.LitLua o = Lit.Unity.LitLua.UID_Get(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UID_Delete(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			bool o = Lit.Unity.LitLua.UID_Delete(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UID_Clear(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			Lit.Unity.LitLua.UID_Clear();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

