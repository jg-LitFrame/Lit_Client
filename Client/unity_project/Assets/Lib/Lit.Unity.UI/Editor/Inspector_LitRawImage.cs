using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Lit.Unity.UI
{
    [CustomEditor(typeof(LitImage))]
    [CanEditMultipleObjects]
    public class Inspector_LitRawImage : Editor {

        private LitSerializeObj litSerObj;
        void OnEnable()
        {
            litSerObj = LitSerializeObj.Create(target);
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
