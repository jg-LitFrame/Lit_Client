using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.Unity
{
    public static class ExtendCollection {


        #region Extend List
        public static bool _Remove<T>(this IList<T> lists, T t)
        {
            if (lists.Count <= 2) 
                return lists.Remove(t);
            int index = lists.IndexOf(t);
            if (index >= 0)
            {
                lists._Swap(index, lists.Count - 1);
                lists.RemoveAt(lists.Count - 1);
                return true;
            }
            return false;
        }

        public static void _Swap<T>(this IList<T> lists, int a, int b)
        {
            if (!lists._isValidIndex(a) || !lists._isValidIndex(b))
            {
                LitLogger.ErrorFormat("Invalid Index : {0} , {1}", a, b);
                return;
            }
            T tmp = lists[a];
            lists[a] = lists[b];
            lists[b] = tmp;
        }


        public static bool _isValidIndex<T>(this IList<T> lists, int index)
        {
            if (index < 0 || index > lists.Count)
                return false;
            return true;
        }
        #endregion
    }

}
