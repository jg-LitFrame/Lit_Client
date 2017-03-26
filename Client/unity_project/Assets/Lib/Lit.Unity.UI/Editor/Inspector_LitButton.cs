using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Lit.Unity.UI
{
    [CustomEditor(typeof(LitButton))]
    [CanEditMultipleObjects]
    public class Inspector_LitButton : Editor
    {
        private LitSerializeObj litSerObj;
        void OnEnable()
        {
            litSerObj = LitSerializeObj.Create(target);
        }
        public override void OnInspectorGUI()
        {
            litSerObj.Update();
            litSerObj.SerializeProperties(
                "audioName",
                "animationType",
                "scaleFactor",
                "clickInterval",
                "tweenDuration"
            );
            litSerObj.Apply();
        }
    }

}
