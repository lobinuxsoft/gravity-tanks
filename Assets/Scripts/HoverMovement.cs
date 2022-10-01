// Basado en: https://www.youtube.com/watch?v=XF4HzViMSRk&t=101s

using UnityEngine;
using UnityEngine.InputSystem;

namespace GravityTanks
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
    public class HoverMovement : MonoBehaviour
    {
        [SerializeField] float multiplier = 2;
        [SerializeField] float moveForce = 20, turnSpeed = 10;
        [SerializeField] Vector3 centerOfMass = Vector3.down * 2;
        [SerializeField] LayerMask rayLayerMask;
        [SerializeField] Transform[] anchors;

        Rigidbody rb;

        float moveInput;
        float turnInput;

        RaycastHit[] hits;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            rb.mass = 1;
            rb.drag = 1;
            rb.angularDrag = 2;

            hits = new RaycastHit[anchors.Length];
        }

        private void FixedUpdate()
        {
            rb.centerOfMass = centerOfMass;

            for (int i = 0; i < anchors.Length; i++)
            {
                ApplyForce(anchors[i], ref hits[i]);
            }

            Vector3 moveDir = new Vector3(turnInput, 0, moveInput);

            //rb.AddForce(moveInput * moveForce * transform.forward);
            rb.AddForce(moveDir * moveForce);
            //rb.AddTorque(turnInput * turnTorque * transform.up);

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
            if (Physics.Raycast(anchor.position, -anchor.up, out hit, multiplier, rayLayerMask))
            {
                float force = Mathf.Abs(1 / Mathf.Clamp(Vector3.Distance(hit.point, anchor.position), .1f, multiplier));
                rb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
            }
        }

        public void MoveInput(InputAction.CallbackContext callbackContext) => moveInput = callbackContext.ReadValue<float>();

        public void TurnInput(InputAction.CallbackContext callbackContext) => turnInput = callbackContext.ReadValue<float>();

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
                        Gizmos.DrawLine(anchors[i].position, anchors[i].position - anchors[i].up * multiplier);
                        Gizmos.DrawWireSphere(anchors[i].position - anchors[i].up * multiplier, .25f);
                    }
                }
            }
        }
#endif
    }
}