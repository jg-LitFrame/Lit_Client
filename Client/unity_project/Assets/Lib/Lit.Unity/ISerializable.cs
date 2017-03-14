using UnityEngine;
using System.Collections;

namespace Lit.Unity.UI
{
    public interface ISerializable{

        SerializeObj Serialize();

        void DeSerialize();
    }
}
