// Basado en: https://www.youtube.com/watch?v=XF4HzViMSRk&t=101s

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class HoverMovement : MonoBehaviour
{
    [SerializeField] float multiplier;
    [SerializeField] float moveForce, turnTorque;
    [SerializeField] Vector3 centerOfMass = Vector3.down;
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

        rb.AddForce(moveInput * moveForce * transform.forward);
        rb.AddTorque(turnInput * turnTorque * transform.up);
    }

    void ApplyForce(Transform anchor, ref RaycastHit hit)
    {
        if(Physics.Raycast(anchor.position, -anchor.up, out hit, multiplier, rayLayerMask))
        {
            float force = Mathf.Abs(1 / Mathf.Clamp(Vector3.Distance(hit.point, anchor.position), .1f, multiplier));
            rb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }
    }

    public void MoveInput(InputAction.CallbackContext callbackContext)
    {
        Vector2 input = callbackContext.ReadValue<Vector2>();
        moveInput = input.y;
        turnInput = input.x;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + centerOfMass, .25f);

        if(anchors != null && anchors.Length > 0) 
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < anchors.Length; i++)
            {
                Gizmos.DrawSphere(anchors[i].position, .25f);
            }
        }

        if(hits != null && hits.Length > 0)
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
