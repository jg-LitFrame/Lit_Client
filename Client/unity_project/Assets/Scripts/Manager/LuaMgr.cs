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
      //  new LuaResLoader();
       // luaMgr.AddSearchPath(FileTools.ToLuaSystemInitPath);
        LuaBinder.Bind(luaMgr);
        luaMgr.Start();
    }
}
