using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    private Camera mainCamera;

    // We'll keep track of the currently selected NoteCell if the user is holding right-mouse
    private NoteCell currentlySelectedCell = null;
    private Vector3 lastMousePosition;

    // Adjust how sensitive volume changes are (larger => more rapid volume changes).
    [SerializeField] private float volumeSensitivity = 0.005f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 1) LEFT MOUSE for toggling On/Off
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                NoteCell noteCell = hit.collider.GetComponent<NoteCell>();
                if (noteCell != null)
                {
                    noteCell.ToggleNote();
                }
            }
        }

        // 2) RIGHT MOUSE for Volume Control
        //    (a) On Right-Mouse Down, see if we hit a NoteCell.
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                NoteCell noteCell = hit.collider.GetComponent<NoteCell>();
                if (noteCell != null)
                {
                    currentlySelectedCell = noteCell;
                }
            }
            lastMousePosition = Input.mousePosition; 
        }

        //    (b) While Right-Mouse is Held, adjust volume based on vertical mouse movement.
        if (Input.GetMouseButton(1) && currentlySelectedCell != null)
        {
            Vector3 currentMousePos = Input.mousePosition;
            
            // Positive deltaY => moved mouse up => increase volume
            // Negative deltaY => moved mouse down => decrease volume
            float deltaY = currentMousePos.y - lastMousePosition.y;
            
            // Convert deltaY to some fraction
            float volumeChange = deltaY * volumeSensitivity;

            // Adjust volume on the currently selected cell
            currentlySelectedCell.AdjustVolume(volumeChange);

            // Update last mouse position
            lastMousePosition = currentMousePos;
        }

        //    (c) When Right-Mouse is Released, stop adjusting volume
        if (Input.GetMouseButtonUp(1))
        {
            currentlySelectedCell = null;
        }
    }
}
