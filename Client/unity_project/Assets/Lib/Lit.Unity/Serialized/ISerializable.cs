using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public interface ISerializable{

        SerializeEntity Serialize();

        void DeSerialize(SerializeEntity data);

    }
}
