using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CryingOnionTools.AudioTools;

namespace GravityTanks.UI
{
    [RequireComponent(typeof(MixerVolumeController), typeof(SFXTrigger))]
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] AudioClip clickSfx;

        UIDocument document;
        VisualElement pausePanel;
        Button pauseButton;
        Button resumeButton;
        Button toMenuButton;
        Toggle audioToggle;
        MixerVolumeController volumeController;
        SFXTrigger sfxTrigger;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
            pausePanel = document.rootVisualElement.Q<VisualElement>("pause-panel");
            pauseButton = document.rootVisualElement.Q<Button>("pause-button");
            resumeButton = document.rootVisualElement.Q<Button>("resume-button");
            toMenuButton = document.rootVisualElement.Q<Button>("mainmenu-button");
            audioToggle = document.rootVisualElement.Q<Toggle>("audio-toggle");

            volumeController = GetComponent<MixerVolumeController>();
            sfxTrigger = GetComponent<SFXTrigger>();
            audioToggle.value = volumeController.Volume > 0;

            pauseButton.clicked += Open;
            resumeButton.clicked += Close;
            toMenuButton.clicked += ToMainMenu;
            audioToggle.RegisterValueChangedCallback(OnToggleChange);
        }

        private void Start() => Close();

        private void OnDestroy()
        {
            pauseButton.clicked -= Open;
            resumeButton.clicked -= Close;
            toMenuButton.clicked -= ToMainMenu;
            audioToggle.UnregisterValueChangedCallback(OnToggleChange);
        }

        private void Open()
        {
            sfxTrigger.PlaySFX(clickSfx);

            Time.timeScale = 0;
            pausePanel.SetEnabled(true);
            pauseButton.SetEnabled(false);
        }

        private void Close()
        {
            sfxTrigger.PlaySFX(clickSfx);

            Time.timeScale = 1;
            pausePanel.SetEnabled(false);
            pauseButton.SetEnabled(true);
        }

        private void ToMainMenu()
        {
            sfxTrigger.PlaySFX(clickSfx);

            Close();
            TransitionSceneUI.FadeOut("MainMenu");
        }

        private void OnToggleChange(ChangeEvent<bool> evt)
        {
            sfxTrigger.PlaySFX(clickSfx);
            volumeController.ChangeVolume(evt.newValue ? 1 : 0);
        }
    }
}
