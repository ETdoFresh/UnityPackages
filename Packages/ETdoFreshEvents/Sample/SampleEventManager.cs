using ETdoFresh.UnityPackages.ExtensionMethods;
using UnityEngine;
using UnityEngine.UI;

namespace ETdoFresh.UnityPackages.Events.Sample
{
    public class SampleEventManager : MonoBehaviour
    {
        [SerializeField] private SampleEvent buttonClick;
        [SerializeField] private Button button;
        [SerializeField] private Text text;
        private int _clickCount;

        private void OnValidate()
        {
            if (!buttonClick) buttonClick = AssetDatabaseUtil.FindObjectOfType<SampleEvent>("OnButtonClick");
        }

        private void Awake()
        {
            button.onClick.AddPersistentListener(buttonClick.Invoke);
            buttonClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemovePersistentListener(buttonClick.Invoke);
            buttonClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            text.text = $"Button Clicked {++_clickCount} times";
        }
    }
}