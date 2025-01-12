using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation in degrees per second

    private InputActions inputActions; // Replace this with your specific input class if named differently
    private float rotationInput;
    private float rotationInputVertical;


    private void Awake()
    {
        // Initialize the input actions (replace 'GameplayActions' with your actual input class)
        inputActions = new InputActions();

        // Bind the RotateCamera action
        inputActions.Gameplay.RotateCamera.performed += OnRotateCamera;
        inputActions.Gameplay.RotateCamera.canceled += OnRotateCamera;
        inputActions.Gameplay.RotateCameraVertical.performed += OnRotateCameraVertical;
        inputActions.Gameplay.RotateCameraVertical.canceled += OnRotateCameraVertical;
    }

    private void OnEnable()
    {
        // Enable the Gameplay action map
        inputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        // Disable the Gameplay action map
        inputActions.Gameplay.Disable();
    }

    private void Update()
    {
        // Apply rotation input
        if (rotationInput != 0)
        {
            float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
        }
        if (rotationInputVertical != 0)
        {
            float rotationAmount = rotationInputVertical * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, rotationAmount); // Rotates around the global X-axis
        }
    }

    private void OnRotateCamera(InputAction.CallbackContext context)
    {
        // Read the input value
        rotationInput = context.ReadValue<float>();
    }

    private void OnRotateCameraVertical(InputAction.CallbackContext context)
    {
        // Read the input value
        rotationInputVertical = context.ReadValue<float>();
    }
}
