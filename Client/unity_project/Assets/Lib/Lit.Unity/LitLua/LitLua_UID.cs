using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.Unity
{
    public partial class LitLua {

        private static Dictionary<string, LitLua> UIDMap = new Dictionary<string, LitLua>();

        public static void UID_Register(string uid, LitLua obj)
        {
            if(uid.isEmpty() || obj == null)
            {
                LitLogger.ErrorFormat("Register Invalid UID : <{0}> , <{1}>", uid, obj);

            }else if (UID_Contains(uid))
            {
                LitLogger.WarningFormat("UID <{0}> repeated !! => {1} will replace by {2}",uid,UID_Get(uid),obj);
                UIDMap[uid] = obj;

            }else
            {
                UIDMap.Add(uid, obj);
            }
            
        }

        public static bool UID_Contains(string uid)
        {
            if (uid.isEmpty())
                return false;
            return UIDMap.ContainsKey(uid);
        }


        public static LitLua UID_Get(string uid)
        {
            LitLua ret = null;
            UIDMap.TryGetValue(uid, out ret);
            return ret;
        }

        public static bool UID_Delete(string uid)
        {
            if (!UID_Contains(uid))
                return false;
            return UIDMap.Remove(uid);
        }

        public static void UID_Clear()
        {
            UIDMap.Clear();
        }

    }

}
