using CryingOnionTools.ScriptableVariables;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GravityTanks.UI
{
    public class TimerUI : MonoBehaviour
    {
        [Tooltip("Duracion total del contador en segundos")]
        [SerializeField] int timerDuration = 60;

        [SerializeField] IntVariable timerCount;

        UIDocument uiDocument;
        Label timerLabel;

        public UnityEvent onCountDownEnd;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            timerLabel = uiDocument.rootVisualElement.Q<Label>("timer-label");
            timerCount.onValueChange += UpdateView;

            timerCount.Value = timerDuration;

            StartCountDown();
        }

        private void OnDestroy() => timerCount.onValueChange -= UpdateView;

        private void UpdateView(int value) => timerLabel.text = $"{value}:s";

        public void StartCountDown()
        {
            StopCountDown();
            
            timerCount.Value = timerDuration;

            StartCoroutine(CountDownRoutine());
        }

        public void StopCountDown() => StopAllCoroutines();

        IEnumerator CountDownRoutine()
        {
            while (timerCount.Value > 0)
            {
                yield return new WaitForSeconds(1);
                timerCount.Value--;
            }

            onCountDownEnd?.Invoke();
        }
    }
}