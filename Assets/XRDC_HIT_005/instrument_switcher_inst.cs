using UnityEngine;

public class InstrumentSwitcherNew : MonoBehaviour
{
    [Tooltip("Reference to the ring manager that creates and holds NoteCells.")]
    public RingGridManager ringManager;  // Assign in Inspector
    
    void Update()
    {
        // Press J to go to previous instrument on all cells
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (NoteCell cell in ringManager.cells)
            {
                cell.PreviousInstrument();
            }
        }

        // Press K to go to next instrument on all cells
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("K key pressed: Switching to the previous instrument");
            foreach (NoteCell cell in ringManager.cells)
            {
                cell.NextInstrument();
            }
        }
    }
}


