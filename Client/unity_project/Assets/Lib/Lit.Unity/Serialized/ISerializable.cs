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


    }

    public interface ISerializeEvent
    {
        EventEntity Serialize();
    }
    #endregion
}
