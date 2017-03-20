using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Lit.Unity
{
    public class LitDeserializer{

        private SerializeObj so;
        private Transform parent;

        public LitDeserializer(SerializeObj so) {
            this.so = so;
        }

        public LitDeserializer(SerializeObj so, Transform parent)
        {
            this.so = so;
            this.parent = parent;
        }

        public void Deserialize()
        {
            GameObject curGo = DeSerialize(so, parent);


        }

        private GameObject DeSerialize(SerializeObj data,Transform parent = null)
        {
            GameObject curGo = CreateGO(data.ObjName);
            if (parent != null)
                curGo.transform.SetParent(parent);
            InitComps(curGo, data.Comps);
            if(data.Childs != null && data.Childs.Count > 0)
            {
                for (int i = 0; i < data.Childs.Count; i++)
                {
                    var child = DeSerialize(data.Childs[i], curGo.transform);
                  //  child.transform.SetParent(curGo.transform);
                }
            }
            return curGo;
        }

        private void InitComps(GameObject go, List<SerializeEntity> comps)
        {
            if(comps == null || comps.Count <=0)
            {
                LitLogger.WarningFormat("{0} has no Components to Add", go.name);
                return;
            }
            for (int i = 0; i < comps.Count; i++)
            {
                InitComp(go, comps[i]);
            }
        }
        private void InitComp(GameObject go, SerializeEntity se)
        {
            if (se.Type.StartsWith("LE_"))
            {
                InitEventComp(go, se);
                return;
            }
            Type type = SerializeReg.GetType(se.Type);
            if(type == null)
            {
                LitLogger.ErrorFormat("{0} not registe in SerializeReg", se.Type);
                return;
            }
            
            if(type == typeof(Transform))
            {
                var trans = go.GetComponent<Transform>();
                TryDeserializeComp(trans, se);
            }
            else
            {
                var comp = go.AddComponent(type);
                TryDeserializeComp(comp, se);
            }
        }

        private void InitEventComp(GameObject go, SerializeEntity se)
        {
            var litLua = go.GetOrAddComponent<LitLua>();
            var eventEntity = EventEntity.Parse(se);
            litLua.RegistEvent(eventEntity);
        }

        private void TryDeserializeComp(Component comp, SerializeEntity se)
        {
            if(comp is ISerializable)
            {
                (comp as ISerializable).DeSerialize(se);
                return;
            }
            bool tryCallExtend = SerializeReg.DeSerialize(comp, se);
            if (!tryCallExtend)
            {
                //TODO 强行反射序列化
            }
        }

        private GameObject CreateGO(string name, GameObject parent = null)
        {
            GameObject go = new GameObject(name);
            if (parent != null)
                go.transform.SetParent(parent.transform);
            return go;
        }

    }
}

