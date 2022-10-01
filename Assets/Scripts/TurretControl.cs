using System.Linq;
using UnityEngine;

namespace GravityTanks
{
    public class TurretControl : MonoBehaviour
    {
        [SerializeField] float rotSpeed = 5f;
        [SerializeField] float minDistanceToAim = 5f;
        [SerializeField] float minAngleToShot = 2.5f;
        [SerializeField] Transform aim;
        [SerializeField] CannonControl cannon;

        Transform targetToShot = null;

        Rigidbody body;

        private void Awake()
        {
            aim.SetParent(null);
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {

            if (Time.frameCount % 5 == 0)
                targetToShot = FindClosedTarget();

            aim.gameObject.SetActive(targetToShot);

            if (!targetToShot) return;

            aim.position = targetToShot.position;

            Vector3 targetDir = targetToShot.position - body.position;
            Quaternion newRot = Quaternion.LookRotation(targetDir, Vector3.up);
            body.rotation = Quaternion.RotateTowards(
                transform.rotation,
                newRot,
                Quaternion.Angle(transform.rotation, newRot) * rotSpeed * Time.deltaTime);

            if (Vector3.Angle(transform.forward, targetDir.normalized) <= minAngleToShot)
            {
                cannon.transform.LookAt(targetToShot);
                cannon.Shoot();
            }

            if ((targetToShot.position - transform.position).sqrMagnitude > minDistanceToAim * minDistanceToAim)
                targetToShot = null;
        }

        private Transform FindClosedTarget(string tag = "Enemy")
        {
            var all = GameObject.FindGameObjectsWithTag(tag).ToList();

            all = all.Where(t => (transform.position - t.transform.position).sqrMagnitude <= minDistanceToAim * minDistanceToAim).ToList();

            all.Sort
                (
                    delegate(GameObject a, GameObject b)
                    {
                        return (transform.position - a.transform.position).sqrMagnitude.CompareTo((transform.position - b.transform.position).sqrMagnitude);
                    }
                );

            return all.Count > 0 ? all[0].transform : null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, minDistanceToAim);
        }
    }
}