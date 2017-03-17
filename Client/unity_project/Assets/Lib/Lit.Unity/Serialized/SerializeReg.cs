using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lit.Unity.UI;

namespace Lit.Unity
{
    public static class SerializeReg {

        private static Dictionary<string, Type> allSerializeType = new Dictionary<string, Type>{
            { "Trans",typeof(Transform) },
            { "RectTrans",typeof(RectTransform) },
            { "CanvasRender",typeof(CanvasRenderer) },
            { "Btn",typeof(LitButton) },
            { "Text",typeof(LitText) },
            { "Img",typeof(LitImage) },
            { "RImg",typeof(LitRawImage) },

        };

        #region TODO 如果没有继承ISerializable接口，需要手动调用一下对应的扩展方法
        public static SerializeEntity Serialize(object obj)
        {
            if(obj is Transform)
            {
                return (obj as Transform).Serialize();
            }
            else if(obj is CanvasRenderer)
            {
                return (obj as CanvasRenderer).Serialize();
            }
            else if(obj is LitImage)
            {
                return (obj as LitImage).Ser_Image();
            }
            else if(obj is LitRawImage)
            {
                return (obj as LitRawImage).Ser_RawImage();
            }
            return null;
        }

        public static bool DeSerialize(object obj, SerializeEntity data)
        {
            bool ret = false;
            if (obj is Transform)
            {
                (obj as Transform).DeSerialize(data);
                ret = true;
            }
            else if (obj is LitImage)
            {
                (obj as LitImage).D_Image(data);
            }
            else if (obj is LitRawImage)
            {
                (obj as LitRawImage).D_RawImage(data);
            }
            return ret;
        }

        #endregion


        #region 获取注册类型
        public static Type GetType(string name)
        {
            Type t = null;
            allSerializeType.TryGetValue(name,out t);
            if(t == null)
                LitLogger.ErrorFormat("The Type not resgister {0}", name);
            return t;
        }

        public static bool HasRegType(string type)
        {
            return allSerializeType.ContainsKey(type);
        }
        #endregion
    }
}
