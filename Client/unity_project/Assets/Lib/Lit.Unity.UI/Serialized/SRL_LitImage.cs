using UnityEngine;
using System.Collections;
using Lit.Unity;
using System;
using UnityEngine.UI;

namespace Lit.Unity.UI
{
    public partial class LitImage : ISerializable {

        public void DeSerialize(SerializeEntity data)
        {
        }

        public SerializeEntity Serialize()
        {
            SerializeEntity se = new SerializeEntity();
            
            return se;
        }

        public void Test()
        {

        }

    }

}
