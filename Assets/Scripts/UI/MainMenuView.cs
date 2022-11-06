using HNW;
using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] Transform playerAvatar;
    [SerializeField] WeaponData weaponData;

    void Start()
    {
        weaponData.BuildWeapon(playerAvatar);
    }
}