using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace HNW
{
    [RequireComponent(typeof(Image))]
    public class HolographicButton : MonoBehaviour, IPointerClickHandler
    {
        [Header("Base Setup")]
        [SerializeField] Material baseMaterial;
        [SerializeField] Sprite rgbMask;
        [SerializeField] float lerpSpeed = 1.5f;
        [SerializeField] AnimationCurve lerpBehaviour;
        [SerializeField] Vector2 tilingUV = Vector2.one;
        [SerializeField] Vector2 offsetUV;
        [SerializeField, Range(-180, 180)] float rollUV;
        [SerializeField] float glitchStrenght;
        [SerializeField] float glitchScale;
        [SerializeField] float glitchSpeed;

        [Space(10)]
        [Header("Red Channel Settings")]
        [SerializeField] ChannelState normalR;
        [SerializeField] ChannelState pressR;

        [Space(10)]
        [Header("Green Channel Settings")]
        [SerializeField] ChannelState normalG;
        [SerializeField] ChannelState pressG;

        [Space(10)]
        [Header("Blue Channel Settings")]
        [SerializeField] ChannelState normalB;
        [SerializeField] ChannelState pressB;

        [Space(10)]
        [Header("Icon Settings")]
        [SerializeField] Sprite iconTex;
        [SerializeField] IconState normalIcon;
        [SerializeField] IconState pressIcon;

        Image image;

        public event Action onClick;

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
                image.material.SetFloat("_RollUV", rollUV);
                image.material.SetFloat("_GlitchStrenght", glitchStrenght);
                image.material.SetFloat("_GlitchScale", glitchScale);
                image.material.SetFloat("_GlitchSpeed", glitchSpeed);

                //Red
                image.material.SetColor("_ColorR", normalR.color);
                image.material.SetFloat("_ScaleR", normalR.scale);
                image.material.SetFloat("_RotSpeedR", normalR.rotSpeed);
                image.material.SetVector("_OffsetR", normalR.offset);

                //Green
                image.material.SetColor("_ColorG", normalG.color);
                image.material.SetFloat("_ScaleG", normalG.scale);
                image.material.SetFloat("_RotSpeedG", normalG.rotSpeed);
                image.material.SetVector("_OffsetG", normalG.offset);

                //Blue
                image.material.SetColor("_ColorB", normalB.color);
                image.material.SetFloat("_ScaleB", normalB.scale);
                image.material.SetFloat("_RotSpeedB", normalB.rotSpeed);
                image.material.SetVector("_OffsetB", normalB.offset);

                //Icon
                image.material.SetTexture("_IconTex", iconTex.texture);
                image.material.SetColor("_ColorIcon", normalIcon.color);
                image.material.SetFloat("_ScaleIcon", normalIcon.scale);
                image.material.SetFloat("_RotIcon", normalIcon.rotation);
                image.material.SetVector("_OffsetIcon", normalIcon.offset);
            }
        }

        IEnumerator ClickAnimRoutine(float speed)
        {
            float lerp = 0;

            while (lerp < 1)
            {
                lerp += Time.unscaledDeltaTime * speed;

                //Red
                image.material.SetColor("_ColorR", Color.LerpUnclamped(normalR.color, pressR.color, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_ScaleR", Mathf.LerpUnclamped(normalR.scale, pressR.scale, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_RotSpeedR", Mathf.LerpUnclamped(normalR.rotSpeed, pressR.rotSpeed, lerpBehaviour.Evaluate(lerp)));
                image.material.SetVector("_OffsetR", Vector2.LerpUnclamped(normalR.offset, pressR.offset, lerpBehaviour.Evaluate(lerp)));

                //Green
                image.material.SetColor("_ColorG", Color.LerpUnclamped(normalG.color, pressG.color, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_ScaleG", Mathf.LerpUnclamped(normalG.scale, pressG.scale, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_RotSpeedG", Mathf.LerpUnclamped(normalG.rotSpeed, pressG.rotSpeed, lerpBehaviour.Evaluate(lerp)));
                image.material.SetVector("_OffsetG", Vector2.LerpUnclamped(normalG.offset, pressG.offset, lerpBehaviour.Evaluate(lerp)));

                //Blue
                image.material.SetColor("_ColorB", Color.LerpUnclamped(normalB.color, pressB.color, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_ScaleB", Mathf.LerpUnclamped(normalB.scale, pressB.scale, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_RotSpeedB", Mathf.LerpUnclamped(normalB.rotSpeed, pressB.rotSpeed, lerpBehaviour.Evaluate(lerp)));
                image.material.SetVector("_OffsetB", Vector2.LerpUnclamped(normalB.offset, pressB.offset, lerpBehaviour.Evaluate(lerp)));

                //Icon
                image.material.SetColor("_ColorIcon", Color.LerpUnclamped(normalIcon.color, pressIcon.color, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_ScaleIcon", Mathf.LerpUnclamped(normalIcon.scale, pressIcon.scale, lerpBehaviour.Evaluate(lerp)));
                image.material.SetFloat("_RotIcon", Mathf.LerpUnclamped(normalIcon.rotation, pressIcon.rotation, lerpBehaviour.Evaluate(lerp)));
                image.material.SetVector("_OffsetIcon", Vector2.LerpUnclamped(normalIcon.offset, pressIcon.offset, lerpBehaviour.Evaluate(lerp)));

                yield return null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayAnimation(lerpSpeed);
            onClick?.Invoke();
        }

        public void PlayAnimation(float speed) => StartCoroutine(ClickAnimRoutine(speed));
    }


    [Serializable]
    public struct ChannelState
    {
        [ColorUsage(true, true)] public Color color;
        public float scale;
        public float rotSpeed;
        public Vector2 offset;
    }

    [Serializable]
    public struct IconState
    {
        [ColorUsage(true, true)] public Color color;
        public float scale;
        [Range(-180, 180)] public float rotation;
        public Vector2 offset;
    }
}