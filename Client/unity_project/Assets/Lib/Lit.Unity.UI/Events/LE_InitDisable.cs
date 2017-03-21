using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity.UI
{

    public class LE_InitDisable : LitBehaviour, ISerializeEvent
    {
        public EventEntity Serialize()
        {
            EventEntity ee = new EventEntity();
            return ee;
        }
    }

}
