using ETdoFreshExtensionMethods;
using UnityEngine;
using UnityEngine.UI;

namespace ETdoFreshEvents.Sample
{
    public class SampleEventManager : MonoBehaviour
    {
        [SerializeField] private SampleActionEvent buttonClick;
        [SerializeField] private Button button;
        [SerializeField] private Text text;
        private int _clickCount;

        private void OnValidate()
        {
            if (!buttonClick) buttonClick = SampleActionEvent.FindEventInEditor("OnButtonClick");
        }

        private void Awake()
        {
            button.onClick.AddPersistentListener(buttonClick.Raise);
            buttonClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemovePersistentListener(buttonClick.Raise);
            buttonClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            text.text = $"Button Clicked {++_clickCount} times";
        }
    }
}