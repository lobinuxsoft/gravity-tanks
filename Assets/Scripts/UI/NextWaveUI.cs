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
        [SerializeField] TextMeshProUGUI healLabel;

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

        public void Show(bool showHealButton, int healCost)
        {
            healLabel.text = $"Heal -{healCost}Exp";
            healButton.gameObject.SetActive(showHealButton);
            popup.Show();
        }


        private void OnHealClicked()
        {
            healButton.gameObject.SetActive(false);
            onHealClicked?.Invoke();
        }

        private void OnNextClicked() => popup.Hide(onNextClicked);

        private void OnReturnClicked() => onReturnClicked?.Invoke();
    }
}