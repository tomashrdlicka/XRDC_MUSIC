using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputHandler : MonoBehaviour
{
    private Camera mainCamera;

    private NoteCell currentlySelectedCell = null;
    private Vector3 lastMousePosition;

    [SerializeField] private float volumeSensitivity = 0.005f;

    private InputActions inputActions;
    private bool isRightMouseHeld = false;

    void Awake()
    {
        mainCamera = Camera.main;  // or assign in Inspector
        inputActions = new InputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        // Bind to the Gameplay map
        inputActions.Gameplay.LeftClick.performed += OnLeftMouseClick;
        inputActions.Gameplay.RightMouseDown.performed += OnRightMouseDown;
        inputActions.Gameplay.RightMouseUp.performed += OnRightMouseUp;
    }

    void OnDisable()
    {
        inputActions.Gameplay.LeftClick.performed -= OnLeftMouseClick;
        inputActions.Gameplay.RightMouseDown.performed -= OnRightMouseDown;
        inputActions.Gameplay.RightMouseUp.performed -= OnRightMouseUp;

        inputActions.Disable();
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isRightMouseHeld && currentlySelectedCell != null)
        {
            Vector3 currentMousePos = Mouse.current.position.ReadValue();

            float deltaY = currentMousePos.y - lastMousePosition.y;
            float volumeChange = deltaY * volumeSensitivity;

            currentlySelectedCell.AdjustVolume(volumeChange);
            lastMousePosition = currentMousePos;
        }
    }

    public void OnLeftMouseClick(InputAction.CallbackContext context)
{
    // Check if the input action actually performed
    if (context.performed)
    {
        Debug.Log("OnLeftMouseClick was performed.");

        // Construct a ray from the camera
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.Log($"Ray created from camera. Origin: {ray.origin}, Direction: {ray.direction}");

        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log($"Raycast hit object: {hit.collider.gameObject.name} at distance {hit.distance}");

            if (hit.collider == null)
            {
                Debug.LogWarning("Raycast hit something, but collider is null.");
                return;
            }

            // Check if the hit object has a NoteCell component
            NoteCell noteCell = hit.collider.GetComponent<NoteCell>();
            if (noteCell != null)
            {
                Debug.Log($"Hit a NoteCell on object '{hit.collider.gameObject.name}'. Toggling note...");
                noteCell.ToggleNote();
            }
            else
            {
                Debug.Log("Hit an object that does not have a NoteCell component.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything within 100 units.");
        }
    }
    else
    {
        Debug.Log("OnLeftMouseClick called, but the action was not performed.");
    }
}

    public void OnRightMouseDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                NoteCell noteCell = hit.collider.GetComponent<NoteCell>();
                if (noteCell != null)
                {
                    currentlySelectedCell = noteCell;
                    isRightMouseHeld = true;
                }
            }
            lastMousePosition = Mouse.current.position.ReadValue();
        }
    }

    public void OnRightMouseUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRightMouseHeld = false;
            currentlySelectedCell = null;
        }
    }
}



