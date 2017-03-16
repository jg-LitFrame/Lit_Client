using UnityEngine;
using System.Collections;

namespace Lit.Unity
{ 
    /// <summary>
    /// 此工具类用于为序列化操作，其中“SRL”开始的方法是将参数转为string，
    /// “DSRL”开头方法是将string转对应的类型
    /// </summary>
    public static class SerializeUitls {

        #region Vector3
        public static string SRL_Vector3(Vector3 vector)
        {
            return string.Format("{0},{1},{2}",vector.x,vector.y,vector.z);
        }
        public static Vector3 DSRL_Vector3(string str_vector3)
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
        public static string SRL_Quaternion(Quaternion q)
        {
            return string.Format("{0},{1},{2},{3}", q.x, q.y, q.z,q.w);
        }
        public static Quaternion DSRL_Quaternion(string str_vector3)
        {
            Quaternion retV = new Quaternion();
            var strs = str_vector3.Split(',');
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



        public static string GetResPath(Object obj)
        {
#if UNITY_EDITOR
            string fullPath =  UnityEditor.AssetDatabase.GetAssetPath(obj);
            int startIndex = fullPath.IndexAtTimes('/', 2) + 1;
            int endIndex = fullPath.IndexOf(".");
            if(startIndex <0 || endIndex <0 || startIndex < endIndex)
            {
                LitLogger.ErrorFormat("Not Found Res {0}", obj);
                return "";
            }
            return fullPath.Substring(startIndex, endIndex - startIndex);
#endif
            return "";
        }

    }
}
