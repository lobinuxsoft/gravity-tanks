using System.Collections;
using TMPro;
using UnityEngine;

namespace HNW.UI
{
    public class ExpView : MonoBehaviour
    {
        [SerializeField] LongVariable expVariable;
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] AnimationCurve animBehaviour;
        [SerializeField] float animationSpeed = 1f;

        private void Awake() => expVariable.onValueChange += OnValueChange;

        private void Start() => SetText(expVariable.Value);

        private void OnDestroy() => expVariable.onValueChange -= OnValueChange;

        private void OnValueChange(long value)
        {
            StartCoroutine(PlayAnimation(animationSpeed));
            SetText(value);
        }

        private void SetText(long value)
        {
            label.text = $"{value.ToString()}<sprite name=\"token_icon\" color=#{ColorUtility.ToHtmlStringRGBA(label.color)}>";
        }

        IEnumerator PlayAnimation(float speed)
        {
            float lerp = 0;

            while (lerp < 1f)
            {
                lerp += Time.unscaledDeltaTime * speed;
                label.transform.localScale = Vector3.one * animBehaviour.Evaluate(lerp);
                yield return null;
            }

            label.transform.localScale = Vector3.one * animBehaviour.Evaluate(1);
        }
    }
}