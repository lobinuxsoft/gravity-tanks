using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace HNW.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WeaponsShopUI : MonoBehaviour
    {
        [SerializeField] ShipData shipdata;
        [SerializeField] WeaponsShopData weaponsShopData;
        [SerializeField] HolographicButton leftButton;
        [SerializeField] HolographicButton rightButton;
        [SerializeField] HolographicButton buyButton;
        [SerializeField] TextMeshProUGUI infoLabel;
        [SerializeField] TextMeshProUGUI buyLabel;

        public bool IsVisible => canvasGroup.interactable;

        public Transform Owner { get; set; }

        int index;

        Weapon weaponSelected;

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
            ViewSelectedWeapon();

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide()
        {
            if (weaponSelected != null)
                Destroy(weaponSelected.gameObject);

            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        private void OnLeftClicked()
        {
            index--;
            index = Mathf.Clamp(index, 0, weaponsShopData.GetAllWeapons().Count-1);
            ViewSelectedWeapon();
        }

        private void OnRightClicked()
        {
            index++;
            index = Mathf.Clamp(index, 0, weaponsShopData.GetAllWeapons().Count-1);
            ViewSelectedWeapon();
        }

        private void OnBuyClicked()
        {
            if (weaponSelected != null)
                onBuyClicked?.Invoke(weaponSelected.name, weaponsShopData.GetWeaponDataByName(weaponSelected.name).Cost);
        }

        private void ViewSelectedWeapon()
        {
            if(weaponSelected != null)
                Destroy(weaponSelected.gameObject);

            WeaponData wd = weaponsShopData.GetAllWeapons()[index];

            infoLabel.text = $"{wd.name}" +
                $"\nAttack Mul: {wd.AttackMultiplier:0.0}" +
                $"\nProjectiles: {wd.EmmitAmount:0}" +
                $"\nRate: {wd.ShotRate:0.0}s" +
                $"\nAngle: {wd.ShotAngle:0}º";

            
            if (shipdata.Value.weaponsNames.Contains(wd.name))
                buyLabel.text = $"Equiped";
            else
                buyLabel.text = $"Buy ${wd.Cost}<sprite name=\"token_icon\" color=#{ColorUtility.ToHtmlStringRGBA(buyLabel.color)}>";

            weaponSelected = wd.BuildWeapon(Owner);
        }

        public void UpdateCostLabel(string value) => buyLabel.text = value;
    }
}