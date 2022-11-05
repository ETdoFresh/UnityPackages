using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.etdofresh.etspackageeditor
{
    [Serializable]
    public class PackageJson
    {
        public string name = "com.etdofresh.template";
        public string version = "0.0.1";
        public string displayName = "Template Package Name";
        public string description = "Template Package Description";
        public string documentationUrl = "https://example.com/";
        public string changelogUrl = "https://example.com/changelog.html";
        public string licenseUrl = "https://example.com/licensing.html";
        public Author author = new Author();
    }

    [Serializable]
    public class Author
    {
        public string name = "ETdoFresh";
        public string email = "ETdoFresh@gmail.com";
        public string url = "https://www.etdofresh.com";
    }

    [Serializable]
    public class AssemblyDefinitionJson
    {
        public string name = "com.etdofresh.template";
        public string rootNamespace = "";
        public string[] references = Array.Empty<string>();
        public string[] includePlatforms = new string[] { "Editor" };
        public string[] excludePlatforms = Array.Empty<string>();
        public bool allowUnsafeCode = false;
        public bool overrideReferences = false;
        public string[] precompiledReferences = Array.Empty<string>();
        public bool autoReferenced = true;
        public string[] defineConstraints = Array.Empty<string>();
        public string[] versionDefines = Array.Empty<string>();
        public bool noEngineReferences = false;
    }

    public class PackageEditorWindow : EditorWindow
    {
        private enum PackageEditorMode { None, Create, Localize }

        private static PackageEditorMode _mode = PackageEditorMode.None;
        private static PackageJson _createPackage = new PackageJson();

        private static string PackageDirectory => Path.Combine(Directory.GetCurrentDirectory(), "Packages");
        private static string PluginsDirectory => Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Plugins");

        [MenuItem("Window/ETs Package Editor")]
        public static void ShowWindow()
        {
            GetWindow<PackageEditorWindow>("Package Editor");
        }

        private void OnGUI()
        {
            ShowCreatePackageButton();
            ShowLocalizePackageButton();
            EditorGUILayout.Space();
            ShowCreatePackageGUI();
            ShowLocalizePackageGUI();
            
            if (GUILayout.Button("Open UnityPackages GitHub"))
            {
                Application.OpenURL("https://github.com/ETdoFresh/UnityPackages");
            }
        }

        private void ShowCreatePackageButton()
        {
            EditorGUI.BeginDisabledGroup(_mode == PackageEditorMode.Create);
            if (GUILayout.Button("Create Package"))
            {
                _mode = PackageEditorMode.Create;
            }

            EditorGUI.EndDisabledGroup();
        }

        private void ShowLocalizePackageButton()
        {
            EditorGUI.BeginDisabledGroup(_mode == PackageEditorMode.Localize);
            if (GUILayout.Button("Localize Package"))
            {
                _mode = PackageEditorMode.Localize;
            }

            EditorGUI.EndDisabledGroup();
        }

        private void ShowCreatePackageGUI()
        {
            if (_mode != PackageEditorMode.Create) return;
            _createPackage.name = EditorGUILayout.TextField("Name", _createPackage.name);
            _createPackage.version = EditorGUILayout.TextField("Version", _createPackage.version);
            _createPackage.displayName = EditorGUILayout.TextField("Title", _createPackage.displayName);
            _createPackage.description = EditorGUILayout.TextField("Description", _createPackage.description);
            _createPackage.documentationUrl =
                EditorGUILayout.TextField("Documentation Url", _createPackage.documentationUrl);
            _createPackage.changelogUrl = EditorGUILayout.TextField("Changelog Url", _createPackage.changelogUrl);
            _createPackage.licenseUrl = EditorGUILayout.TextField("License Url", _createPackage.licenseUrl);
            _createPackage.author.name = EditorGUILayout.TextField("Author Name", _createPackage.author.name);
            _createPackage.author.email = EditorGUILayout.TextField("Author Email", _createPackage.author.email);
            _createPackage.author.url = EditorGUILayout.TextField("Author Url", _createPackage.author.url);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create in Packages"))
            {
                CreatePackage(_createPackage, PackageDirectory);
                _mode = PackageEditorMode.None;
            }
            if (GUILayout.Button("Create in Plugins"))
            {
                CreatePackage(_createPackage, PluginsDirectory);
                _mode = PackageEditorMode.None;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowLocalizePackageGUI()
        {
            if (_mode != PackageEditorMode.Localize) return;

            var libraryPackageCacheDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Library", "PackageCache");
            var packageDirectories = Directory.GetDirectories(libraryPackageCacheDirectory)
                .Where(x => !x.Contains("com.unity")).Select(x => x).ToArray();

            var packageNames = packageDirectories.Select(x => x.Split(Path.DirectorySeparatorChar).Last()).ToArray();
            foreach (var packageName in packageNames)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(packageName);
                if (GUILayout.Button("Localize to Packages"))
                {
                    LocalizePackage(packageName);
                }

                if (GUILayout.Button("Localize to Plugins"))
                {
                    LocalizePackageToPlugins(packageName);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void CreatePackage(PackageJson newPackage, string packageDirectory)
        {
            Debug.Log("Creating package: " + newPackage.name);
            if (!Directory.Exists(packageDirectory))
                Directory.CreateDirectory(packageDirectory);

            var packagePath = Path.Combine(packageDirectory, newPackage.name);
            if (!Directory.Exists(packagePath))
                Directory.CreateDirectory(packagePath);

            Debug.Log("Writing package.json");
            var packageJsonPath = Path.Combine(packagePath, "package.json");
            if (!File.Exists(packageJsonPath))
            {
                var json = JsonUtility.ToJson(newPackage, true);
                File.WriteAllText(packageJsonPath, json);
            }

            Debug.Log($"Writing {newPackage.name}.asmdef");
            var asmdefPath = Path.Combine(packagePath, $"{newPackage.name}.asmdef");
            if (!File.Exists(asmdefPath))
            {
                var asmdef = new AssemblyDefinitionJson
                {
                    name = newPackage.name,
                };
                var json = JsonUtility.ToJson(asmdef, true);
                File.WriteAllText(asmdefPath, json);
            }

            Debug.Log($"Writing README.md");
            var readmePath = Path.Combine(packagePath, "README.md");
            if (!File.Exists(readmePath))
            {
                var readme = $"# {newPackage.displayName}\n\n{newPackage.description}";
                File.WriteAllText(readmePath, readme);
            }

            Debug.Log($"Finished creating package: {newPackage.name}");

            AssetDatabase.Refresh();
        }

        private void LocalizePackage(string packageName)
        {
            Debug.Log($"Localizing package: {packageName}");
            var packagePath = Path.Combine(Directory.GetCurrentDirectory(), "Library", "PackageCache", packageName);
            if (!Directory.Exists(packagePath))
            {
                Debug.LogError($"Package not found: {packageName}");
                return;
            }

            var newPackagePath = Path.Combine(PackageDirectory, packageName);
            
            // trying to move com.etdofresh.compiletime to Packages/com.etdofresh.compiletime fails (false positive?)
            if (Directory.Exists(newPackagePath))
            {
                Debug.LogError($"Package Directory already exists: {newPackagePath}");
                return;
            }

            Directory.Move(packagePath, newPackagePath);
            
            AssetDatabase.Refresh();
        }

        private void LocalizePackageToPlugins(string packageName)
        {
            Debug.Log($"Localizing package: {packageName}");
            var packagePath = Path.Combine(Directory.GetCurrentDirectory(), "Library", "PackageCache", packageName);
            if (!Directory.Exists(packagePath))
            {
                Debug.LogError($"Package not found: {packageName}");
                return;
            }

            var packageNameWithoutCommit = packageName.Split('@').First();
            var newPackagePath = Path.Combine(PluginsDirectory, packageNameWithoutCommit);
            if (Directory.Exists(newPackagePath)) // False positive?
            {
                Debug.LogError($"Package already exists: {packageNameWithoutCommit}");
                return;
            }

            Directory.Move(packagePath, newPackagePath);

            UnityEditor.PackageManager.Client.Remove(packageNameWithoutCommit);

            AssetDatabase.Refresh();
        }
    }
}