using System;
using UnityEngine;

namespace HNW.UI
{
    [RequireComponent(typeof(UIPopup))]
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] HolographicButton homeButton;
        [SerializeField] HolographicButton closeButton;

        UIPopup popup;

        public event Action onHomeButtonClicked;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            homeButton.onClick += OnHomeButtonClicked;
            closeButton.onClick += Hide;
        }

        private void OnDestroy()
        {
            homeButton.onClick -= OnHomeButtonClicked;
            closeButton.onClick -= Hide;
        }

        public void Show()
        {
            Time.timeScale = 0;
            popup.Show();
        }

        private void Hide()
        {
            Time.timeScale = 1;
            popup.Hide();
        }

        private void OnHomeButtonClicked()
        {
            Time.timeScale = 1;
            popup.Hide(onHomeButtonClicked, null, 1);
        }
    }
}