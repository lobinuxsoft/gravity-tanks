using CryingOnionTools.ScriptableVariables;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RadialHPBar : MonoBehaviour
{
    [SerializeField] IntVariable curHp;
    [SerializeField] IntVariable maxHp;

    bool isInited = false;
    Image image;

    private void Awake() 
    {
        image = GetComponent<Image>();
        curHp.onValueChange += OnValueChange;
        maxHp.onValueChange += OnValueChange;
    }

    private void Start() 
    {
        image.material.SetFloat("_FillAmountR", 0);
        image.material.SetFloat("_FillAmountG", 0);
        image.material.SetFloat("_FillAmountB", 0);

        StartCoroutine(InitHPBar(1f));
    }

    private void OnDestroy()
    {
        curHp.onValueChange -= OnValueChange;
        maxHp.onValueChange -= OnValueChange;
    }

    IEnumerator InitHPBar(float speed)
    {
        yield return StartCoroutine(AnimBarEffect("_FillAmountR", 1f, speed));
        yield return StartCoroutine(AnimBarEffect("_FillAmountB", 1f, speed));
        yield return StartCoroutine(AnimBarEffect("_FillAmountG", 1f, speed));
        isInited = true;
    }

    IEnumerator AnimBarEffect(string shaderPropertyName, float targetValue, float speed)
    {
        float lerp = 0;

        float curValue = image.material.GetFloat(shaderPropertyName);

        while (lerp < 1f)
        {
            lerp +=Time.unscaledDeltaTime * speed;
            image.material.SetFloat(shaderPropertyName, Mathf.Lerp(curValue, targetValue, lerp));
            yield return null;
        }

        image.material.SetFloat(shaderPropertyName, targetValue);
    }

    private void OnValueChange(int value)
    {
        if(!isInited) return;

        StartCoroutine(AnimBarEffect("_FillAmountG", (float)curHp.Value / maxHp.Value, 2f));
    }
}