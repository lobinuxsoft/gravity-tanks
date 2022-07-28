using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GravityTanks.UI
{
    public class TransitionSceneUI : MonoBehaviour
    {
        UIDocument document;
        static VisualElement transitionElement;
        static string nextSneceName;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
            transitionElement = document.rootVisualElement.Q<VisualElement>("transition");
        }

        private void Start() => FadeIn();

        public static void FadeOut(string sceneName) 
        {
            nextSneceName = sceneName;
            transitionElement.RegisterCallback<TransitionEndEvent>(ChangeScene);
            transitionElement.pickingMode = PickingMode.Position;
            transitionElement.SetEnabled(true);
        }

        private static void ChangeScene(TransitionEndEvent evt) 
        {
            transitionElement.UnregisterCallback<TransitionEndEvent>(ChangeScene);
            SceneManager.LoadScene(nextSneceName); 
        }

        private static void FadeIn() => transitionElement.SetEnabled(false);
    }
}