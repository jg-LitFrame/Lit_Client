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
            ee.EventType = LitEventType.LE_InitDisable;
            ee.EventParam = "false";
            return ee;
        }
    }

}
