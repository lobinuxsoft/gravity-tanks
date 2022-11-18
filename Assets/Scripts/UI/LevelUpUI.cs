using System;
using UnityEngine;

namespace HNW
{
    public class LevelUpUI : MonoBehaviour
    {
        [SerializeField] StatUI attackStat;
        [SerializeField] StatUI defenseStat;
        [SerializeField] StatUI speedStat;
        [SerializeField] HolographicButton backButton;

        public int AttackValue
        {
            set
            {
                attackStat.StatValue = value;
                attackStat.CostValue = value * 10;
            }
        }

        public int DefenseValue
        {
            set
            {
                defenseStat.StatValue = value;
                defenseStat.CostValue = value * 10;
            }
        }

        public int SpeedValue
        {
            set
            {
                speedStat.StatValue = value;
                speedStat.CostValue = value * 10;
            }
        }

        UIPopup popup;

        public event Action onAttackClicked;
        public event Action onDefenseClicked;
        public event Action onSpeedClicked;
        public event Action onBackClicked;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            attackStat.onStatClicked += OnAttackClicked;
            defenseStat.onStatClicked += OnDefenseClicked;
            speedStat.onStatClicked += OnSpeedClicked;

            backButton.onClick += OnBackClicked;
        }

        private void OnDestroy()
        {
            attackStat.onStatClicked -= OnAttackClicked;
            defenseStat.onStatClicked -= OnDefenseClicked;
            speedStat.onStatClicked -= OnSpeedClicked;

            backButton.onClick -= OnBackClicked;
        }

        private void OnAttackClicked() => onAttackClicked?.Invoke();

        private void OnDefenseClicked() => onDefenseClicked?.Invoke();

        private void OnSpeedClicked() => onSpeedClicked?.Invoke();

        private void OnBackClicked() => onBackClicked?.Invoke();

        public void Show() => popup.Show();

        public void Hide() => popup.Hide();
    }
}