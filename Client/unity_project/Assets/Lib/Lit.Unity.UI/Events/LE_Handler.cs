using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity.UI
{
    public class LE_Handler : LitBehaviour , ISerializeEvent{
        public string script_path = "";

        public EventEntity Serialize()
        {
            if (script_path.isEmpty())
            {
                LitLogger.ErrorFormat("Can not Add LE_Handler with no script_path");
                return null;
            }
            EventEntity eventEntity = new EventEntity();
            eventEntity.EventType = LitEventType.LE_Handler;
            eventEntity.HandleFunc = script_path;
            return eventEntity;
        }
    }

}
