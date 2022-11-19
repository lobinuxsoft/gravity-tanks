using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(CanvasGroup))]
    public class EnginesShopUI : MonoBehaviour
    {
        [SerializeField] ShipData shipdata;
        [SerializeField] EnginesShopData enginesShopData;
        [SerializeField] HolographicButton leftButton;
        [SerializeField] HolographicButton rightButton;
        [SerializeField] HolographicButton buyButton;
        [SerializeField] TextMeshProUGUI infoLabel;
        [SerializeField] TextMeshProUGUI buyLabel;

        public bool IsVisible => canvasGroup.interactable;

        public Transform Owner { get; set; }

        int index;

        Ship ship;

        CanvasGroup canvasGroup;

        public event Action<string, int> onBuyClicked;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            leftButton.onClick += OnLeftClicked;
            rightButton.onClick += OnRightClicked;
            buyButton.onClick += OnBuyClicked;
        }

        private void OnDestroy()
        {
            leftButton.onClick -= OnLeftClicked;
            rightButton.onClick -= OnRightClicked;
            buyButton.onClick -= OnBuyClicked;
        }

        public void Show()
        {
            index = 0;
            ship = Owner.GetComponent<Ship>();

            ViewSelectedEngine();

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide()
        {
            if (ship != null && ship.Engine != null)
                Destroy(ship.Engine.gameObject);

            ship = null;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        private void OnLeftClicked()
        {
            index--;
            index = Mathf.Clamp(index, 0, enginesShopData.GetAllEngines().Count - 1);
            ViewSelectedEngine();
        }

        private void OnRightClicked()
        {
            index++;
            index = Mathf.Clamp(index, 0, enginesShopData.GetAllEngines().Count - 1);
            ViewSelectedEngine();
        }

        private void OnBuyClicked()
        {
            if (ship.Engine != null)
                onBuyClicked?.Invoke(ship.Engine.name, enginesShopData.GetEngineDataByName(ship.Engine.name).Cost);
        }

        private void ViewSelectedEngine()
        {
            if (ship.Engine != null)
                Destroy(ship.Engine.gameObject);

            EngineData ed = enginesShopData.GetAllEngines()[index];

            infoLabel.text = $"{ed.name}" +
                $"\nMove mul.: {ed.MoveForceMultiplier:0.0}" +
                $"\nTurn speed mul.: {ed.TurnSpeedMultiplier:0.0}";


            if (shipdata.Value.engineName.Contains(ed.name))
            {
                buyLabel.text = $"Equiped";
            }
            else
            {
                buyLabel.text = $"Buy ${ed.Cost}<sprite name=\"token_icon\" color=#{ColorUtility.ToHtmlStringRGBA(buyLabel.color)}>";
            }

            ship.Engine = ed.BuildEngine(ship.transform);
        }

        public void UpdateCostLabel(string value) => buyLabel.text = value;
    }
}

