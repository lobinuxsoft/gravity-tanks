using GooglePlayGames;
using System;
using System.Linq;
using UnityEngine;

namespace HNW.UI
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

        [Space(10)]
        [Header("Shop settings")]
        [SerializeField] ShopUI shopUI;

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
            levelUpUI.onHPClicked += OnHPClicked;
            levelUpUI.onBackClicked += OnCloseLevelUpUI;

            shopUI.onBackClicked += OnCloseShop;
            shopUI.onWeaponBuyClicked += OnBuyWeapon;
            shopUI.onEngineBuyClicked += OnBuyEngine;

            shopUI.Owner = avatarOwner;

            shipData.LoadData();
            exp.LoadData();

            ship = shipData.BuildShip(avatarOwner);

            levelUpUI.AttackValue = shipData.Value.attack;
            levelUpUI.DefenseValue = shipData.Value.defense;
            levelUpUI.SpeedValue = shipData.Value.speed;
            levelUpUI.HPValue = shipData.Value.maxHp;
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
            levelUpUI.onHPClicked -= OnHPClicked;
            levelUpUI.onBackClicked -= OnCloseLevelUpUI;

            shopUI.onBackClicked -= OnCloseShop;
            shopUI.onWeaponBuyClicked -= OnBuyWeapon;
            shopUI.onEngineBuyClicked -= OnBuyEngine;
        }

        private void OnPlayClicked()
        {
            TimelineUITransitionScene.Instance.FadeStart(gameplayScene, 1, fadeIn, fadeOut);
        }

        private void OnShopClicked()
        {
            shopUI.Show();
            popup.Hide();
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
            var cost = shipData.Value.attack * 100;

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
            var cost = shipData.Value.defense * 100;

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
            var cost = shipData.Value.speed * 100;

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

        private void OnHPClicked()
        {
            var cost = shipData.Value.maxHp * 100;

            if (exp.Value >= cost)
            {
                var temp = shipData.Value;
                temp.maxHp++;

                shipData.Value = temp;

                exp.Value -= cost;

                shipData.SaveData();
                exp.SaveData();

                levelUpUI.HPValue = shipData.Value.maxHp;
            }
        }

        private void OnCloseLevelUpUI()
        {
            levelUpUI.Hide();
            popup.Show();
        }

        private void OnCloseShop()
        {
            for (int i = 0; i < avatarOwner.childCount; i++)
            {
                Destroy(avatarOwner.GetChild(i).gameObject);
            }

            ship = shipData.BuildShip(avatarOwner);

            shopUI.Hide();
            popup.Show();
        }

        private void OnBuyWeapon(string weaponName, int cost)
        {
            if (shipData.Value.weaponsNames.Contains(weaponName)) return;

            if (exp.Value >= cost)
            {
                var weaponsList = shipData.Value.weaponsNames.ToList();
                weaponsList.Add(weaponName);

                var temp = shipData.Value;
                temp.weaponsNames = weaponsList.ToArray();

                shipData.Value = temp;
                shipData.SaveData();

                exp.Value -= cost;
                exp.SaveData();

                for (int i = 0; i < avatarOwner.childCount; i++)
                {
                    Destroy(avatarOwner.GetChild(i).gameObject);
                }

                ship = shipData.BuildShip(avatarOwner);

                shopUI.UpdateCostLabel("Equiped");

                #if UNITY_ANDROID

                int count = shipData.Value.weaponsNames.Length;

                if (count > 1)
                    PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_secondary_weapon, 100.0f, (bool success) => { });

                if (count > 2)
                    PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_gun_lover, 100.0f, (bool success) => { });

                if (count > 3)
                    PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_war_machine, 100.0f, (bool success) => { });

                #endif
            }
        }

        private void OnBuyEngine(string engineName, int cost)
        {
            if (shipData.Value.engineName.Contains(engineName)) return;

            if (exp.Value >= cost)
            {

                var temp = shipData.Value;
                temp.engineName = engineName;

                shipData.Value = temp;
                shipData.SaveData();

                exp.Value -= cost;
                exp.SaveData();

                for (int i = 0; i < avatarOwner.childCount; i++)
                {
                    Destroy(avatarOwner.GetChild(i).gameObject);
                }

                ship = shipData.BuildShip(avatarOwner);

                shopUI.UpdateCostLabel("Equiped");
            }
        }
    }
}