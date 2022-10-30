using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HNW.Utils
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera farCamera;
        [SerializeField] CinemachineVirtualCamera nearCamera;

        void Start()
        {
            farCamera.gameObject.SetActive(true);
            nearCamera.gameObject.SetActive(false);
        }

        public void SwitchCamera(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            nearCamera.gameObject.SetActive(!nearCamera.gameObject.activeSelf);
            farCamera.gameObject.SetActive(!farCamera.gameObject.activeSelf);
        }
    }
}