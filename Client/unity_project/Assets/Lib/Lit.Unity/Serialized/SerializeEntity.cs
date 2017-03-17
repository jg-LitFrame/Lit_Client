using UnityEngine;
using System.Collections;
using LitJson;

namespace Lit.Unity
{
    public class SerializeEntity {

        private JsonData data = null;

        public SerializeEntity() { }
        public SerializeEntity(JsonData data){ this.data = data; }
        public JsonData Data { get { return data; } }

        public SerializeEntity(string json) {
            data = JsonMapper.ToObject(json);
        }

        public string Type
        {
            get {
                if(data == null)
                    return "";
                return data["type"];
            }
            set {
                EnsureJsonData();
                data["type"] = value;
            }
        }

        public void SetJsonData(JsonData data)
        {
            this.data = data;
        }

        public JsonData this[string name]
        {
            get {  return Get(name); }
            set
            {
                EnsureJsonData();
                if (string.IsNullOrEmpty(name))
                {
                    LitLogger.LogFormat("Add Proterty Error => key : {0} , value : {1}", name, value);
                    return;
                }
                else if (data.Contains(name))
                {
                    LitLogger.WarningFormat("The Proterty Exist : {0} ,it will be repalce ", name);
                }
                else
                {
                    data[name] = value;
                }
            }
        }

        /// <summary>
        /// 所有的属性值均以string的方式存储,必须设置类型！！
        /// </summary>
        public SerializeEntity Add(string name, object value)
        {
            this[name] = System.Convert.ToString(value);
            return this;
        }

        public SerializeEntity Add(string name, object value, object defV)
        {
            string sdef = System.Convert.ToString(defV);
            string sv = System.Convert.ToString(value);
            if(sdef != sv)
            {
                this[name] = sv;
            }
            return this;
        }

        public JsonData Get(string name)
        {
            if(data == null || !data.Contains(name))
            {
                LitLogger.WarningFormat("Not Found {0} in JsonData {1}", name, data.ToJson());
                return new JsonData();
            }
            return data[name];
        }

        public JsonData GetOrDef<T>(string name, T t)
        {
            if (data == null || !data.Contains(name))
            {
                return new JsonData(t);
            }
            return data[name];
        }


        public bool Contains(string key)
        {
            return data.Contains(key);
        }

        private void EnsureJsonData()
        {
            if(data == null)
            {
                data = new JsonData();
            }
        }

        public override string ToString()
        {
            EnsureJsonData();
            return data.ToJson();
        }
    }
}
