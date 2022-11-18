using GooglePlayGames;
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
        [SerializeField] HolographicButton achievementsButton;
        [SerializeField] HolographicButton leaderboardButton;

        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            playButton.onClick += OnPlayClicked;
            shopButton.onClick += OnShopClicked;
            statsButton.onClick += OnStatsClicked;
            achievementsButton.onClick += OnAchievementsClicked;
            leaderboardButton.onClick += OnLeaderboardClicked;
        }

        private void OnDestroy()
        {
            playButton.onClick -= OnPlayClicked;
            shopButton.onClick -= OnShopClicked;
            statsButton.onClick -= OnStatsClicked;
            achievementsButton.onClick -= OnAchievementsClicked;
            leaderboardButton.onClick -= OnLeaderboardClicked;
        }

        private void OnPlayClicked()
        {
            TimelineUITransitionScene.Instance.FadeStart(gameplayScene, 1, fadeIn, fadeOut);
        }

        private void OnShopClicked()
        {
            throw new NotImplementedException();
        }

        private void OnStatsClicked()
        {
            throw new NotImplementedException();
        }

        private void OnAchievementsClicked()
        {
            #if UNITY_ANDROID
            PlayGamesPlatform.Instance.ShowAchievementsUI();
            #endif
        }

        private void OnLeaderboardClicked()
        {
            #if UNITY_ANDROID
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
            #endif
        }
    }
}