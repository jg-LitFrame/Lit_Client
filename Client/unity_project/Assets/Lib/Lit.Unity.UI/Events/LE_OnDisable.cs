using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity.UI
{
    public class LE_OnDisable : LitBehaviour, ISerializeEvent
    {
        public string handler = "";

        public EventEntity Serialize()
        {
            if (handler.isEmpty())
            {
                LitLogger.ErrorFormat("Can not Add LE_OnDisable with no handler");
                return null;
            }
            EventEntity eventEntity = new EventEntity();
            eventEntity.EventType = LitEventType.LE_Disable;
            eventEntity.EventParam = handler;
            return eventEntity;
        }
    }
}
