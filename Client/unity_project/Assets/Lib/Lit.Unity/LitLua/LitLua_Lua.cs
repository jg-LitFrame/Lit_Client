using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public partial class LitLua {



        public void InitLuaFile(string file_name)
        {
            LoadLuafile(file_name);
        }

        public void LoadLuafile(string file_name)
        {
            LitLogger.Log(file_name);

        }

        public void CallLuaFunc(string func_name, params object[] ps)
        {
            LitLogger.Log(func_name);
        }


    }
}
