using System;
using TMPro;
using UnityEngine;

namespace HNW
{
    public class StatUI : MonoBehaviour
    {
        [SerializeField] HolographicButton statButton;
        [SerializeField] TextMeshProUGUI statLabel;
        [SerializeField] TextMeshProUGUI costLabel;

        public event Action onStatClicked;

        public int StatValue
        {
            set => statLabel.text = $"{value}";
        }

        public int CostValue
        {
            set => costLabel.text = $"${value}<sprite name=\"token_icon\" color=#{ColorUtility.ToHtmlStringRGBA(costLabel.color)}>";
        }

        private void Awake()
        {
            statButton.onClick += OnStatClicked;
        }

        private void OnDestroy()
        {
            statButton.onClick -= OnStatClicked;
        }

        private void OnStatClicked() => onStatClicked?.Invoke();
    }
}