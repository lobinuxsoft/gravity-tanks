using HNW;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBuilderTest : MonoBehaviour
{
    [SerializeField] WeaponData[] weaponDatas;

    [SerializeField] ShootControl shootControl;

    [SerializeField] Button buttonTemplate;
    [SerializeField] Transform container;

    private void Start()
    {
        foreach (var data in weaponDatas)
        {
            var button = Instantiate(buttonTemplate, container);
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = data.name;
            button.onClick.AddListener(() => { CreateWeapon(data); });
        }
    }

    public void CreateWeapon(WeaponData data)
    {
        data.BuildWeapon(shootControl.transform);
        shootControl.UpdateWeapons();
    }
}