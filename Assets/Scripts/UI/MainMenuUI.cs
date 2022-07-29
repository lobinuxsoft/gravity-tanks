#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UIElements;
using CryingOnionTools.AudioTools;

namespace GravityTanks.UI
{
    [RequireComponent(typeof(MixerVolumeController), typeof(SFXTrigger))]
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] AudioClip clickSfx;

        UIDocument document;
        Button playButton;
        Button quitButton;
        Toggle audioToggle;
        MixerVolumeController volumeController;
        SFXTrigger sfxTrigger;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
            playButton = document.rootVisualElement.Q<Button>("play-button");
            quitButton = document.rootVisualElement.Q<Button>("quit-button");
            audioToggle = document.rootVisualElement.Q<Toggle>("audio-toggle");

            volumeController = GetComponent<MixerVolumeController>();
            sfxTrigger = GetComponent<SFXTrigger>();
            

            playButton.clicked += ToGamePlay;
            quitButton.clicked += QuitGame;
            audioToggle.RegisterValueChangedCallback(OnToggleChange);

#if UNITY_WEBGL
            quitButton.SetEnabled(false);
            quitButton.visible = false;
#endif
        }

        private void Start() => AudioSettingLoad();

        private void OnDestroy()
        {
            playButton.clicked -= ToGamePlay;
            quitButton.clicked -= QuitGame;
            audioToggle.UnregisterValueChangedCallback(OnToggleChange);
        }

        private void OnToggleChange(ChangeEvent<bool> evt) 
        { 
            sfxTrigger.PlaySFX(clickSfx);
            AudioSettingSave(evt.newValue);
        }

        private void ToGamePlay() 
        { 
            sfxTrigger.PlaySFX(clickSfx);
            TransitionSceneUI.FadeOut("Gameplay"); 
        }

        private void AudioSettingSave(bool value)
        {
            volumeController.ChangeVolume(value ? 1 : 0);
            PlayerPrefs.SetInt("AudioSetting", (int)volumeController.Volume);
            PlayerPrefs.Save();
        }

        private void AudioSettingLoad()
        {
            if (PlayerPrefs.HasKey("AudioSetting"))
                volumeController.ChangeVolume(PlayerPrefs.GetInt("AudioSetting"));

            audioToggle.value = volumeController.Volume > .1f;
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}