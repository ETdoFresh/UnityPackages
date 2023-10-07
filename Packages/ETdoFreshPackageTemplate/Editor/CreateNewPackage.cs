using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ETdoFreshPublishNPM.Editor
{
    public class CreateNewPackage : ScriptableObject
    {
        [SerializeField] public AssemblyDefinitionAsset TemplateAssemblyDefinitionAsset;
        [SerializeField] public TextAsset TemplatePackageJson;
        [SerializeField] public TextAsset TemplateReadme;
        [Header("Package Information")]
        [SerializeField] public string directoryName = "ETdoFreshTemplate";
        [SerializeField] public string name = "com.etdofresh.unitypackages.template";

        public static void CreatePackage()
        {
            var instance = AssetDatabase.FindAssets("t:CreateNewPackage")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<CreateNewPackage>)
                .FirstOrDefault();

            if (!instance)
            {
                Debug.LogError("[CreateNewPackage] Scriptable Object not found");
                return;
            }
            if (!instance.TemplateAssemblyDefinitionAsset)
            {
                Debug.LogError("[CreateNewPackage] TemplateAssemblyDefinitionAsset not found");
                return;
            }
            if (!instance.TemplatePackageJson)
            {
                Debug.LogError("[CreateNewPackage] TemplatePackageJson not found");
                return;
            }
            if (!instance.TemplateReadme)
            {
                Debug.LogError("[CreateNewPackage] TemplateReadme not found");
                return;
            }

            var packagesDirectory = Path.Combine(Application.dataPath, "..", "packages");
            var packageDirectory = Path.Combine(packagesDirectory, instance.directoryName);
            if (Directory.Exists(packageDirectory))
            {
                Debug.LogError($"Package {instance.directoryName} directory already exists");
                return;
            }

            Directory.CreateDirectory(packageDirectory);
            var templateAsmDefPath = AssetDatabase.GetAssetPath(instance.TemplateAssemblyDefinitionAsset);
            var templatePackageJsonPath = AssetDatabase.GetAssetPath(instance.TemplatePackageJson);
            var templateReadmePath = AssetDatabase.GetAssetPath(instance.TemplateReadme);
            var destinationAsmDefPath = Path.Combine(packageDirectory, $"{instance.name}.asmdef");
            var destinationPackageJsonPath = Path.Combine(packageDirectory, "package.json");
            var destinationReadmePath = Path.Combine(packageDirectory, "README.md");
            File.Copy(templateAsmDefPath, destinationAsmDefPath);
            File.Copy(templatePackageJsonPath, destinationPackageJsonPath);
            File.Copy(templateReadmePath, destinationReadmePath);

            FileReplaceText(destinationAsmDefPath,
                "\"name\": \"com.etdofresh.unitypackages.template\",",
                $"\"name\": \"{instance.name}\",");

            FileReplaceText(destinationPackageJsonPath,
                "\"name\": \"com.etdofresh.unitypackages.template\",",
                $"\"name\": \"{instance.name}\",");

            FileReplaceText(destinationPackageJsonPath,
                "\"displayName\": \"ETdoFresh Template\",",
                $"\"displayName\": \"{instance.directoryName}\",");

            FileReplaceText(destinationReadmePath, "ETdoFresh Template", instance.directoryName);

            AssetDatabase.Refresh();
        }

        private static void FileReplaceText(string path, string oldValue, string newValue)
        {
            var contents = File.ReadAllText(path);
            contents = contents.Replace(oldValue, newValue);
            File.WriteAllText(path, contents);
        }

        [MenuItem("Publish NPM/Create New Package")]
        public static void SelectNPMMenuAsset()
        {
            if (AssetDatabase.FindAssets("t:CreateNewPackage").Length > 0)
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<CreateNewPackage>(
                    AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:CreateNewPackage")[0]));
            }
            else
            {
                Debug.LogWarning(
                    "[CreateNewPackage] No CreateNewPackage asset found. [Create a new scriptable object instance of CreateNewPackage]");
            }
        }
    }

    [CustomEditor(typeof(CreateNewPackage))]
    public class CreatePackageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create Package"))
            {
                CreateNewPackage.CreatePackage();
            }
        }
    }
}