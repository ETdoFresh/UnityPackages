using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ETdoFreshEditorTools.Editor
{
    public static class CreateScriptableObjectScriptContextMenu
    {
        [MenuItem("Assets/Create/Scriptable Object Asset", true)]
        public static bool CreateScriptableObjectValidate()
        {
            var activeObject = Selection.activeObject;
            if (activeObject == null) return false;
            if (!(activeObject is TextAsset)) return false;
            
            var assetPath = AssetDatabase.GetAssetPath(activeObject);
            if (assetPath == null) return false;
            
            var monoScript = (MonoScript)AssetDatabase.LoadAssetAtPath(assetPath, typeof(MonoScript));
            if (monoScript == null) return false;
            
            var scriptType = monoScript.GetClass();
            if (scriptType == null) return false;
            if (!typeof(ScriptableObject).IsAssignableFrom(scriptType)) return false;

            return true;
        }
        
        [MenuItem("Assets/Create/Scriptable Object Asset", false, -10)]
        public static void CreateScriptableObject()
        {
            var activeObject = Selection.activeObject;
            var assetPath = AssetDatabase.GetAssetPath(activeObject);
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
            CreateNewInstance(assetPath, script);
        }

        private static void CreateNewInstance(string assetPath, MonoScript script)
        {
            var scriptType = script.GetClass();
            var path = Path.Combine(Path.GetDirectoryName(assetPath) ?? string.Empty, scriptType.Name + ".asset");
            try
            {
                var inst = ScriptableObject.CreateInstance(scriptType);
                AssetDatabase.CreateAsset(inst, path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
