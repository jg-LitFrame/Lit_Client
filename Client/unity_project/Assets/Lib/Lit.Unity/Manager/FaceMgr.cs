using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity{
    public class FaceMgr: SingletonBehaviour<FaceMgr>
    {
        #region 初始化<SingletonBehaviour>管理器
        protected override void Init()
        {
            base.Init();
            InitSingleBehaviour(typeof(AudioMgr));
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
    }
}
