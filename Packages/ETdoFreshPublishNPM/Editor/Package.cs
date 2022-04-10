using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ETdoFreshPublishNPM.Editor
{
    [Serializable]
    public class Package
    {
        public string name;
        public string version;
        public string displayName;
        public string description;
        public string path;
        public string packageJsonPath;
    }

    [CustomPropertyDrawer(typeof(Package))]
    public class PackagePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 1;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var name = property.FindPropertyRelative("name");
            var version = property.FindPropertyRelative("version");
            var path = property.FindPropertyRelative("path");
            var packageJsonPath = property.FindPropertyRelative("packageJsonPath");
            var menu = (PublishNPMMenu) property.serializedObject.targetObject;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var nameRect = new Rect(position.x, position.y, position.width * 0.5f, position.height);
            var publishButtonRect = new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f,
                position.height);

            EditorGUI.LabelField(nameRect, name.stringValue);
            if (GUI.Button(publishButtonRect, "Publish"))
                if (NPMProcessRunner.Run(path.stringValue, menu.RegistryPath))
                {
                    var previousVersion = version.stringValue;
                    version.stringValue = IncrementVersion(previousVersion, packageJsonPath.stringValue);
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                    Debug.Log($"[Package] Publish {previousVersion} Success!");
                }

            EditorGUI.indentLevel = indent;
        }

        private static string IncrementVersion(string previousVersion, string packageJsonPath)
        {
            var split = previousVersion.Split('.');
            var last = int.Parse(split[split.Length - 1]);
            last++;
            split[split.Length - 1] = last.ToString();
            var currentVersion = string.Join(".", split);
            
            var text = File.ReadAllText(packageJsonPath);
            text = text.Replace($"\"version\": \"{previousVersion}\",",
                $"\"version\": \"{currentVersion}\",");
            File.WriteAllText(packageJsonPath, text);
            
            return currentVersion;
        }
    }
}