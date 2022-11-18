using TMPro;
using UnityEngine;

namespace HNW
{
    public class ExpView : MonoBehaviour
    {
        [SerializeField] LongVariable expVariable;
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] HolographicButton icon;

        private void Awake() => expVariable.onValueChange += OnValueChange;

        private void Start() => label.text = expVariable.Value.ToString();

        private void OnDestroy() => expVariable.onValueChange -= OnValueChange;

        private void OnValueChange(long value)
        {
            icon.PlayAnimation(1.5f);
            label.text = value.ToString();
        }
    }
}