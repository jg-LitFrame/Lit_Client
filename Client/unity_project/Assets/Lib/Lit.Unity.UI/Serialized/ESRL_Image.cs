using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lit.Unity;

namespace Lit.Unity.UI
{
    /// <summary>
    /// 扩展序列化Image和RawImage
    /// </summary>
    public static class ESRL_Image {

        public static SerializeEntity Ser_MaskableGraphic(this MaskableGraphic mg)
        {
            SerializeEntity se = new SerializeEntity();
            se.Add("m", SerializeUitls.GetResPath(mg.material))
              .Add("col", SerializeUitls.S_Color(mg.color))
              .Add("RT", mg.raycastTarget);
            return se;
        }
        public static void D_MaskableGraphic(this MaskableGraphic mg, SerializeEntity data)
        {
            mg.color = SerializeUitls.D_Color(data["col"]);
            mg.raycastTarget = data["RT"];
            mg.material = SerializeUitls.LoadObj<Material>(data["m"]);
        }


        /// <summary>
        /// 序列化LitImage
        /// </summary>
        public static SerializeEntity Ser_Image(this LitImage img)
        {
            var se = img.Ser_MaskableGraphic();
            se.Type = "Img";
            se.Add("path", SerializeUitls.GetResPath(img.sprite))
              .Add("PA", img.preserveAspect);
            return se;
        }

        public static void D_Image(this Image img, SerializeEntity data)
        {
            img.D_MaskableGraphic(data);
            img.sprite = SerializeUitls.LoadObj<Sprite>(data["path"]);
            img.preserveAspect = data["PA"];
        }


        /// <summary>
        /// 序列化LitRawImage
        /// </summary>
        public static SerializeEntity Ser_RawImage(this LitRawImage img)
        {
            var se = img.Ser_MaskableGraphic();
            se.Type = "RImg";
            se.Add("path", SerializeUitls.GetResPath(img.texture))
              .Add("UV", SerializeUitls.S_Rect(img.uvRect));
            return se;
        }

        public static void D_RawImage(this LitRawImage img, SerializeEntity data)
        {
            img.D_MaskableGraphic(data);
            img.texture = SerializeUitls.LoadObj<Texture>(data["path"]);
            img.uvRect = SerializeUitls.D_Rect(data["UV"]);
        }

    }

}
