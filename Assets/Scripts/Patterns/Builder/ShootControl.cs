using System.Linq;
using UnityEngine;

namespace HNW
{
    public class ShootControl : MonoBehaviour
    {
        [Header("Aiming settings")]
        [SerializeField] LayerMask targetLayerMask;
        [SerializeField] int rotationSpeed = 10;
        [SerializeField] int detectionDistance = 10;
        [SerializeField] int minAngleToShot = 5;
        [SerializeField] Vector3 aimOffset = Vector3.up;
        [SerializeField] Transform aim;

        /// <summary>
        /// The speed to look rotation
        /// </summary>
        public int RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }

        /// <summary>
        /// Auto detection Enemy distance
        /// </summary>
        public int DetectionDistance
        {
            get => detectionDistance;
            set => detectionDistance = value;
        }

        /// <summary>
        /// The min angle to start shooting
        /// </summary>
        public int MinAngleToShot
        {
            get => minAngleToShot;
            set => minAngleToShot = value;
        }

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

            aim.position = targetToShoot.position + aimOffset;

            Vector3 targetDir = targetToShoot.position - body.position;
            Quaternion newRot = Quaternion.LookRotation(targetDir, Vector3.up);

            body.rotation = Quaternion.RotateTowards(
                transform.rotation,
                newRot,
                Quaternion.Angle(transform.rotation, newRot) * rotationSpeed * Time.deltaTime);

            if (Vector3.Angle(transform.forward, targetDir.normalized) <= minAngleToShot)
            {
                for (int i = 0; i < weapons.Length; i++)
                {
                    weapons[i].Shoot();
                }
            }

            if ((targetToShoot.position - body.position).sqrMagnitude > detectionDistance * detectionDistance)
                targetToShoot = null;
        }

        private Transform FindClosedTarget(LayerMask mask)
        {
            var all = Physics.OverlapSphere(transform.position, detectionDistance, mask).ToList();

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
                Gizmos.DrawWireSphere(transform.position, detectionDistance);
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