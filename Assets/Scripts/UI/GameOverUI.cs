using UnityEngine;
using CryingOnionTools.AudioTools;
using UnityEngine.UIElements;
using CryingOnionTools.ScriptableVariables;
using GravityTanks.Utils;

namespace GravityTanks.UI
{
    [RequireComponent(typeof(SFXTrigger))]
    public class GameOverUI : MonoBehaviour
    {
        const int enemyScoreValue = 10;
        const int timeScoreValue = 10;

        [SerializeField] IntVariable enemiesAmount;
        [SerializeField] IntVariable timeRemain;
        [SerializeField] ScoreBoardData scoreBoardData;
        [SerializeField] VisualTreeAsset scoreLabel;
        [SerializeField] AudioClip clickSfx;

        UIDocument uiDocument;
        Label sessionScore;
        GroupBox scoreBoardContainer;
        Button toMainMenuButton;
        SFXTrigger sfxTrigger;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();

            sessionScore = uiDocument.rootVisualElement.Q<Label>("session-score");
            scoreBoardContainer = uiDocument.rootVisualElement.Q<GroupBox>("score-board");
            toMainMenuButton = uiDocument.rootVisualElement.Q<Button>("mainmenu-button");
            toMainMenuButton.clicked += ToMainMenu;

            sfxTrigger = GetComponent<SFXTrigger>();

            ShowScore();
        }

        private void OnDestroy()
        {
            sfxTrigger.PlaySFX(clickSfx);
            toMainMenuButton.clicked -= ToMainMenu;
        }

        private void ShowScore()
        {
            int score = enemiesAmount.Value * 10 + timeRemain.Value * 100;

            sessionScore.text = $"<size=60><sprite=0></size>: {enemiesAmount.Value} <size=60><sprite=1></size>: {timeRemain.Value} Score: {score}";

            var newScore = new ScoreData { enemiesEliminated = enemiesAmount.Value, timeRemain = timeRemain.Value, score = score };

            EvaluateScores(newScore);

            for (int i = 0; i < scoreBoardData.Value.Count; i++)
            {
                AddScore(i+1, scoreBoardData.Value[i], scoreBoardContainer);
            }
        }

        private void ToMainMenu() => TransitionSceneUI.FadeOut("MainMenu");

        private void EvaluateScores(ScoreData value)
        {
            scoreBoardData.LoadData();
            
            if(scoreBoardData.Value.Count > 0)
            {
                for (int i = 0; i < scoreBoardData.Value.Count; i++)
                {
                    if (scoreBoardData.Value[i].score < value.score)
                    {
                        scoreBoardData.Value[i] = value;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    scoreBoardData.Value.Add(new ScoreData());


                for (int j = 0; j < scoreBoardData.Value.Count; j++)
                {
                    if (scoreBoardData.Value[j].score < value.score)
                    {
                        scoreBoardData.Value[j] = value;
                        break;
                    }
                }
            }

            scoreBoardData.SaveData();
        }

        private void AddScore(int index, ScoreData data, GroupBox container)
        {
            var element = scoreLabel.Instantiate();
            Label label = element.Q<Label>("score-label");
            label.text = $"{index:0º} <size=60><sprite=0></size>: {data.enemiesEliminated} <size=60><sprite=1></size>: {data.timeRemain} Score: {data.score}";
            container.Add(element);
        }
    }
}
