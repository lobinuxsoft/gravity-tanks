using UnityEngine;
using CryingOnionTools.AudioTools;
using UnityEngine.UIElements;

namespace GravityTanks.UI
{
    [RequireComponent(typeof(SFXTrigger))]
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] AudioClip clickSfx;
        UIDocument document;
        VisualElement tutorialPanel;
        Button tutorialButton;
        Button returnButton;

        SFXTrigger sfxTrigger;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
            tutorialPanel = document.rootVisualElement.Q<VisualElement>("tutorial-panel");
            tutorialButton = document.rootVisualElement.Q<Button>("tutorial-button");
            returnButton = document.rootVisualElement.Q<Button>("return-button");
            sfxTrigger = GetComponent<SFXTrigger>();

            tutorialButton.clicked += OpenPanel;
            returnButton.clicked += ClosePanel;

            tutorialPanel.SetEnabled(false);
            tutorialPanel.pickingMode = PickingMode.Ignore;
        }

        private void OnDestroy()
        {
            tutorialButton.clicked -= OpenPanel;
            returnButton.clicked -= ClosePanel;
        }

        private void OpenPanel()
        {
            sfxTrigger.PlaySFX(clickSfx);
            tutorialPanel.SetEnabled(true);
            tutorialPanel.pickingMode = PickingMode.Position;
        }

        private void ClosePanel()
        {
            sfxTrigger.PlaySFX(clickSfx);
            tutorialPanel.SetEnabled(false);
            tutorialPanel.pickingMode = PickingMode.Ignore;
        }
    }
}