using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ETdoFreshPublishNPM.Editor
{
    public class PublishNPMMenu : ScriptableObject
    {
        [SerializeField] private string registryPath = "http://192.168.254.108:4873/";
        [SerializeField] private List<Package> packages = new List<Package>();

        public string RegistryPath => registryPath;

        private void OnValidate()
        {
            packages = new List<Package>();
            var packagesPath = Application.dataPath + "/../packages";
            var directories = Directory.GetDirectories(packagesPath);
            foreach (var directory in directories)
            {
                var packageJsonPath = directory + "/package.json";
                if (!File.Exists(packageJsonPath)) continue;
                var packageJson = File.ReadAllText(packageJsonPath);
                var package = JsonUtility.FromJson<Package>(packageJson);
                package.path = directory;
                package.packageJsonPath = packageJsonPath;
                packages.Add(package);
            }
        }

        [MenuItem("Publish NPM/Select Packages")]
        public static void SelectNPMMenuAsset()
        {
            if (AssetDatabase.FindAssets("t:PublishNPMMenu").Length > 0)
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<PublishNPMMenu>(
                    AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:PublishNPMMenu")[0]));
            }
            else
            {
                Debug.LogWarning(
                    "[PublishNPMMenu] No PublishNPMMenu asset found. [Create a new scriptable object instance of PublishNPMMenu]");
            }
        }
    }
}