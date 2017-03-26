using UnityEngine;
using System.Collections;

namespace Lit.Unity.UI
{
    public static partial class UIUtils
    {
        public static void CatchEvent()
        {
            LitLua.CatchEvent();
        }

        public static LitLua GetLit(string LID)
        {
            if(!LID.isEmpty())
                return LitLua.UID_Get(LID);
            return null;
        }

    }

}

