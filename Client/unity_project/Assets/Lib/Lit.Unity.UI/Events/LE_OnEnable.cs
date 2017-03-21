using UnityEngine;
using System.Collections;

namespace Lit.Unity.UI
{
    public class LE_OnEnable : LitBehaviour , ISerializeEvent{
        public string handler = "";
        public EventEntity Serialize()
        {
            if (handler.isEmpty())
            {
                LitLogger.ErrorFormat("Can not Add LE_OnEnable with no handler");
                return null;
            }
            EventEntity eventEntity = new EventEntity();
            eventEntity.EventType = LitEventType.LE_Enable;
            eventEntity.EventParam = handler;
            return eventEntity;
        }
    }
}
