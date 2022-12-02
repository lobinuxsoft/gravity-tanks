using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HNW
{
    public class LinkButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        private string url;

        public string LabelText
        {
            set => textMesh.text = value;
        }

        public string URL
        {
            set => url = value;
        }

        public void OnPointerClick(PointerEventData eventData) => Application.OpenURL(url);
    }
}