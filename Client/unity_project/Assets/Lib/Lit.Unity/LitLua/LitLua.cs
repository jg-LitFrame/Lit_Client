using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.Unity
{
    public partial class LitLua : LitBehaviour {

        public LitLua GetParent()
        {
            Transform p = transform.parent;
            if (p == null)
                return null;
            return p.gameObject.GetOrAddComponent<LitLua>(); 
        }
    
    }
}
