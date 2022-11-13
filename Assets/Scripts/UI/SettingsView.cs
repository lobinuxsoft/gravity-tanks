using UnityEngine;

namespace HNW
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] HolographicButton closeButton;
        [SerializeField] HolographicButton debugButton;
        [SerializeField] DebugView debugView;

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
            popup.Hide(null, () => debugView.Show(), 2);
        }
    }
}