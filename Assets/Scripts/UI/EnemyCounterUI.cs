using System.Collections;
using TMPro;
using UnityEngine;

namespace HNW
{
    public class EnemyCounterUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI counterLabel;
        [SerializeField] AnimationCurve animBehaviour;
        [SerializeField] float animationSpeed = 1f;

        int max;
        int cur;

        private void Awake()
        {
            EnemyFactory.onMaxEnemyChange += OnMaxEnemyChange;
            EnemyFactory.onEnemyRemainingAliveChange += OnEnemyAlive;
        }

        private void OnDestroy()
        {
            EnemyFactory.onMaxEnemyChange -= OnMaxEnemyChange;
            EnemyFactory.onEnemyRemainingAliveChange -= OnEnemyAlive;
        }

        private void OnMaxEnemyChange(int value)
        {
            max = value;
            UpdateView();
        }

        private void OnEnemyAlive(int value)
        {
            cur = value;
            UpdateView();
        }

        private void UpdateView()
        {
            counterLabel.text = $"{cur}/{max}<sprite name=\"virus_icon\" color=#{ColorUtility.ToHtmlStringRGBA(counterLabel.color)}>";
            StartCoroutine(PlayAnimation(animationSpeed));
        }

        IEnumerator PlayAnimation(float speed)
        {
            float lerp = 0;

            while (lerp < 1f)
            {
                lerp += Time.unscaledDeltaTime * speed;
                counterLabel.transform.localScale = Vector3.one * animBehaviour.Evaluate(lerp);
                yield return null;
            }

            counterLabel.transform.localScale = Vector3.one * animBehaviour.Evaluate(1);
        }
    }
}