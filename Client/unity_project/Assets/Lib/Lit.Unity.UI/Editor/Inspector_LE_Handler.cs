using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace Lit.Unity.UI
{
    [CustomEditor(typeof(LE_Handler))]
    [CanEditMultipleObjects]
    public class Inspector_LE_Handler : Editor
    {
        private LitSerializeObj litSerObj;
        private LE_Handler handler = null;
        private char[] InvalidChars = { ' ', ',' , '!','/','\\'};
        void OnEnable()
        {
            litSerObj = LitSerializeObj.Create(target);
            handler = target as LE_Handler;
        }
        public override void OnInspectorGUI()
        {
            litSerObj.Update();
            litSerObj.SerializeProperties(
                "script_path"
            );
            if(!File.Exists(FileTools.EventHandleLuaPath + handler.script_path.Replace('.', '/') + ".lua"))
            {
                GUILayout.Space(20);
                DrawCreateBtn();
            }
            litSerObj.Apply();
        }

        private void DrawCreateBtn()
        {
            if (GUILayout.Button("Create Handle File"))
            {
                if(!isValidPaht(handler.script_path))
                {
                    LitLogger.ErrorFormat("Invalid Path : {0}", handler.script_path);
                }
                string full_path = string.Concat(FileTools.EventHandleLuaPath, handler.script_path.Replace('.','/'), ".lua");
                EnsureFold(full_path);
                CreateFile(full_path);
                AssetDatabase.Refresh();
            }
        }

        private void CreateFile(string full_path)
        {
            string templateFilePath = FileTools.ClientDataPath + "UITemplate.lua";
            var text = File.ReadAllText(templateFilePath);
            string module_name = handler.script_path.Replace('.', '_');
            var newText = text.Replace("{module_name}", module_name);
            File.WriteAllText(full_path, newText);
        }

        private void EnsureFold(string full_path)
        {
            string dictPath = Directory.GetParent(full_path).FullName;
            if(!Directory.Exists(full_path))
                Directory.CreateDirectory(dictPath);
        }
        private bool isValidPaht(string path)
        {
            bool valid = !string.IsNullOrEmpty(handler.script_path);
            for (int i = 0; i < InvalidChars.Length; i++)
            {
                valid = valid && !path.Contains(InvalidChars[i] + "");
            }
            return valid;
        }
    }
}
