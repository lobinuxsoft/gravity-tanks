using CryingOnionTools.ScriptableVariables;
using System.Collections;
using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(HolographicRadialProgressBar))]
    public class HPRadialBar : MonoBehaviour
    {
        [SerializeField] IntVariable cur;
        [SerializeField] IntVariable max;

        bool isInited = false;
        HolographicRadialProgressBar radialBar;

        private void Awake()
        {
            radialBar = GetComponent<HolographicRadialProgressBar>();
            cur.onValueChange += OnValueChange;
            max.onValueChange += OnValueChange;
        }

        private void OnDestroy()
        {
            cur.onValueChange -= OnValueChange;
            max.onValueChange -= OnValueChange;
        }

        private void Start()
        {
            radialBar.SetFillChanneldRGB(0, 0, 0);

            StartCoroutine(InitHPBar(1f));
        }

        IEnumerator InitHPBar(float speed)
        {
            yield return StartCoroutine(InitAnimationBar(speed));
            isInited = true;
        }

        IEnumerator InitAnimationBar(float speed)
        {
            float lerp = 0;

            while (lerp < 1f)
            {
                lerp += Time.unscaledDeltaTime * speed;
                radialBar.SetFillChanneldRGB(Mathf.Clamp01(lerp), Mathf.Clamp01(lerp), Mathf.Clamp01(lerp));
                yield return null;
            }

            radialBar.SetFillChanneldRGB(Mathf.Clamp01(lerp), Mathf.Clamp01(lerp), Mathf.Clamp01(lerp));
        }

        private void OnValueChange(int value)
        {
            if (!isInited) return;

            StartCoroutine(AnimBarEffect((float)cur.Value / max.Value, 2f));
        }

        IEnumerator AnimBarEffect(float targetValue, float speed)
        {
            float lerp = 0;

            float curValue = radialBar.GetFillAmountG();

            while (lerp < 1f)
            {
                lerp += Time.unscaledDeltaTime * speed;
                radialBar.SetFillChanneldRGB(1, Mathf.Lerp(curValue, targetValue, lerp), 1);
                yield return null;
            }

            radialBar.SetFillChanneldRGB(1, targetValue, 1);
        }
    }
}