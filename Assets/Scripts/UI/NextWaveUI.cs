using System;
using TMPro;
using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(UIPopup))]
    public class NextWaveUI : MonoBehaviour
    {
        [SerializeField] HolographicButton healButton;
        [SerializeField] HolographicButton nextButton;
        [SerializeField] HolographicButton returnButton;
        [SerializeField] TextMeshProUGUI titleLabel;
        [SerializeField] TextMeshProUGUI healLabel;

        public string Title
        {
            set => titleLabel.text = value;
        }

        public bool ShowHealtButton
        {
            set => healButton.gameObject.SetActive(value);
        }

        public int HealthCost
        {
            set => healLabel.text = $"Heal ${value}<sprite name=\"token_icon\" color=#{ColorUtility.ToHtmlStringRGBA(healLabel.color)}>";
        }

        public bool ShowNextButton
        {
            set => nextButton.gameObject.SetActive(value);
        }

        UIPopup popup;

        public event Action onNextClicked;
        public event Action onHealClicked;
        public event Action onReturnClicked;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();
            healButton.onClick += OnHealClicked;
            nextButton.onClick += OnNextClicked;
            returnButton.onClick += OnReturnClicked;
        }

        private void OnDestroy()
        {
            healButton.onClick += OnHealClicked;
            nextButton.onClick += OnNextClicked;
            returnButton.onClick += OnReturnClicked;
        }

        public void Show() => popup.Show();

        private void OnHealClicked()
        {
            healButton.gameObject.SetActive(false);
            onHealClicked?.Invoke();
        }

        private void OnNextClicked() => popup.Hide(onNextClicked);

        private void OnReturnClicked() => onReturnClicked?.Invoke();
    }
}