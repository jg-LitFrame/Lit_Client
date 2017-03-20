using UnityEngine;
using System.Collections;

namespace Lit.Unity.UI
{
    public class TE_OnEnable : LitBehaviour , ISerializeEvent{
        public string handler = "";
        public EventEntity Serialize()
        {
            EventEntity eventEntity = new EventEntity();
            eventEntity.EventType = LitEventType.LE_Enable;
            eventEntity.HandleFunc = handler;
            return eventEntity;
        }
    }
}
