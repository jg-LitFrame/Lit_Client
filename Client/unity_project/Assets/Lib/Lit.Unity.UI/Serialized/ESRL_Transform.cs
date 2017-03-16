using UnityEngine;
using System.Collections;
using Lit.Unity;


namespace Lit.Unity.UI {

    public static class Serialize_Transform {

        public static SerializeEntity Serialize(this Transform trans)
        {
            SerializeEntity se = new SerializeEntity();
            se.Type = "Trans";
            se.Add("p", SerializeUitls.SRL_Vector3(trans.localPosition));
            se.Add("r", SerializeUitls.SRL_Vector3(trans.localEulerAngles));
            se.Add("s", SerializeUitls.SRL_Vector3(trans.localScale));

            return se;
        }

        public static void DeSerialize(this Transform trans, SerializeEntity data)
        {
            Vector3 pos = SerializeUitls.DSRL_Vector3(data["p"]);
            Vector3 rAngle = SerializeUitls.DSRL_Vector3(data["r"]);
            Vector3 scale = SerializeUitls.DSRL_Vector3(data["s"]);

            trans.localPosition = pos;
            trans.localEulerAngles = rAngle;
            trans.localScale = scale;

        }



    }
}

