using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity.UI
{
    public class LE_UID : LitBehaviour, ISerializeEvent
    {
        public string UID = "";

        public EventEntity Serialize()
        {
            if (UID.isEmpty())
            {
                LitLogger.ErrorFormat("Can not Add LE_UID with no domain : <{0}>",gameObject.name);
                return null;
            }
            EventEntity eventEntity = new EventEntity();
            eventEntity.EventType = LitEventType.LE_UID;
            eventEntity.HandleFunc = UID;
            return eventEntity;
        }
      
    }
}
