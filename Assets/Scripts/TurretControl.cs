using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GravityTanks
{
    public class TurretControl : MonoBehaviour
    {
        [SerializeField] float rotSpeed = 1f;
        [SerializeField] Transform turret;
        [SerializeField] Transform aim;

        bool isMouse;
        Camera cam;
        Vector2 pointerInput;
        Vector3 dir;
        Vector3 pointer3D = Vector3.zero;

        public UnityEvent onTurretLookTarget;

        private void Awake()
        {
            cam = Camera.main;

            aim.SetParent(null);
        }

        private void LateUpdate()
        {
            Ray ray = cam.ViewportPointToRay(pointerInput);

            Physics.Raycast(ray, out RaycastHit hit);

            if (hit.collider != null)
            {
                pointer3D = hit.point;


                if (Vector3.Distance(pointer3D, transform.position) > 1.5f)
                {
                    dir = (pointer3D - transform.position).normalized;

                    aim.gameObject.SetActive(true);
                    aim.position = pointer3D + hit.normal * .25f;
                }
                else
                {
                    aim.gameObject.SetActive(false);
                }

                float singleStep = rotSpeed * Time.deltaTime;

                Vector3 newDir = Vector3.RotateTowards(turret.forward, dir, singleStep, 0.0f);

                turret.rotation = Quaternion.LookRotation(newDir, transform.up);
            }
            else
            {
                aim.gameObject.SetActive(false);
            }
        }

        private void Shoot()
        {
            if (Vector3.Angle(turret.forward, dir) < 1)
                onTurretLookTarget?.Invoke();
        }

        public void PointerInput(InputAction.CallbackContext callbackContext)
        {
            Vector2 inputValue = callbackContext.ReadValue<Vector2>();

            if (isMouse)
            {
                pointerInput.x = inputValue.x / Screen.width;
                pointerInput.y = inputValue.y / Screen.height;
            }
            else
            {
                pointerInput.x = .5f + (inputValue.x * .5f);
                pointerInput.y = .5f + (inputValue.y * .5f);
            }
        }

        public void ShootInput(InputAction.CallbackContext callbackContext)
        {
            if (!callbackContext.performed) return;

            Shoot();
        }

        Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }

        public void ChangedControls(PlayerInput playerInput)
        {
            isMouse = !playerInput.currentControlScheme.Contains("Gamepad");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(pointer3D, .25f);
        }
    }
}