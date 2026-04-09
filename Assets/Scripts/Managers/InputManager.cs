using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public UnityAction OnMouseButtonDown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown?.Invoke();
        }

    }
}
