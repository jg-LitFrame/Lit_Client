using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Lit.Unity
{
    public partial class LitLua {

        #region 事件定义
        public class TrrigerEvent
        {
            private EventEntity eventInfo = null;
            private object sender;

            public EventEntity EventInfo
            {
                get { return eventInfo; }
                set { eventInfo = value; }
            }
            public object Sender
            {
                get { return sender; }
                set { sender = value; }
            }

            public TrrigerEvent(EventEntity ee, object sender)
            {
                this.eventInfo = ee;
                this.sender = sender;
            }
        }

        private static TrrigerEvent curEvent;
        public static void CatchEvent()
        {
            curEvent = null;
        }

        //因为事件的个数很有限，so直接用序列的效率更高
        private List<EventEntity> _lifeEvents;
        protected List<EventEntity> LifeEvents
        {
            get { return _lifeEvents; }
            set { _lifeEvents = value; }
        }

        public void RegisterLifeEvent(EventEntity e)
        {
            if (!e.isValid)
            {
                LitLogger.ErrorFormat("Invalid Event : {0}", e.ToString());
                return;
            }
            if (LifeEvents == null)
                LifeEvents = new List<EventEntity>();
            LifeEvents.Add(e);
        }

        public bool isRegisterEvent(LitEventType type)
        {
            if (LifeEvents == null) return false;
            for (int i = 0; i < LifeEvents.Count; i++)
            {
                if (LifeEvents[i].EventType == type)
                    return true;
            }
            return false;
        }

        public EventEntity GetEventEntity(LitEventType type)
        {
            if (LifeEvents == null) return null;
            for (int i = 0; i < LifeEvents.Count; i++)
            {
                if (LifeEvents[i].EventType == type)
                    return LifeEvents[i];
            }
            return null;
        }

        #endregion

        
        #region 事件触发
        public void TryHandleEvent()
        {
            if (curEvent == null)
                return;
            CallLuaFunc(curEvent.EventInfo.EventParam, curEvent.Sender);

            if (curEvent != null)
            {
                var p = GetParent();
                if (p == null)
                {
                    LitLogger.ErrorFormat("{0} No Handler for Event : {1}", curEvent.Sender, curEvent.EventInfo);
                    CatchEvent();
                }
                else
                {
                    p.TryHandleEvent();
                }
            }
        }

        public void GenerateEvent(LitEventType type, object sender)
        {
            if(type == LitEventType.LE_None || sender == null)
            {
                LitLogger.ErrorFormat("Trriger Invalid Event : {0} , {1}", type, sender);
                return;
            }
            var ee = GetEventEntity(type);
            if(ee != null)
            {
                TrrigerEvent te = new TrrigerEvent(ee, sender);
                curEvent = te;
                TryHandleEvent();
            }
        }

        public void OnDisable()
        {
            GenerateEvent(LitEventType.LE_Disable, this);
        }

        public void OnEnable()
        {
            GenerateEvent(LitEventType.LE_Enable, this);
        }

        #endregion


        #region 反序列化时进行初始化操作
        public void RegistEvent(EventEntity litEvent)
        {
            switch (litEvent.EventType)
            {
                case LitEventType.LE_None:
                    LitLogger.ErrorFormat("Register Invalid Event Type {0} , {1}",
                        litEvent.EventType, litEvent.EventParam);
                    break;
                case LitEventType.LE_UID:
                    RegisterLifeEvent(litEvent);
                    InitUID(litEvent.EventParam);
                    break;
                case LitEventType.LE_Handler:
                    RegisterLifeEvent(litEvent);
                    InitLuaFile(litEvent.EventParam);
                    break;
                case LitEventType.LE_InitDisable:
                    InitLitState(litEvent);
                    break;
                default:
                    InitLifeEvent(litEvent);
                    break;
            }
        }

        private void InitLitState(EventEntity litEvent)
        {
            RegisterLifeEvent(litEvent);
            if (!litEvent.EventParam.isEmpty() && string.Compare(litEvent.EventParam, "false", true) == 0)
                gameObject.SetActive(false);
        }
        public void InitUID(string uid)
        {
            UID_Register(uid, this);
        }

        public void InitLifeEvent(EventEntity e)
        {
            RegisterLifeEvent(e);
        }
        #endregion
    }

}
