using HNW;
using System;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] GameObject weaponCamera;
    [SerializeField] GameObject engineCamera;

    [SerializeField] HolographicButton backButton;
    [SerializeField] HolographicButton weaponsButton;
    [SerializeField] HolographicButton enginesButton;

    [SerializeField] WeaponsShopUI weaponsUI;


    public Transform Owner
    {
        set
        {
            weaponsUI.Owner = value;
        }
    }

    UIPopup popup;

    public event Action onBackClicked;
    public event Action<string, int> onWeaponBuyClicked;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();

        backButton.onClick += OnBackClicked;
        weaponsButton.onClick += OnWeaponsClicked;
        enginesButton.onClick += OnEnginesClicked;

        weaponsUI.onBuyClicked += OnWeaponBuyClicked;
    }

    private void OnDestroy()
    {
        backButton.onClick += OnBackClicked;
        weaponsButton.onClick += OnWeaponsClicked;
        enginesButton.onClick += OnEnginesClicked;

        weaponsUI.onBuyClicked += OnWeaponBuyClicked;
    }

    private void OnBackClicked() => onBackClicked?.Invoke();

    private void OnWeaponsClicked()
    {
        ShowWeapons();
    }

    private void OnEnginesClicked()
    {
        ShowEngines();
    }

    private void OnWeaponBuyClicked(string weaponName, int cost) => onWeaponBuyClicked?.Invoke(weaponName, cost);

    private void ShowWeapons()
    {
        weaponCamera.SetActive(true);
        engineCamera.SetActive(false);

        weaponsUI.Show();
    }

    private void ShowEngines()
    {
        weaponCamera.SetActive(false);
        engineCamera.SetActive(true);

        weaponsUI.Hide();
    }

    public void UpdateCostLabel(string value)
    {
        if (weaponsUI.IsVisible)
            weaponsUI.UpdateCostLabel(value);
    }

    public void Show()
    {
        ShowWeapons();
        popup.Show();
    }

    public void Hide()
    {
        weaponCamera.SetActive(false);
        engineCamera.SetActive(false);

        if (weaponsUI.IsVisible) weaponsUI.Hide();

        popup.Hide();
    }
}