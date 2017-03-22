using UnityEngine;
using System.Collections;

namespace Lit.Unity.UI
{
    public static partial class UIUtils
    {

        #region Log 封装
        public static void Log(object obj)
        {
            LitLogger.Log(obj);
        }

        #endregion



        public static Sprite LoadSprite(string path)
        {
            if (path == null) return null;
            return Resources.Load<Sprite>(path);
        }
    }
}
