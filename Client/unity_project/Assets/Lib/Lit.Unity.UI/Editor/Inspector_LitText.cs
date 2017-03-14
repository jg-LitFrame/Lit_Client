using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Lit.Unity.UI
{
    [CustomEditor(typeof(LitText))]
    [CanEditMultipleObjects]
    public class Inspector_LitText : Editor
    {
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
