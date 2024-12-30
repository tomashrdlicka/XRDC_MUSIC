using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Check if we hit a NoteCell
                NoteCell noteCell = hit.collider.GetComponent<NoteCell>();
                if (noteCell != null)
                {
                    noteCell.ToggleNote();
                }
            }
        }
    }
}
