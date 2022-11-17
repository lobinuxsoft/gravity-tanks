using CryingOnionTools.ScriptableVariables;
using System.Collections;
using System.Runtime.Remoting.Channels;
using UnityEngine;
using UnityEngine.UI;

public class RadialHPBar : MonoBehaviour
{
    [SerializeField] IntVariable cur;
    [SerializeField] IntVariable max;

    [Header("Base Settings")]
    [SerializeField] Material baseMaterial;
    [SerializeField] Sprite mainTex;
    [SerializeField, Range(-180, 180)] float rollUV;
    [SerializeField] Vector2 tilingUV;
    [SerializeField] Vector2 offsetUV;

    [Header("Red Channel")]
    [SerializeField] RadialBarStruct redChannel;

    [Header("Green Channel")]
    [SerializeField] RadialBarStruct greenChannel;

    [Header("Blue Channel")]
    [SerializeField] RadialBarStruct blueChannel;

    [Header("Icon")]
    [SerializeField] RadialIconStruct icon;

    bool isInited = false;
    Image image;

    private void OnValidate()
    {
        UpdateView();
    }

    private void Awake() 
    {
        UpdateView();
        cur.onValueChange += OnValueChange;
        max.onValueChange += OnValueChange;
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
        cur.onValueChange -= OnValueChange;
        max.onValueChange -= OnValueChange;
    }

    void UpdateView()
    {
        if(TryGetComponent(out image))
        {
            image.material = new Material(baseMaterial);

            image.material.SetTexture("_MainTex", mainTex.texture);
            image.material.SetFloat("_RollUV", rollUV);
            image.material.SetVector("_TilingUV", tilingUV);
            image.material.SetVector("_OffsetUV", offsetUV);

            #region Red Channel
            image.material.SetColor("_ColorR", redChannel.color);
            image.material.SetVector("_OffsetR", redChannel.offset);
            image.material.SetFloat("_RadialScaleR", redChannel.scale);
            image.material.SetFloat("_RotSpeedR", redChannel.rotationSpeed);
            image.material.SetFloat("_RadialGradientR", redChannel.gradientScale);
            image.material.SetFloat("_FillOrientationR", redChannel.fillOrientation);
            image.material.SetFloat("_FillAmountR", redChannel.fillAmount);
            #endregion

            #region Green Channel
            image.material.SetColor("_ColorG", greenChannel.color);
            image.material.SetVector("_OffsetG", greenChannel.offset);
            image.material.SetFloat("_RadialScaleG", greenChannel.scale);
            image.material.SetFloat("_RotSpeedG", greenChannel.rotationSpeed);
            image.material.SetFloat("_RadialGradientG", greenChannel.gradientScale);
            image.material.SetFloat("_FillOrientationG", greenChannel.fillOrientation);
            image.material.SetFloat("_FillAmountG", greenChannel.fillAmount);
            #endregion

            #region Blue Channel
            image.material.SetColor("_ColorB", blueChannel.color);
            image.material.SetVector("_OffsetB", blueChannel.offset);
            image.material.SetFloat("_RadialScaleB", blueChannel.scale);
            image.material.SetFloat("_RotSpeedB", blueChannel.rotationSpeed);
            image.material.SetFloat("_RadialGradientB", blueChannel.gradientScale);
            image.material.SetFloat("_FillOrientationB", blueChannel.fillOrientation);
            image.material.SetFloat("_FillAmountB", blueChannel.fillAmount);
            #endregion

            #region Icon
            image.material.SetTexture("_IconTex", icon.iconTex.texture);
            image.material.SetColor("_IconColor", icon.color);
            image.material.SetFloat("_IconScale", icon.scale);
            image.material.SetFloat("_RotationIcon", icon.rotation);
            image.material.SetVector("_OffsetIcon", icon.offset);
            #endregion
        }
    }

    public void SetFillChannelR(float amount)
    {
        redChannel.fillAmount = amount;
        UpdateView();
    }

    public void SetFillChannelG(float amount)
    {
        greenChannel.fillAmount = amount;
        UpdateView();
    }

    public void SetFillChannelB(float amount)
    {
        blueChannel.fillAmount = amount;
        UpdateView();
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

        StartCoroutine(AnimBarEffect("_FillAmountG", (float)cur.Value / max.Value, 2f));
    }
}

[System.Serializable]
public struct RadialBarStruct
{
    [ColorUsage(true, true)] public Color color;
    public Vector2 offset;
    public float scale;
    public float rotationSpeed;
    public float gradientScale;
    [Range(-180, 180)] public float fillOrientation;
    [Range(0, 1)] public float fillAmount;
}

[System.Serializable]
public struct RadialIconStruct
{
    public Sprite iconTex;
    [ColorUsage(true, true)] public Color color;
    public float scale;
    [Range(-180, 180)] public float rotation;
    public Vector2 offset;
}