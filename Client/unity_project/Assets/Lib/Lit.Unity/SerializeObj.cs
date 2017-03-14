using UnityEngine;
using System.Collections;
using LitJson;

namespace Lit.Unity
{
    public class SerializeObj {

        private JsonData data = null;
        
        public SerializeObj()
        {
            data = new JsonData();
        }
        /// <summary>
        /// 所有的属性值均以string的方式存储
        /// </summary>
        public void Add(string name, object value)
        {
            if(name != null && name != "")
            {
                data[name] = System.Convert.ToString(value);
            }
            else
            {
                LitLogger.LogFormat("Add Proterty Error => key : {0} , value : {1}", name, value);
            }
        }

        public void Get<T>(string name)
        {

        }


    }
}
