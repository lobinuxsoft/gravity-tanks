using System;
using UnityEngine;

namespace HNW
{
    public class CoreLoopView : MonoBehaviour
    {
        [SerializeField] string gameplayScene = "Gameplay";
        [SerializeField] Gradient fadeIn;
        [SerializeField] Gradient fadeOut;
        [SerializeField] HolographicButton playButton;
        [SerializeField] HolographicButton shopButton;
        [SerializeField] HolographicButton statsButton;
        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            playButton.onClick += OnPlayClicked;
            shopButton.onClick += OnShopClicked;
            statsButton.onClick += OnStatsClicked;
        }

        private void OnDestroy()
        {
            playButton.onClick -= OnPlayClicked;
            shopButton.onClick -= OnShopClicked;
            statsButton.onClick -= OnStatsClicked;
        }

        private void OnPlayClicked()
        {
            TimelineUITransition.Instance.FadeStart(gameplayScene, 1, fadeIn, fadeOut);
        }

        private void OnShopClicked()
        {
            throw new NotImplementedException();
        }

        private void OnStatsClicked()
        {
            throw new NotImplementedException();
        }
    }
}