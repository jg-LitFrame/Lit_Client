using UnityEngine;
using System.Collections;
using Lit.Unity;
using LuaInterface;

public class LuaMgr :  SingletonBehaviour<LuaMgr>
{


    #region 初始化
    private  LuaState luaMgr;
    protected override void Init()
    { 
        //初始化lua
        luaMgr = new LuaState();
        InitLuaSearchPath();
        LuaBinder.Bind(luaMgr);
        luaMgr.Start();
        InitLuaSystem();
    }

    private void InitLuaSystem()
    {
        luaMgr.DoFile("system");
    }

    private void InitLuaSearchPath()
    {
        new LuaResLoader();
        luaMgr.AddSearchPath(FileTools.ToLuaSystemInitPath);
        luaMgr.AddSearchPath(FileTools.SysLuaPath);
    }
    #endregion


    public object[] LoadScript(string file_name){
        return luaMgr.DoFile(file_name);
    }

    //TODO 效率待测
    public object[] CallFunc(string module_name, string func_name, params object[] args)
    {
        string full_name = string.Format("{0}.{1}",module_name,func_name);
        return CallFunc(full_name, args);
    }

    public void CallFunc(string module_name, string func_name, object p)
    {
        string full_name = string.Format("{0}.{1}", module_name, func_name);
        CallFunc(full_name, p);
    }


    public void CallFunc(string funcFunName, object p)
    {
        try
        {
            var func = luaMgr.GetFunction(funcFunName);
            func.Push(p);
            func.Call();
            func.EndPCall();

        }catch(System.Exception e)
        {
            LitLogger.Error(e.Message);
        }
    }

    public object[] CallFunc(string funcFunName, params object[] args)
    {
        try
        {
            var func = luaMgr.GetFunction(funcFunName);
            return func.Call(args);
        }
        catch (System.Exception e)
        {
            LitLogger.Error(e.Message);
            return null;
        }
    }
}
