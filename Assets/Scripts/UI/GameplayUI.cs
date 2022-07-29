using CryingOnionTools.ScriptableVariables;
using UnityEngine;
using UnityEngine.UIElements;
using CryingOnionTools.AudioTools;

namespace GravityTanks.UI
{
    [RequireComponent(typeof(SFXTrigger))]
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] IntVariable enemiesEliminated;
        [SerializeField] IntVariable timeRemain;
        [SerializeField] AudioClip clickSfx;

        UIDocument document;
        VisualElement gameplayMessagePanel;
        Label gameplayMessageLabel;
        Label sessionScoreLabel;
        Button resultButton;

        SFXTrigger sfxTrigger;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
            gameplayMessagePanel = document.rootVisualElement.Q<VisualElement>("gameplay-message-panel");
            gameplayMessageLabel = document.rootVisualElement.Q<Label>("gameplay-message-label");
            sessionScoreLabel = document.rootVisualElement.Q<Label>("session-score");
            resultButton = document.rootVisualElement.Q<Button>("result-button");
            sfxTrigger = GetComponent<SFXTrigger>();

            resultButton.clicked += ToGameOver;

            gameplayMessagePanel.SetEnabled(false);
            gameplayMessagePanel.pickingMode = PickingMode.Ignore;
        }

        public void ShowMessage(string message)
        {
            gameplayMessageLabel.text = message;
            int score = enemiesEliminated.Value * 10 + timeRemain.Value * 100;
            sessionScoreLabel.text = $"<size=110><sprite=0></size>:{enemiesEliminated.Value} <size=50>x 10</size> + <size=110><sprite=1></size>:{timeRemain.Value:0s} <size=50>x 100</size> = {score}";
            gameplayMessagePanel.SetEnabled(true);
            gameplayMessagePanel.pickingMode = PickingMode.Position;
        }

        private void OnDestroy() => resultButton.clicked -= ToGameOver;

        private void ToGameOver() 
        {
            sfxTrigger.PlaySFX(clickSfx);
            TransitionSceneUI.FadeOut("GameOver"); 
        }
    }
}