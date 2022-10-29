using System.Linq;
using UnityEngine;

namespace HNW
{
    public class ShootControl : MonoBehaviour
    {
        [Header("Aiming settings")]
        [SerializeField] LayerMask targetLayerMask;
        [SerializeField] float rotSpeed = 5f;
        [SerializeField] float minDistanceToAim = 10f;
        [SerializeField] float minAngleToShot = 2.5f;
        [SerializeField] Transform aim;

        Weapon[] weapons;

        Transform targetToShot = null;

        private void Start()
        {
            aim.SetParent(null);
            UpdateWeapons();
        }

        private void OnEnable()
        {
            UpdateWeapons();
        }

        public void UpdateWeapons()
        {
            weapons = GetComponentsInChildren<Weapon>();

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].LayerToDamage = targetLayerMask;
            }
        }

        private void FixedUpdate()
        {
            if (Time.frameCount % 5 == 0)
                targetToShot = FindClosedTarget(targetLayerMask);

            aim.gameObject.SetActive(targetToShot);

            if (!targetToShot) return;

            aim.position = targetToShot.position;

            Vector3 targetDir = targetToShot.position - transform.position; //body.position;
            Quaternion newRot = Quaternion.LookRotation(targetDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                newRot,
                Quaternion.Angle(transform.rotation, newRot) * rotSpeed * Time.deltaTime);

            if (Vector3.Angle(transform.forward, targetDir.normalized) <= minAngleToShot)
            {
                for (int i = 0; i < weapons.Length; i++)
                {
                    weapons[i].Shoot();
                }
            }

            if ((targetToShot.position - transform.position).sqrMagnitude > minDistanceToAim * minDistanceToAim)
                targetToShot = null;
        }

        private Transform FindClosedTarget(LayerMask mask)
        {
            var all = Physics.OverlapSphere(transform.position, minDistanceToAim, mask).ToList();

            all.Sort
                (
                    delegate (Collider a, Collider b)
                    {
                        return (transform.position - a.transform.position).sqrMagnitude.CompareTo((transform.position - b.transform.position).sqrMagnitude);
                    }
                );

            return all.Count > 0 ? all[0].transform : null;
        }

        private void OnDrawGizmos()
        {
            if (!targetToShot)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, minDistanceToAim);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, targetToShot.position);
                Gizmos.DrawSphere(targetToShot.position, .25f);
            }
        }
    }
}