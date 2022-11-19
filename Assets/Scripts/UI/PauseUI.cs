using System;
using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(UIPopup))]
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] HolographicButton homeButton;
        [SerializeField] HolographicButton closeButton;
        [SerializeField] HolographicButton debugButton;

        UIPopup popup;

        public event Action onHomeButtonClicked;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            homeButton.onClick += OnHomeButtonClicked;
            closeButton.onClick += Hide;
            debugButton.onClick += ShowDebugLogs;

            DebugView.onCloseDebug += OnCloseDebug;
        }


        private void OnDestroy()
        {
            homeButton.onClick -= OnHomeButtonClicked;
            closeButton.onClick -= Hide;
            debugButton.onClick -= ShowDebugLogs;

            DebugView.onCloseDebug -= OnCloseDebug;
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

        private void ShowDebugLogs()
        {
            DebugView.Instance.Show();
            popup.Hide();
        }

        private void OnCloseDebug() => Show();
    }
}