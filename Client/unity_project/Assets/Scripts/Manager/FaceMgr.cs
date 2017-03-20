using UnityEngine;
using System.Collections;
using System;
using Lit.Unity;

public class FaceMgr: SingletonBehaviour<FaceMgr>
{
    #region 初始化<SingletonBehaviour>管理器
    protected override void Init()
    {
        base.Init();
        IndieMgrInit();
        InitSingleBehaviour(typeof(AudioMgr));
        InitSingleBehaviour(typeof(LuaMgr));
    }

    private void IndieMgrInit()
    {
        TableMgr.GetInstance();
       // LuaMgr.GetInstance();
    }

    private void InitSingleBehaviour(Type mgrType)
    {
        GameObject go = new GameObject(mgrType.Name);
        go.AddComponent(mgrType);
        go.transform.SetParent(transform);
    }
    #endregion

    public static AudioMgr audioMgr
    {
        get { return AudioMgr.GetInstance(); }
    }

    public static TableMgr tableMgr
    {
        get { return TableMgr.GetInstance(); }
    }

    public static LuaMgr luaMgr
    {
        get { return LuaMgr.GetInstance(); }
    }
}
