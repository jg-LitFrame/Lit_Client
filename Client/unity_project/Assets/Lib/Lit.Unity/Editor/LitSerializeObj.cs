using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Lit.Unity.UI
{

    public class LitSerializeObj {

        private SerializedObject serializedObject;
        private LitSerializeObj(UnityEngine.Object obj) {
            serializedObject = new SerializedObject(obj);
        }
        public static LitSerializeObj Create(UnityEngine.Object obj)
        {
            if(obj == null)
            {
                Debug.LogError("Can not Create LitSerializeObj by null");
                return null;
            }
            return new LitSerializeObj(obj);
        }


        public LitSerializeObj SerializeProperties(params string[] names)
        {
            if(names != null)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    SerializeProperty(names[i]);
                }
            }
            return this;
        }

        public LitSerializeObj SerializeProperty(string name ,GUIContent label = null, params GUILayoutOption[] options)
        {
            SerializedProperty sp = serializedObject.FindProperty(name);
            if (sp == null)
            {
                Debug.LogErrorFormat("Can'not Found Property {0} in {1}", name, serializedObject.targetObject.name);
            }
            else if (label == null)
            {
                PropertyField(sp, options);
            }
            else
            {
                PropertyField(sp, label, options);
            }
            return this;
        }

        public LitSerializeObj PropertyField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUILayout.PropertyField(property, options);
            return this;
        }

        public LitSerializeObj PropertyField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUILayout.PropertyField(property, label, options);
            return this;
        }
    }
}
