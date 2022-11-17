using HNW;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBuilderTest : MonoBehaviour
{
    [SerializeField] ShipData shipData;
    [SerializeField] WeaponsShopData weaponsShopData;

    [SerializeField] ShootControl shootControl;

    [SerializeField] Button buttonTemplate;
    [SerializeField] Transform container;

    Ship ship;

    private void Start()
    {
        foreach (var data in weaponsShopData.GetAllWeapons())
        {
            var button = Instantiate(buttonTemplate, container);
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = data.name;
            button.onClick.AddListener(() => { CreateWeapon(data); });
        }

        ship = shipData.BuildShip(shootControl.transform);
    }

    public void CreateWeapon(WeaponData data)
    {
        var temWlist = ship.Weapons.ToList();

        temWlist.Add(data.BuildWeapon(shootControl.transform));

        ship.Weapons = temWlist.ToArray();
    }
}