using UnityEngine;
using UnityEngine.UIElements;
using GravityTanks.Utils;

namespace GravityTanks.UI
{
    public class CreditsUI : MonoBehaviour
    {
        [SerializeField] Credits credits;
        [SerializeField] VisualTreeAsset categoryTemplate;
        [SerializeField] VisualTreeAsset autorTemplate;

        UIDocument document;
        GroupBox creditsContainer;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
            creditsContainer = document.rootVisualElement.Q<GroupBox>("CreditsContainer");

            LoadFromFile();
        }

        private void LoadFromFile()
        {
            string jsonString = Resources.Load<TextAsset>("credits").text;
            credits = JsonUtility.FromJson<Credits>(jsonString);

            foreach (var category in credits.categories)
            {
                AddCategory(category.name, creditsContainer);

                foreach (var author in category.authors)
                {
                    AddAutor(author.name, author.url, creditsContainer);
                }
            }
        }

        private void AddCategory(string name, GroupBox container)
        {
            var element = categoryTemplate.Instantiate();
            Label categoryLabel = element.Q<Label>("category-label");
            categoryLabel.text = name;
            container.Add(element);
        }

        private void AddAutor(string name, string url, GroupBox container)
        {
            var element = autorTemplate.Instantiate();
            Button autorLabel = element.Q<Button>("autor-label");
            autorLabel.text = name;
            autorLabel.clicked += () => { Application.OpenURL(url); };
            container.Add(element);
        }
    }
}
