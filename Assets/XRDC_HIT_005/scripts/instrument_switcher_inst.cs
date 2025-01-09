using UnityEngine;
using UnityEngine.InputSystem;

public class InstrumentSwitcherNew : MonoBehaviour
{
    [Tooltip("Reference to the ring manager that creates and holds NoteCells.")]
    public RingGridManager ringManager;  // Assign in Inspector

    private InputActions inputActions;

    void Awake()
    {
        // Initialize the InputActions
        inputActions = new InputActions();
    }

    void OnEnable()
    {
        // Enable InputActions and subscribe to the relevant actions
        inputActions.Enable();
        inputActions.Gameplay.PreviousInstrument.performed += OnPreviousInstrument;
        inputActions.Gameplay.NextInstrument.performed += OnNextInstrument;
    }

    void OnDisable()
    {
        // Unsubscribe from actions and disable InputActions
        inputActions.Gameplay.PreviousInstrument.performed -= OnPreviousInstrument;
        inputActions.Gameplay.NextInstrument.performed -= OnNextInstrument;
        inputActions.Disable();
    }

    private void OnPreviousInstrument(InputAction.CallbackContext context)
    {
        // Call the method to switch to the previous instrument
        ringManager.PreviousInstrument();
    }

    private void OnNextInstrument(InputAction.CallbackContext context)
    {
        // Call the method to switch to the next instrument
        ringManager.NextInstrument();
    }
}


