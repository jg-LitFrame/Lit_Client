using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public partial class LitLua {

        private string module_name;
        //TODO
        public void InitLuaFile(string file_name)
        {
           // int index = file_name.IndexOf('.');

            this.module_name = file_name;//.Substring(0,index);
            LoadLuafile(file_name);
        }

        //TODO
        public void LoadLuafile(string file_name)
        {
          //  LitLogger.Log(file_name);
            FaceMgr.luaMgr.LoadScript(file_name);

        }

        public void CallLuaFunc(string func_name, object obj)
        {
            if (isRegisterEvent(LitEventType.LE_Handler))
            {
                FaceMgr.luaMgr.CallFunc(module_name, func_name, obj);
            }
        }


    }
}
