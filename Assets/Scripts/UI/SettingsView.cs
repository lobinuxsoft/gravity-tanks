using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(UIPopup))]
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] HolographicButton closeButton;
        [SerializeField] HolographicButton creditsButton;
        [SerializeField] CreditsView creditsView;

        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            closeButton.onClick += Hide;
            creditsButton.onClick += ShowCredits;
        }

        private void OnDestroy()
        {
            closeButton.onClick -= Hide;
        }

        public void Show() => popup.Show();

        private void Hide() => popup.Hide();

        private void ShowCredits() => popup.Hide(()=> creditsView.Show());
    }
}