using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ETdoFresh.UnityPackages.EditorTools.Editor
{
    public static class InspectorCollapseAllComponents
    {
        [MenuItem("CONTEXT/Component/Collapse All")]
        private static void CollapseAllInGameObject(MenuCommand command)
        {
            SetAllInspectorsExpanded((command.context as Component).gameObject, false);
        }

        [MenuItem("CONTEXT/Component/Expand All")]
        private static void ExpandAllInGameObject(MenuCommand command)
        {
            SetAllInspectorsExpanded((command.context as Component).gameObject, true);
        }

        [MenuItem("Component/Components/Collapse All")]
        private static void CollapseAll()
        {
            foreach (var gameObject in Object.FindObjectsOfType<GameObject>())
                SetAllInspectorsExpanded(gameObject, false);
        }

        [MenuItem("Component/Components/Expand All")]
        private static void ExpandAll()
        {
            foreach (var gameObject in Object.FindObjectsOfType<GameObject>())
                SetAllInspectorsExpanded(gameObject, true);
        }

        private static void SetAllInspectorsExpanded(GameObject go, bool expanded)
        {
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component is Transform) continue;
                if (component is Renderer renderer)
                {
                    var materials = renderer.sharedMaterials;
                    foreach (var material in materials)
                        InternalEditorUtility.SetIsInspectorExpanded(material, expanded);
                }
                InternalEditorUtility.SetIsInspectorExpanded(component, expanded);
            }
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }
    }
}