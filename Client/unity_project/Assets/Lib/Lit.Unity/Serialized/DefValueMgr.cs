using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public class DefValueMgr : LitBehaviour {

        public static DefValueMgr Instance;

        public T GetDefComp<T>() where T : Component
        {
            RemoveOtherComps();
            return GetOrAddComponent<T>();
        }
    }
}
