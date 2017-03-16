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
            data.Type = "btn";
            data.Add("AN", audioName)
                .Add("AT", (int)this.animationType)
                .Add("TD", tweenDuration)
                .Add("SF", scaleFactor)
                .Add("CI", clickInterval);
            return data;
        }

        public void DeSerialize(SerializeEntity data)
        {
            this.audioName = data["AN"];
            this.clickInterval = data["CI"];
            this.tweenDuration = data["TD"];
            this.scaleFactor = data["SF"];
            int CA = data["AT"];
            this.animationType = (ClickAnimation)CA;
        }
    }
}
