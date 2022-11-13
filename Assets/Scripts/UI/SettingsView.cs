using UnityEngine;

namespace HNW
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] HolographicButton closeButton;

        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();
            closeButton.onClick += Hide;
        }

        private void OnDestroy()
        {
            closeButton.onClick -= Hide;
        }

        public void Show() => popup.Show();

        private void Hide() => popup.Hide();
    }
}