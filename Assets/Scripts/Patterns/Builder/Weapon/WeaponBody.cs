using UnityEngine;

public class WeaponBody : MonoBehaviour
{
    [SerializeField] Transform[] shootPoints;

    public Transform[] ShootPoints => shootPoints;
}