using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity.UI
{
    public class LE_OnClick : LitBehaviour, ISerializeEvent
    {
        public string handler = "";
        public EventEntity Serialize()
        {
            if (handler.isEmpty())
            {
                LitLogger.ErrorFormat("Can not Add LE_OnClick with no handler");
                return null;
            }
            EventEntity eventEntity = new EventEntity();
            eventEntity.EventType = LitEventType.LE_OnClick;
            eventEntity.EventParam = handler;
            return eventEntity;
        }
    }
}
