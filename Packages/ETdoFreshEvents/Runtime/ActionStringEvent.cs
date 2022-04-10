using ETdoFreshExtensionMethods;
using UnityEngine;

namespace ETdoFreshEvents
{
    [CreateAssetMenu(menuName = "Events/ActionStringEvent")]
    public class ActionStringEvent : ActionEvent<int>
    {
        public static ActionStringEvent FindEventInEditor(string eventName) =>
            AssetDatabaseUtil.FindObjectOfType<ActionStringEvent>(eventName);
    }
}