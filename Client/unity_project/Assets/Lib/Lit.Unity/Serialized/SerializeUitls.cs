using UnityEngine;
using System.Collections;

namespace Lit.Unity
{ 
    /// <summary>
    /// 此工具类用于为序列化操作，其中“S”开始的方法是将参数转为string，
    /// “D”开头方法是将string转对应的类型
    /// </summary>
    public static class SerializeUitls {

        #region Vector3
        public static string S_Vector3(Vector3 vector)
        {
            return string.Format("{0},{1},{2}",vector.x,vector.y,vector.z);
        }
        public static Vector3 D_Vector3(string str_vector3)
        {
            Vector3 retV = Vector3.zero;
            var strs = str_vector3.Split(',');
            if (strs.Length > 0)
                retV.x = strs[0].ToFloat();
            if (strs.Length > 1)
                retV.y = strs[1].ToFloat();
            if (strs.Length > 2)
                retV.z = strs[2].ToFloat();
            return retV;
        }
        #endregion


        #region Quaternion
        public static string S_Quaternion(Quaternion q)
        {
            return string.Format("{0},{1},{2},{3}", q.x, q.y, q.z,q.w);
        }
        public static Quaternion D_Quaternion(string str_q)
        {
            Quaternion retV = new Quaternion();
            var strs = str_q.Split(',');
            if (strs.Length > 0)
                retV.x = strs[0].ToFloat();
            if (strs.Length > 1)
                retV.y = strs[1].ToFloat();
            if (strs.Length > 2)
                retV.z = strs[2].ToFloat();
            if (strs.Length > 3)
                retV.w = strs[3].ToFloat();
            return retV;
        }
        #endregion


        #region Vector2
        public static string S_Vector2(Vector2 vect)
        {
            return string.Format("{0},{1}", vect.x, vect.y);
        }
        public static Vector2 D_Vector2(string str_vect)
        {
            Vector2 retV = Vector2.zero;
            var strs = str_vect.Split(',');
            if (strs.Length > 0)
                retV.x = strs[0].ToFloat();
            if (strs.Length > 1)
                retV.y = strs[1].ToFloat();
            return retV;
        }
        #endregion


        #region Color
        public static string S_Color(Color c)
        {
            return string.Format("{0},{1},{2},{3}", c.r, c.g, c.b, c.a);
        }
        public static Color D_Color(string str_c)
        {
            Color retV = new Color();
            var strs = str_c.Split(',');
            if (strs.Length > 0)
                retV.r = strs[0].ToFloat();
            if (strs.Length > 1)
                retV.g = strs[1].ToFloat();
            if (strs.Length > 2)
                retV.b = strs[2].ToFloat();
            if (strs.Length > 3)
                retV.a = strs[3].ToFloat();
            return retV;
        }
        #endregion


        #region Rect
        public static string S_Rect(Rect rect)
        {
            return string.Format("{0},{1},{2},{3}", rect.x, rect.y, rect.width, rect.height);
        }
        public static Rect D_Rect(string str_c)
        {
            Rect retV = new Rect();
            var strs = str_c.Split(',');
            if (strs.Length > 0)
                retV.x = strs[0].ToFloat();
            if (strs.Length > 1)
                retV.y = strs[1].ToFloat();
            if (strs.Length > 2)
                retV.width = strs[2].ToFloat();
            if (strs.Length > 3)
                retV.height = strs[3].ToFloat();
            return retV;
        }
        #endregion


        #region 设置属性值
        public static void SetString(ref string value, SerializeEntity data, string name)
        {
            if (data == null || !data.Contains(name))
                return;
            value = data[name];
        }

        public static void SetFloat(ref float value, SerializeEntity data, string name)
        {
            if (data == null || !data.Contains(name))
                return;
            value = data[name];
        }

        public static void SetInt(ref int value, SerializeEntity data, string name)
        {
            if (data == null || !data.Contains(name))
                return;
            value = data[name];
        }

        public static void SetDouble(ref double value, SerializeEntity data, string name)
        {
            if (data == null || !data.Contains(name))
                return;
            value = data[name];
        }
        public static void SetLong(ref long value, SerializeEntity data, string name)
        {
            if (data == null || !data.Contains(name))
                return;
            value = data[name];
        }

        //public static void SetEnum<T>(ref T value, SerializeEntity data, string name) where T : System.IConvertible
        //{
        //    if (data == null || !data.Contains(name))
        //        return;
        //    int _v = data[name];
        //    value = T.C;
        //}

        #endregion



        #region "资源处理"
        public static string GetResPath(Object obj)
        {
            if (obj == null) return "";
#if UNITY_EDITOR
            string fullPath =  UnityEditor.AssetDatabase.GetAssetPath(obj);
            int startIndex = fullPath.IndexAtTimes('/', 2) + 1;
            int endIndex = fullPath.IndexOf(".");
            if(startIndex <0 || endIndex <0 || startIndex >= endIndex)
            {
                LitLogger.WarningFormat("Not Found Res {0}", obj);
                return "";
            }
            return fullPath.Substring(startIndex, endIndex - startIndex);
#endif
            return "";
        }

        public static T LoadObj<T>(string path) where T : UnityEngine.Object
        {
            if (path.isEmpty())
                return null;
            return Resources.Load<T>(path);
        }
        #endregion

    }
}
