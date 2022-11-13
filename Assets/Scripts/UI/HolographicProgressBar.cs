using UnityEngine;
using UnityEngine.UI;

namespace HNW
{
    public class HolographicProgressBar : MonoBehaviour
    {
        [Header("Base Setup")]
        [SerializeField] Material baseMaterial;
        [SerializeField] Sprite rgbMask;
        [SerializeField] Vector2 tilingUV = Vector2.one;
        [SerializeField] Vector2 offsetUV;
        [SerializeField] float glitchStrenght;
        [SerializeField] float glitchScale;
        [SerializeField] float glitchSpeed;

        [Space(10)]
        [Header("Red Channel Settings")]
        [SerializeField] ProgressBarState channelR;

        [Space(10)]
        [Header("Green Channel Settings")]
        [SerializeField] ProgressBarState channelG;

        [Space(10)]
        [Header("Blue Channel Settings")]
        [SerializeField] ProgressBarState channelB;

        Image image;

        private void OnValidate() => UpdateView();

        private void Awake() => UpdateView();

        private void UpdateView()
        {
            if (TryGetComponent(out image))
            {
                image.material = new Material(baseMaterial);

                image.sprite = rgbMask;
                image.material.SetVector("_TilingUV", tilingUV);
                image.material.SetVector("_OffsetUV", offsetUV);
                image.material.SetFloat("_GlitchStrenght", glitchStrenght);
                image.material.SetFloat("_GlitchScale", glitchScale);
                image.material.SetFloat("_GlitchSpeed", glitchSpeed);

                //Red
                image.material.SetColor("_ColorR", channelR.color);
                image.material.SetFloat("_FillAmountR", channelR.fillAmount);
                image.material.SetFloat("_ScaleR", channelR.scale);
                image.material.SetVector("_OffsetR", channelR.offset);

                //Green
                image.material.SetColor("_ColorG", channelG.color);
                image.material.SetFloat("_FillAmountG", channelG.fillAmount);
                image.material.SetFloat("_ScaleG", channelG.scale);
                image.material.SetVector("_OffsetG", channelG.offset);

                //Blue
                image.material.SetColor("_ColorB", channelB.color);
                image.material.SetFloat("_FillAmountB", channelB.fillAmount);
                image.material.SetFloat("_ScaleB", channelB.scale);
                image.material.SetVector("_OffsetB", channelB.offset);
            }
        }

        public void SetFillChannelR(float amount)
        {
            channelR.fillAmount = amount;
            UpdateView();
        }

        public void SetFillChannelG(float amount)
        {
            channelG.fillAmount = amount;
            UpdateView();
        }

        public void SetFillChannelB(float amount)
        {
            channelB.fillAmount = amount;
            UpdateView();
        }
    }
}

[System.Serializable]
public struct ProgressBarState
{
    [ColorUsage(true, true)] public Color color;
    [Range(0f, 1f)] public float fillAmount;
    public float scale;
    public Vector2 offset;
}