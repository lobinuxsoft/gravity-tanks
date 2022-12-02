using HNW.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HNW
{
    [RequireComponent(typeof(UIPopup))]
    public class CreditsView : MonoBehaviour
    {
        [SerializeField] HolographicButton closeButton;
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] LinkButton linkButton;
        [SerializeField] Scrollbar scrollbar;
        [SerializeField] Transform container;

        [Space(10)]
        [Header("Credits Settings")]
        [SerializeField] Credits credits;

        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();

            closeButton.onClick += Hide;

            CreateCreditsButtons();
        }

        private void OnDestroy() => closeButton.onClick -= Hide;

        public void Show() => popup.Show();

        private void Hide() => popup.Hide();

        private void CreateCreditsButtons()
        {
            string jsonString = Resources.Load<TextAsset>("credits").text;
            credits = JsonUtility.FromJson<Credits>(jsonString);

            foreach (var category in credits.categories)
            {
                var title = Instantiate(titleText, container);
                title.text = $"\n\n{category.name}";

                for (int i = 0; i < category.authors.Count; i++)
                {
                    var authorButton = Instantiate(linkButton, container);
                    authorButton.LabelText = category.authors[i].name;
                    authorButton.URL = category.authors[i].url;
                }
            }
        }
    }
}