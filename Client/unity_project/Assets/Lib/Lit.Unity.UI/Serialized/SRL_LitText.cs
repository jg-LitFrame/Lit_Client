using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Lit.Unity;

namespace Lit.Unity.UI
{
    public partial class LitText : ISerializable
    {

        public SerializeEntity Serialize()
        {
            SerializeEntity se = new SerializeEntity();
            se.Type = "Text";
            se.Add("info", text);
            //TODO 字体和字体暂时不支持序列化，后期自定义字体时再做支持
            se.Add("FStyle", fontStyle.ToString())
               .Add("FSize", fontSize)
               .Add("LS", lineSpacing)
               .Add("RT", supportRichText)
               .Add("AG", alignment.ToString())
               .Add("ABG", alignByGeometry)
               .Add("HO", horizontalOverflow.ToString())
               .Add("VO", verticalOverflow.ToString())
               .Add("col", SerializeUitls.S_Color(color))
               .Add("RayT",raycastTarget)
               .Add("BF", resizeTextForBestFit);
            if (resizeTextForBestFit)
            {
                se.Add("MinS", resizeTextMinSize)
                  .Add("MaxS", resizeTextMaxSize);
            }
            return se;
        }

        public void DeSerialize(SerializeEntity data)
        {
            text = data["info"];
            fontStyle = (FontStyle)System.Enum.Parse(typeof(FontStyle), data["FStyle"]);
            fontSize = data["FSize"];
            lineSpacing = data["LS"];
            supportRichText = data["RT"];
            alignment = (TextAnchor)System.Enum.Parse(typeof(TextAnchor), data["AG"]);
            alignByGeometry = data["ABG"];
            horizontalOverflow = (HorizontalWrapMode)System.Enum.Parse(typeof(HorizontalWrapMode), data["HO"]);
            verticalOverflow = (VerticalWrapMode)System.Enum.Parse(typeof(VerticalWrapMode), data["VO"]);
            color = SerializeUitls.D_Color(data["col"]);
            raycastTarget = data["RayT"];
            resizeTextForBestFit = data["BF"];
            if (resizeTextForBestFit)
            {
                resizeTextMinSize = data["MinS"];
                resizeTextMaxSize = data["MaxS"];
            }

        }

    }
}
