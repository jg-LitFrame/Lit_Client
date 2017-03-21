using UnityEngine;
using System.Collections;
using Lit.Unity;
using LuaInterface;

public class LuaMgr :  SingletonBehaviour<LuaMgr>{



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
}
