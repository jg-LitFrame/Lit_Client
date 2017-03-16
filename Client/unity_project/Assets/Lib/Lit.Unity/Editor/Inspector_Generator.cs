using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Lit.Unity{
    [CustomEditor(typeof(LitGenerator))]
    [CanEditMultipleObjects]
    public class Inspector_Generator : Editor {

        private LitSerializeObj litSerObj;
        void OnEnable()
        {
            litSerObj = LitSerializeObj.Create(target);
        }

        public override void OnInspectorGUI()
        {
            litSerObj.Update();
            base.OnInspectorGUI();
            GUILayout.Space(20);
            if (GUILayout.Button("Generate LitObjs"))
            {
                if(target is LitGenerator)
                {
                    var genterator = target as LitGenerator;
                    genterator.Generate();
                }else
                {
                    LitLogger.Error("Generate Error");
                }
            }
            litSerObj.Apply();
        }
    }
}
