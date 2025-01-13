using UnityEngine;

public class RingGridManager : MonoBehaviour
{
    [Header("Ring Settings")]
    public int rows = 7;              
    public int columns = 12;          
    public float radius = 5f;         
    public float verticalSpacing = 1f;
    public GameObject cellPrefab;     
    public NoteCell[,] cells;         
    public NotePlayer notePlayer;

    // This is the "global" instrument that the user can switch
    // from the UI or keyboard
    [SerializeField] private InstrumentType globalInstrument;
    [SerializeField] private bool canSwitchInstruments = false;
    [SerializeField] private bool canReset = false;

    private bool[] columnsEnabled; // Track whether each row is enabled

    void Start()
    {


        columnsEnabled = new bool[columns];
        // 2. By default, only row 0 is enabled
        columnsEnabled[0] = true;

        // Initialize the 2D array to match the rows and columns
        cells = new NoteCell[rows, columns];

        // Generate the ring with vertical stacks
        for (int c = 0; c < columns; c++)
        {
            float angle = 2 * Mathf.PI * c / columns;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            for (int r = 0; r < rows; r++)
            {
                float y = r * verticalSpacing;
                Vector3 position = new Vector3(x, y, z);

                GameObject newCellGO = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                newCellGO.transform.LookAt(new Vector3(0, y, 0));
                newCellGO.transform.Rotate(0, 0, 0);
                newCellGO.name = $"Cell_{r}_{c}";

                NoteCell cellScript = newCellGO.GetComponent<NoteCell>();
                cellScript.gridRow = r;
                cellScript.gridColumn = c;
                cellScript.notePlayer = notePlayer;
                cellScript.SetPitchIndices(r);

                cellScript.DisableRenderers();
                
                cells[r, c] = cellScript;

                if (c == 0)
                {
                    cellScript.EnableCell();
                }
            }
        }
    }

    private int currentLitColumn = -1; // Track the currently lit column

    public void MoveColumn(int columnIndex)
{
    // Validate the column index
    if (columnIndex < 0 || columnIndex >= columns)
    {
        Debug.LogError($"Invalid column index: {columnIndex}. It must be between 0 and {columns - 1}.");
        return;
    }

    // Reset the previous column's color
    if (currentLitColumn != -1)
    {
        for (int r = 0; r < rows; r++)
        {
            NoteCell previousCell = cells[r, currentLitColumn];
            if (previousCell != null)
            {
                previousCell.SequenceMove(false); // Reset to original color
            }
        }
    }

    // Light up the new column
    for (int r = 0; r < rows; r++)
    {
        NoteCell newCell = cells[r, columnIndex];
        if (newCell != null)
        {
            newCell.SequenceMove(true); // Light up with desired color
        }
    }

    // Update the currently lit column
    currentLitColumn = columnIndex;

    // Do not reset all columns here, as this disrupts smooth infinite looping
}


    public void ResetAllColumns()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                NoteCell cell = cells[r, c];
                if (cell != null)
                {
                    cell.ResetToOriginalPosition();
                }
            }
        }
        currentLitColumn = -1; // Reset the lit column tracker
    }


    // If user triggers "next instrument" globally, we call:
    public void NextInstrument()
    {
        if (!canSwitchInstruments)
        {
            Debug.Log("Instrument switching not allowed right now.");
            return;
        }
        int maxInstrumentCount = System.Enum.GetValues(typeof(InstrumentType)).Length;
        int currentIndex = (int)globalInstrument;
        currentIndex = (currentIndex + 1) % maxInstrumentCount;
        globalInstrument = (InstrumentType)currentIndex;

        // Now tell each cell to switch to that instrument
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                cells[r,c].SetInstrument(globalInstrument);
            }
        }

        Debug.Log("Global instrument switched -> " + globalInstrument);
    }

    public void PreviousInstrument()
    {
        if (!canSwitchInstruments)
        {
            Debug.Log("Instrument switching not allowed right now.");
            return;
        }
        int maxInstrumentCount = System.Enum.GetValues(typeof(InstrumentType)).Length;
        int currentIndex = (int)globalInstrument;
        currentIndex = (currentIndex - 1 + maxInstrumentCount) % maxInstrumentCount;
        globalInstrument = (InstrumentType)currentIndex;

        // Now tell each cell to switch to that instrument
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                cells[r,c].SetInstrument(globalInstrument);
            }
        }

        Debug.Log("Global instrument switched -> " + globalInstrument);
    }

    public InstrumentType GetGlobalInstrument()
    {
        return globalInstrument;
    }
    public void SetSwitchInstruments(bool can)
    {
        canSwitchInstruments = can;
    }
    public void SetReset(bool can)
    {
        canReset = can;
    }

    public void ResetAll()
{
    if (!canReset)
        {
            Debug.Log("Instrument switching not allowed right now.");
            return;
        }

    Debug.Log("Resetting the entire grid to its original state...");

    // 1. Reset all cells to their original state
    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < columns; c++)
        {
            NoteCell cell = cells[r, c];
            if (cell != null)
            {
                cell.ResetToOriginalPosition(); // Reset position and visual state
                //cell.DisableRenderers();        // Disable rendering
                cell.InitializeInstrumentData(InstrumentType.Piano, false, 0f, 0); // Clear note data
                cell.InitializeInstrumentData(InstrumentType.Bass, false, 0f, 0);
                cell.InitializeInstrumentData(InstrumentType.Lead, false, 0f, 0);
                cell.InitializeInstrumentData(InstrumentType.Pad, false, 0f, 0);
            }
        }
    }

    // 2. Reset column tracking
    currentLitColumn = -1;                 // No column is lit
    columnsEnabled = new bool[columns];   // Reinitialize column tracking
    columnsEnabled[0] = true;             // Enable only the first column

    // 3. Reset the global instrument to its default
    globalInstrument = InstrumentType.Piano; // Change this to your desired default instrument
    Debug.Log($"Global instrument reset to {globalInstrument}.");

    // 4. Re-enable the first column's cells
    for (int r = 0; r < rows; r++)
    {
        cells[r, 0].EnableCell();
    }

    Debug.Log("Grid reset complete.");
    

}

private void HandResetAll()
{
    ResetAll();
}


}




