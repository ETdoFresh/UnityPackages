using ETdoFreshExtensionMethods;

namespace ETdoFreshEvents.Sample
{
    internal class SampleActionEvent : ActionEvent
    {
        public static SampleActionEvent FindEventInEditor(string eventName) =>
            AssetDatabaseUtil.FindObjectOfType<SampleActionEvent>(eventName);
    }
}
