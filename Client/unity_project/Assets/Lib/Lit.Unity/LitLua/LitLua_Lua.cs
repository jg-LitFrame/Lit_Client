using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public partial class LitLua {

        private string module_name;
        //TODO
        public void InitLuaFile(string file_name)
        {
            this.module_name = file_name.Replace('.','_');
            LoadLuafile(file_name);
        }

        //TODO
        public void LoadLuafile(string file_name)
        {
            file_name = string.Concat(file_name.Replace('.', '/'), ".lua");
            FaceMgr.luaMgr.LoadScript(file_name);
        }

        public void CallLuaFunc(string func_name, object obj)
        {
            if (isRegisterEvent(LitEventType.LE_Handler))
            {
                FaceMgr.luaMgr.CallFunc(module_name, func_name, obj);
            }
        }

        public void CallLuaFunc(string func_name)
        {
            if (isRegisterEvent(LitEventType.LE_Handler))
            {
                FaceMgr.luaMgr.CallFunc(module_name, func_name);
            }
        }


    }
}
