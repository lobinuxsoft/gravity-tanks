using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;

    [SerializeField] UnityEvent<Vector2> onMoveAction;

    private void OnEnable()
    {
        moveAction.action.performed += OnMovePerform;
        moveAction.action.canceled += OnMovePerform;

        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();

        moveAction.action.performed -= OnMovePerform;
        moveAction.action.canceled -= OnMovePerform;
    }

    private void OnMovePerform(InputAction.CallbackContext context) => onMoveAction?.Invoke(context.ReadValue<Vector2>());
}