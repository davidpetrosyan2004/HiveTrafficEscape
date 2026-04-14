using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance { get; private set; }

    private PlayerInput playerInput;
    public InputAction touchPositionAction;
    public InputAction touchPressAction;
    public Vector2 position;
    public UnityAction<Vector2> OnTouchPressed;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void OnEnable()
    {
        touchPressAction.Enable();
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
        touchPressAction.Disable();
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        if (Touchscreen.current == null) return;

        position = Touchscreen.current.primaryTouch.position.ReadValue();
        OnTouchPressed?.Invoke(position);
    }
}