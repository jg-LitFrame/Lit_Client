using UnityEngine;
using System.Collections;
namespace Lit.Unity
{
    public class LitLogger{

        private static _D_Void<object> _D_LogNothing = (o) => { };
        private static _D_Void_Params _FormatNothing = (a, b) => { };
        
        #region  Debug 重构
        public static _D_Void<object> Log
        {
            get{ return Debug.Log;}
        }
        public static _D_Void<object> Warning
        {
            get { return Debug.LogWarning; }
        }
        public static _D_Void<object> Error
        {
            get{ return Debug.LogError; }
        }
        #endregion

        #region Debug.LogFormat 重构
        public static _D_Void_Params LogFormat
        {
            get { return Debug.LogFormat; }
        }
        public static _D_Void_Params WarningFormat
        {
            get { return Debug.LogWarningFormat; }
        }
        public static _D_Void_Params ErrorFormat
        {
            get { return Debug.LogErrorFormat; }
        }
        #endregion

    }
}
