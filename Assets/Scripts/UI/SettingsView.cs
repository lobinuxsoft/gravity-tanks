using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(UIPopup))]
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] HolographicButton closeButton;
        [SerializeField] HolographicButton debugButton;

        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            closeButton.onClick += Hide;
            debugButton.onClick += ShowDebugLogs;
        }

        private void OnDestroy()
        {
            closeButton.onClick -= Hide;
            debugButton.onClick -= ShowDebugLogs;
        }

        public void Show() => popup.Show();

        private void Hide() => popup.Hide();

        private void ShowDebugLogs()
        {
            popup.Hide(null, () => DebugView.Instance.Show(), 2);
        }
    }
}