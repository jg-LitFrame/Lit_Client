using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lit.Unity;


namespace Lit.Unity.UI
{
    public partial class  LitImage : Image {

        [ContextMenu("TestImagePath")]
        public void TestImagePath()
        {
            LitLogger.Log(SerializeUitls.GetResPath(sprite));
        }
    }

}
