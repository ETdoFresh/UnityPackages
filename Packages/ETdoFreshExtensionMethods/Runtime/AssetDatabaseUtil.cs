using System.Linq;
using UnityEngine;

namespace ETdoFresh.UnityPackages.ExtensionMethods
{
    public static class AssetDatabaseUtil
    {
        public static T FindObjectOfType<T>(string name) where T : Object
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase
                .FindAssets($"t:{typeof(T).Name} {name}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<T>)
                .FirstOrDefault();
#else
        return default;
#endif
        }
    }
}