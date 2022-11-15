using System.Collections.Generic;
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Weapon Builder/Weapons Shop Data")]
    public class WeaponsShopData : ScriptableObject
    {
        [SerializeField] List<WeaponData> weaponsDatas = new List<WeaponData>();

        public WeaponData GetWeaponDataByName(string name) => weaponsDatas.Find(x => x.name == name);

        public List<WeaponData> GetAllWeapons() => weaponsDatas;
    }
}