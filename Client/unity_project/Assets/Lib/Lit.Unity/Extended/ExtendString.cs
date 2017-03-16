using UnityEngine;
using System.Collections;
using System;
namespace Lit.Unity
{

    public static class ExtendString {

        public static bool isEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static int ToInt(this string str, int defV = 0)
        {
            try
            {
                defV = Convert.ToInt32(str);
            }
            catch (System.Exception e)
            {
                LitLogger.Error(e.Message);
            }
            return defV;
        }

        public static float ToFloat(this string str, float defV = 0)
        {
            try
            {
                defV = Convert.ToSingle(str);
            }
            catch (System.Exception e)
            {
                LitLogger.Error(e.Message);
            }
            return defV;
        }

        public static int IndexAtTimes(this string str, char ch, int times)
        {
            if (str == null) return -1;
            int curTime = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if(str[i] == ch)
                {
                    curTime++;
                    if(curTime == times)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

    }

}
