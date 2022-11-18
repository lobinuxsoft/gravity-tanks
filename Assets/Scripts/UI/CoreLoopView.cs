using GooglePlayGames;
using System;
using UnityEngine;

namespace HNW
{
    public class CoreLoopView : MonoBehaviour
    {
        [Header("Coreloop settings")]
        [SerializeField] ShipData shipData;
        [SerializeField] LongVariable exp;
        [SerializeField] string gameplayScene = "Gameplay";
        [SerializeField] Gradient fadeIn;
        [SerializeField] Gradient fadeOut;
        [SerializeField] HolographicButton playButton;
        [SerializeField] HolographicButton shopButton;
        [SerializeField] HolographicButton statsButton;
        [SerializeField] HolographicButton achievementsButton;
        [SerializeField] HolographicButton leaderboardButton;
        [SerializeField] Transform avatarOwner;

        [Space(10)]
        [Header("Levelup settings")]
        [SerializeField] LevelUpUI levelUpUI;

        Ship ship;

        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            playButton.onClick += OnPlayClicked;
            shopButton.onClick += OnShopClicked;
            statsButton.onClick += OnStatsClicked;
            achievementsButton.onClick += OnAchievementsClicked;
            leaderboardButton.onClick += OnLeaderboardClicked;

            levelUpUI.onAttackClicked += OnAttackClicked;
            levelUpUI.onDefenseClicked += OnDefenseClicked;
            levelUpUI.onSpeedClicked += OnSpeedClicked;
            levelUpUI.onBackClicked += OnBackClicked;

            exp.LoadData();

            ship = shipData.BuildShip(avatarOwner);

            levelUpUI.AttackValue = shipData.Value.attack;
            levelUpUI.DefenseValue = shipData.Value.defense;
            levelUpUI.SpeedValue = shipData.Value.speed;
        }

        private void OnDestroy()
        {
            playButton.onClick -= OnPlayClicked;
            shopButton.onClick -= OnShopClicked;
            statsButton.onClick -= OnStatsClicked;
            achievementsButton.onClick -= OnAchievementsClicked;
            leaderboardButton.onClick -= OnLeaderboardClicked;

            levelUpUI.onAttackClicked -= OnAttackClicked;
            levelUpUI.onDefenseClicked -= OnDefenseClicked;
            levelUpUI.onSpeedClicked -= OnSpeedClicked;
            levelUpUI.onBackClicked -= OnBackClicked;
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
            levelUpUI.AttackValue = shipData.Value.attack;
            levelUpUI.DefenseValue = shipData.Value.defense;
            levelUpUI.SpeedValue = shipData.Value.speed;

            levelUpUI.Show();
            popup.Hide();
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

        private void OnAttackClicked()
        {
            var cost = shipData.Value.attack * 10;

            if(exp.Value >= cost)
            {
                var temp = shipData.Value;
                temp.attack++;

                shipData.Value = temp;

                exp.Value -= cost;

                shipData.SaveData();
                exp.SaveData();

                levelUpUI.AttackValue = shipData.Value.attack;
            }
        }

        private void OnDefenseClicked()
        {
            var cost = shipData.Value.defense * 10;

            if (exp.Value >= cost)
            {
                var temp = shipData.Value;
                temp.defense++;

                shipData.Value = temp;

                exp.Value -= cost;

                shipData.SaveData();
                exp.SaveData();

                levelUpUI.DefenseValue = shipData.Value.defense;
            }
        }

        private void OnSpeedClicked()
        {
            var cost = shipData.Value.speed * 10;

            if (exp.Value >= cost)
            {
                var temp = shipData.Value;
                temp.speed++;

                shipData.Value = temp;

                exp.Value -= cost;

                shipData.SaveData();
                exp.SaveData();

                levelUpUI.SpeedValue = shipData.Value.speed;
            }
        }

        private void OnBackClicked()
        {
            levelUpUI.Hide();
            popup.Show();
        }
    }
}