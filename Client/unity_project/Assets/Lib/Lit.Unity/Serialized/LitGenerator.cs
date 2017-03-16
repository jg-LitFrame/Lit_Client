using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using Lit.Unity.UI;

namespace Lit.Unity
{
    public class LitGenerator : LitBehaviour {

        public List<GameObject> serializeObjs = new List<GameObject>();

        public void Generate()
        {
            for (int i = 0; i < serializeObjs.Count; i++)
            {
                SerializeGO(serializeObjs[i]);
            }
        }

        private void SerializeGO(GameObject go)
        {
            var so = GoToSerializeObj(go);
            WriteToFile(so);
        }

        private SerializeObj GoToSerializeObj(GameObject go)
        {
            SerializeObj slo = SerializeObj.Create();
            slo.ObjName = go.name;
            var allComps = GetObjAllComps(go);
            slo.Comps = allComps;

            for (int i = 0; i < go.transform.childCount; i++)
            {
                var child = go.transform.GetChild(i).gameObject;
                var so = GoToSerializeObj(child);
                slo.AddChild(so);
            }
            return slo;
        }

        private List<SerializeEntity> GetObjAllComps(GameObject go)
        {
            List<SerializeEntity> comps = new List<SerializeEntity>();
            var compList = go.GetComponents<Component>();
            for (int i = 0; i < compList.Length; i++)
            {
                var so = TrySerialize(compList[i]);
                if (so != null)
                    comps.Add(so);
                else
                    LitLogger.ErrorFormat("Can'not Serialize Component {0} on {1}", compList[i].GetType().Name, go.name);
            }
            return comps;
        }

        private SerializeEntity TrySerialize(Component comp)
        {
            SerializeEntity retSE = null;
            if (comp is ISerializable)
            {
                var sa = comp as ISerializable;
                retSE = sa.Serialize();
            }
            if(retSE == null)
            {
                retSE = TryCallSerializeByReflection(comp);
            }
            if(comp == null)
            {
                //TODO 完全通过反射的方式序列化对象
            }
            if(retSE != null && !SerializeReg.HasRegType(retSE.Type))
            {
                LitLogger.ErrorFormat("{0} not register in SeriaLizeReg,Can'not be Serialized", retSE.Type);
                retSE = null;
            }
            return retSE;
        }

        private SerializeEntity TryCallSerializeByReflection(Component comp)
        {
            #region TODO 找到合适的反射调用方式，暂时通过手动调用
            //Type type = comp.GetType();
            //var method = type.GetMethod("Serialize", BindingFlags.Static
            //            | BindingFlags.Public | BindingFlags.NonPublic);
            //if(method == null) {
            //    LitLogger.ErrorFormat("Error: Get Method {0} from  {1}", "Serialize", comp.name);
            //    return null;
            //}
            //var rst = method.Invoke(comp, null);
            //if (rst is SerializeEntity)
            //{
            //    return rst as SerializeEntity;
            //}
            #endregion

            return SerializeReg.Serialize(comp);
        }

        private void WriteToFile(SerializeObj so)
        {
            LitLogger.Log("==========");
            strJson = so.Serialize();
            LitLogger.Log(strJson);
        }


        public string strJson = "";
        //强行测试一波
        [ContextMenu("TestDeserialize")]
        public void TestDeserialize()
        {
            LitLogger.Log("===================");
            var so = SerializeObj.Create(strJson);
            LitDeserializer lder = new LitDeserializer(so);
            lder.Deserialize();
            LitLogger.Log(so.Serialize());
        }


    }
}
