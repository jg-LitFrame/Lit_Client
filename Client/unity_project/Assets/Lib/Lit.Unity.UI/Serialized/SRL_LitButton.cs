using UnityEngine;
using System.Collections;
using System;

namespace Lit.Unity.UI
{
    public partial class LitButton : ISerializable
    {
        public SerializeEntity Serialize()
        {
            SerializeEntity data = new SerializeEntity();
            LitButton defBtn = DefValueMgr.Instance.GetDefComp<LitButton>();
	        data.Type = "Btn";
            data.Add("AN", audioName, defBtn.audioName)
                .Add("AT", (int)this.animationType, (int)defBtn.animationType)
                .Add("TD", tweenDuration, defBtn.tweenDuration)
                .Add("SF", scaleFactor, defBtn.scaleFactor)
                .Add("CI", clickInterval, defBtn.clickInterval);
            return data;
        }

        public void DeSerialize(SerializeEntity data)
        {

            SerializeUitls.SetString(ref audioName, data, "AN");
            SerializeUitls.SetFloat(ref clickInterval, data, "AN");
            SerializeUitls.SetFloat(ref tweenDuration, data, "TD");
            SerializeUitls.SetFloat(ref scaleFactor, data, "SF");

            int aType = -1;
            SerializeUitls.SetInt(ref aType, data, "AT");
            if(aType != -1)
            {
                this.animationType = (ClickAnimation)aType;
            }
        }
    }
}
