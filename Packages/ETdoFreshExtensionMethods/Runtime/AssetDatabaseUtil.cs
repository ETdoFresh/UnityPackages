using System.Linq;
using UnityEngine;

namespace ETdoFreshExtensionMethods
{
    public static class AssetDatabaseUtil
    {
        public static T FindObjectOfType<T>(string eventName) where T : Object
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<T>)
                .FirstOrDefault(x => x.name == eventName);
#else
        return default;
#endif
        }
    }
}