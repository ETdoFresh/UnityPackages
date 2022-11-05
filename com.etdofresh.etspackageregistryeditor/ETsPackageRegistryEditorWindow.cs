using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.ETsPackageUpdater
{
    [Serializable]
    public class PackagesJson
    {
        public PackageData[] packages = Array.Empty<PackageData>();
    }

    [Serializable]
    public class PackageData
    {
        public string packageName;
        public string version;
        public string title;
        public string description;
        public string packageUrl;
    }

    [Serializable]
    public class PackageRow
    {
        public string name;
        public string installedVersion;
        public string latestVersion;
        public string[] versions;
        public int selectedIndex;
    }
    
    public class ETsPackageRegistryEditorWindow : EditorWindow
    {
        private const string url = "https://gist.githubusercontent.com/ETdoFresh/022ecf1a98a84635eb3e113c0c13eaec/raw";
        private string _name;
        private string _version;
        private string _title;
        private string _description;
        private string _packageUrl;
        private int _selectedIndex;
        private PackagesJson _packagesJson;

        [MenuItem("Window/ETs Registry Editor")]
        public static void ShowWindow()
        {
            GetWindow<ETsPackageRegistryEditorWindow>("ETs Registry Editor");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Get Packages"))
            {
                GetPackages();
            }

            if (_packagesJson?.packages != null && _packagesJson.packages.Length > 0)
            {
                _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _packagesJson.packages.Select(x => $"{x.packageName}@{x.version}").ToArray());
                if (GUILayout.Button("Copy Data"))
                {
                    _name = _packagesJson.packages[_selectedIndex].packageName;
                    _version = _packagesJson.packages[_selectedIndex].version;
                    _title = _packagesJson.packages[_selectedIndex].title;
                    _description = _packagesJson.packages[_selectedIndex].description;
                    _packageUrl = _packagesJson.packages[_selectedIndex].packageUrl;
                }
            }
            
            _name = EditorGUILayout.TextField("Name", _name);
            _version = EditorGUILayout.TextField("Version", _version);
            _title = EditorGUILayout.TextField("Title", _title);
            _description = EditorGUILayout.TextField("Description", _description);
            _packageUrl = EditorGUILayout.TextField("Package Url", _packageUrl);
            
            if (GUILayout.Button("Copy New JSON to Clipboard"))
            {
                Debug.Log("Downloading existing packages...");
                var webRequest = UnityWebRequest.Get(url);
                webRequest.SendWebRequest();
                while (!webRequest.isDone) { }
                var packagesJson = JsonUtility.FromJson<PackagesJson>(webRequest.downloadHandler.text);
                var packageData = new PackageData
                {
                    packageName = _name,
                    version = _version,
                    title = _title,
                    description = _description,
                    packageUrl = _packageUrl
                };
                packagesJson.packages = packagesJson.packages.Append(packageData).ToArray();
                var json = JsonUtility.ToJson(packagesJson, true);
                EditorGUIUtility.systemCopyBuffer = json;
                Debug.Log("JSON copied to clipboard");
            }
            if (GUILayout.Button("Open Edit Gist"))
            {
                Application.OpenURL(url.Replace("/raw", "/edit"));
            }
        }

        private void GetPackages()
        {
            var webRequest = UnityWebRequest.Get(url);
            webRequest.SendWebRequest();
            while (!webRequest.isDone) { }
            _packagesJson = JsonUtility.FromJson<PackagesJson>(webRequest.downloadHandler.text);
        } 
    }
}