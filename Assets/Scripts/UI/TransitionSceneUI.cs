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

        public static void FadeOut(string sceneName) 
        {
            nextSneceName = sceneName;
            transitionElement.RegisterCallback<TransitionEndEvent>(ChangeScene);
            transitionElement.pickingMode = PickingMode.Position;
            transitionElement.SetEnabled(true);
        }

        private static void ChangeScene(TransitionEndEvent evt) => SceneManager.LoadScene(nextSneceName);

        public static void FadeIn() => transitionElement.SetEnabled(false);
    }
}