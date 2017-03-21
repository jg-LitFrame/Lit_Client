using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public interface ISerializable{

        SerializeEntity Serialize();

        void DeSerialize(SerializeEntity data);

    }


    #region 事件序列化

    public enum LitEventType
    {
        LE_None = 0,
        LE_Enable = 1,
        LE_Disable = 2,
        LE_OnClick = 3,
        LE_Handler = 4,
        LE_UID = 5,
        LE_InitDisable = 6,
    }

    public class EventEntity
    {
        private LitEventType eventType = LitEventType.LE_None;
        private string handleFunc = "";
        
        public LitEventType EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }

        public string HandleFunc
        {
            get { return handleFunc; }
            set { handleFunc = value; }
        }

        public bool isValid
        {
            get
            {
                if (EventType == LitEventType.LE_None || handleFunc.isEmpty())
                    return false;
                return true;
            }
        }

        public SerializeEntity ToSerializeEntity()
        {
            SerializeEntity se = new SerializeEntity();
            se.Type = EventType.ToString();
            se.Add("HF", HandleFunc);
            return se;
        }

        public static EventEntity Parse(SerializeEntity se)
        {
            EventEntity eventEntity = new EventEntity();
            eventEntity.HandleFunc = se["HF"];
            eventEntity.EventType = (LitEventType)System.Enum.Parse(typeof(LitEventType), se.Type);
            return eventEntity;
        }

        public override string ToString()
        {
            return string.Format("Type : {0} , HandlerFunc : {1}", EventType, HandleFunc);
        }

    }

    public interface ISerializeEvent
    {
        EventEntity Serialize();
    }
    #endregion
}
