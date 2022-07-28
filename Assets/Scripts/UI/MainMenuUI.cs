using UnityEngine;
using UnityEngine.UIElements;

namespace GravityTanks.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        UIDocument document;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
        }
    }
}
