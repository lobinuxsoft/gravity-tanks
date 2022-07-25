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

        int timerCount = 0;
        UIDocument uiDocument;
        Label timerLabel;

        public UnityEvent onCountDownEnd;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            timerLabel = uiDocument.rootVisualElement.Q<Label>("timer-label");
            timerCount = timerDuration;
            timerLabel.text = $"{timerCount}:s";

            StartCountDown();
        }

        public void StartCountDown()
        {
            StopCountDown();
            
            timerCount = timerDuration;
            timerLabel.text = $"{timerCount}:s";

            StartCoroutine(CountDownRoutine());
        }

        public void StopCountDown() => StopAllCoroutines();

        IEnumerator CountDownRoutine()
        {
            while (timerCount > 0)
            {
                yield return new WaitForSeconds(1);
                timerCount--;
                timerLabel.text = $"{timerCount}:s";
            }

            onCountDownEnd?.Invoke();
        }
    }
}