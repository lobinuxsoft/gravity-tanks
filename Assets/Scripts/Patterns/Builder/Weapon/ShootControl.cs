using System.Linq;
using UnityEngine;

namespace HNW
{
    public class ShootControl : MonoBehaviour
    {
        [Header("Aiming settings")]
        [SerializeField] LayerMask targetLayerMask;
        [SerializeField] float rotSpeed = 10f;
        [SerializeField] float minDistanceToAim = 10f;
        [SerializeField] float minAngleToShot = 5f;
        [SerializeField] Transform aim;

        Weapon[] weapons;

        Transform targetToShoot = null;

        Rigidbody body;

        private void Start()
        {
            aim.SetParent(null);
            body = GetComponent<Rigidbody>();
            UpdateWeapons();
        }

        private void OnEnable()
        {
            UpdateWeapons();
        }

        public void UpdateWeapons()
        {
            weapons = GetComponentsInChildren<Weapon>();
        }

        private void FixedUpdate()
        {
            if (Time.frameCount % 5 == 0)
                targetToShoot = FindClosedTarget(targetLayerMask);

            aim.gameObject.SetActive(targetToShoot);

            if (!targetToShoot) return;

            aim.position = targetToShoot.position;

            Vector3 targetDir = targetToShoot.position - body.position;
            Quaternion newRot = Quaternion.LookRotation(targetDir, Vector3.up);

            body.rotation = Quaternion.RotateTowards(
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

            if ((targetToShoot.position - body.position).sqrMagnitude > minDistanceToAim * minDistanceToAim)
                targetToShoot = null;
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
            if (!targetToShoot)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, minDistanceToAim);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, targetToShoot.position);
                Gizmos.DrawSphere(targetToShoot.position, .25f);
            }
        }
    }
}