using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;

namespace Lit.Unity
{
    public class SerializeObj
    {
        #region 数据存储
        private List<SerializeObj> childs;
        private List<SerializeEntity> comps;
        private string objName = "unname";

        public string ObjName {
            get { return objName; }
            set { objName = value; }
        }
        public List<SerializeEntity> Comps
        {
            get { return comps; }
            set { comps = value; }
        }

        public List<SerializeObj> Childs
        {
         
            get { return childs; }
            set { Childs = value; }
        }
        #endregion

        #region 构造方法
        private SerializeObj() { }

        public static SerializeObj Create()
        {
            return new SerializeObj();
        }

        public static SerializeObj Create(string strJson)
        {
            JsonData data = JsonMapper.ToObject(strJson);
            return CreateByJson(data);
        }

        private static SerializeObj CreateByJson(JsonData data)
        {
            SerializeObj so = Create();
            if (data.Contains("name"))
            {
                so.ObjName = data["name"];
            }
            if (data.Contains("comps"))
            {
                InitCompsByJsonData(so, data["comps"]);
            }
            if (data.Contains("childs"))
            {
                var childDatas = data["childs"];
                for (int i = 0; i < childDatas.Count; i++)
                {
                    so.AddChild(CreateByJson(childDatas[i]));
                }
            }
            return so;
        }

        private static void InitCompsByJsonData(SerializeObj so, JsonData jsonData)
        {
            if(so == null || jsonData == null || jsonData.Count <=0)
            {
                LitLogger.ErrorFormat("{0} components is Empty {1}", so.ObjName, jsonData);
                return;
            }
            for (int i = 0; i < jsonData.Count; i++)
            {
                var data = jsonData[i];
                SerializeEntity se = new SerializeEntity(data);
                so.AddComp(se);
            }
        }

        #endregion

        #region 外界访问接口

        public void AddChild(SerializeObj so)
        {
            if (so == null)
            {
                LitLogger.Error("Con'not add null child");
                return;
            }
            if (childs == null)
                childs = new List<SerializeObj>();
            childs.Add(so);
        }

        public SerializeObj AddComp(SerializeEntity entity)
        {
            if (comps == null)
                comps = new List<SerializeEntity>();
            comps.Add(entity);
            return this;
        }

        public string Serialize()
        {
            JsonData data = ToJsonData();
            if (data == null)
                LitLogger.LogFormat("Convert To JsonData Error {0}", objName);
            return data.ToJson();
        }

        public JsonData ToJsonData()
        {
            JsonData data = new JsonData();
            data["name"] = objName;
            JsonData compsData = GetCompsData();
            if(compsData != null)
            {
                data["comps"] = compsData;
            }
            if(childs != null && childs.Count > 0)
            {
                data["childs"] = new JsonData();
                for (int i = 0; i < childs.Count; i++)
                {
                    data["childs"].Add(childs[i].ToJsonData());
                }
            }
            return data;
        }

        private JsonData GetCompsData()
        {
            JsonData data = null;
            if(comps != null && comps.Count > 0)
            {
                data = new JsonData();
                for (int i = 0; i < comps.Count; i++)
                {
                    data.Add(comps[i].Data);
                }
            }
            return data;
        }
        #endregion
    }
}
