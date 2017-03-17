using UnityEngine;
using System.Collections;
using Lit.Unity;


namespace Lit.Unity.UI {

    public static class Serialize_Transform {

        public static SerializeEntity Serialize(this Transform trans)
        {
            SerializeEntity se = new SerializeEntity();
            if (trans is RectTransform)
                se.Type = "RectTrans";
            else
                se.Type = "Trans";

            if (trans is RectTransform)
            {
                var rTrans = trans as RectTransform;
                se.Add("AnMin", SerializeUitls.S_Vector2(rTrans.anchorMin));
                se.Add("AnMax", SerializeUitls.S_Vector2(rTrans.anchorMax));
                se.Add("pivot", SerializeUitls.S_Vector2(rTrans.pivot));
                se.Add("W", rTrans.rect.width);
                se.Add("H", rTrans.rect.height);
            }
            se.Add("p", SerializeUitls.S_Vector3(trans.localPosition));
            se.Add("r", SerializeUitls.S_Vector3(trans.localEulerAngles));
            se.Add("s", SerializeUitls.S_Vector3(trans.localScale));

            return se;
        }

        public static void DeSerialize(this Transform trans, SerializeEntity data)
        {

            if (trans is RectTransform)
            {
                var rTrans = trans as RectTransform;
                rTrans.anchorMin = SerializeUitls.D_Vector2(data["AnMin"]);
                rTrans.anchorMax = SerializeUitls.D_Vector2(data["AnMax"]);
                rTrans.pivot = SerializeUitls.D_Vector2(data["pivot"]);
                rTrans.sizeDelta = new Vector2(data["W"], data["H"]);
            }
            trans.localPosition = SerializeUitls.D_Vector3(data["p"]);
            trans.localScale = SerializeUitls.D_Vector3(data["s"]);
            trans.localEulerAngles = SerializeUitls.D_Vector3(data["r"]);

        }


        public static SerializeEntity Serialize(this CanvasRenderer render)
        {
            SerializeEntity se = new SerializeEntity();
            se.Type = "CanvasRender";
            return se;
        }

    }
}

