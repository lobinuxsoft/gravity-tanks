// Basado en: https://www.youtube.com/watch?v=XF4HzViMSRk&t=101s

using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(Rigidbody))]
    public class HoverMovement : MonoBehaviour
    {
        [SerializeField] float heighForce = 2;
        [SerializeField] float moveForce = 20, turnSpeed = 10;
        [SerializeField] Vector3 centerOfMass = Vector3.down * 2;
        [SerializeField] LayerMask rayLayerMask;
        [SerializeField] Transform[] anchors;

        public float HeighForce
        {
            get => heighForce;
            set => heighForce = value;
        }

        public float MoveForce
        {
            get => moveForce;
            set => moveForce = value;
        }

        public float TurnSpeed
        {
            get => turnSpeed;
            set => turnSpeed = value;
        }

        public LayerMask RayLayerMask
        {
            get => rayLayerMask;
            set => rayLayerMask = value;
        }

        Rigidbody rb;

        Vector2 moveInput;

        RaycastHit[] hits;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            rb.mass = 1;
            rb.drag = 1;
            rb.angularDrag = 2;

            if(anchors == null)
                anchors = GetComponents<Transform>();

            hits = new RaycastHit[anchors.Length];
        }

        private void FixedUpdate()
        {
            rb.centerOfMass = centerOfMass;

            for (int i = 0; i < anchors.Length; i++)
            {
                ApplyForce(anchors[i], ref hits[i]);
            }

            Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

            rb.AddForce(moveDir * moveForce);

            if(moveDir.magnitude > 0f)
            {
                Quaternion newRot = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    newRot,
                    Quaternion.Angle(transform.rotation, newRot) * turnSpeed * Time.deltaTime);
            }
        }

        void ApplyForce(Transform anchor, ref RaycastHit hit)
        {
            if (Physics.Raycast(anchor.position, -anchor.up, out hit, heighForce, rayLayerMask))
            {
                float force = Mathf.Abs(1 / Mathf.Clamp(Vector3.Distance(hit.point, anchor.position), .1f, heighForce));
                rb.AddForceAtPosition(transform.up * force * heighForce, anchor.position, ForceMode.Acceleration);
            }
        }

        public void Move(Vector2 value) => moveInput = value;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + centerOfMass, .25f);

            if (anchors != null && anchors.Length > 0)
            {
                Gizmos.color = Color.green;

                for (int i = 0; i < anchors.Length; i++)
                {
                    Gizmos.DrawSphere(anchors[i].position, .25f);
                }
            }

            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].distance > 0)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(anchors[i].position, hits[i].point);
                        Gizmos.DrawWireSphere(hits[i].point, .25f);
                    }
                    else
                    {
                        Gizmos.DrawLine(anchors[i].position, anchors[i].position - anchors[i].up * heighForce);
                        Gizmos.DrawWireSphere(anchors[i].position - anchors[i].up * heighForce, .25f);
                    }
                }
            }
        }
#endif
    }
}