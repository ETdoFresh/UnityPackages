using ETdoFreshExtensionMethods;
using UnityEngine;

namespace ETdoFreshEvents
{
    [CreateAssetMenu(menuName = "Events/ActionIntEvent")]
    public class ActionIntEvent : ActionEvent<int>
    {
        public static ActionIntEvent FindEventInEditor(string eventName) =>
            AssetDatabaseUtil.FindObjectOfType<ActionIntEvent>(eventName);
    }
}