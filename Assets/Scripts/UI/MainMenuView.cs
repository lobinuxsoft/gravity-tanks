using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace HNW
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] string nextSceneName = "Gameplay";
        [SerializeField] Gradient fadeIn;
        [SerializeField] Gradient fadeOut;
        [SerializeField] HolographicButton playButton;
        [SerializeField] HolographicButton settingsButton;
        [SerializeField] HolographicButton achievementsButton;
        [SerializeField] HolographicButton leaderboardButton;
        [SerializeField] SettingsView settingsView;

        private void Awake()
        {
            playButton.onClick += OnPlayClicked;
            settingsButton.onClick += OnSettingsClicked;
            achievementsButton.onClick += OnAchievementsClicked;
            leaderboardButton.onClick += OnLeaderboardClicked;
        }

        private void OnDestroy()
        {
            playButton.onClick -= OnPlayClicked;
            settingsButton.onClick -= OnSettingsClicked;
            achievementsButton.onClick -= OnAchievementsClicked;
            leaderboardButton.onClick -= OnLeaderboardClicked;
        }

        private void Start()
        {
            #if UNITY_ANDROID
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            #endif
        }

        private void OnPlayClicked()
        {
            TimelineUITransitionScene.Instance.FadeStart(nextSceneName, 1.5f, fadeIn, fadeOut);
        }

        private void OnSettingsClicked() => settingsView.Show();

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

        #if UNITY_ANDROID
        void ProcessAuthentication(SignInStatus status)
        {
            Debug.Log($"Authentication statuc: {status.ToString()}");

            switch (status)
            {
                case SignInStatus.Success:
                    PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_hello_world, 100.0f, (bool success) => { });
                    break;
            }
        }
        #endif
    }
}