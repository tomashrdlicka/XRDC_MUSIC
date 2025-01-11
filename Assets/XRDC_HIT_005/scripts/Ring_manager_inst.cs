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

    public void LightUpColumn(int columnIndex)
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
                previousCell.SequenceColor(false); // Reset to original color
            }
        }
    }

    // Light up the new column
    for (int r = 0; r < rows; r++)
    {
        NoteCell newCell = cells[r, columnIndex];
        if (newCell != null)
        {
            newCell.SequenceColor(true); // Light up with desired color
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
                    cell.SequenceColor(false); // Reset to original color
                }
            }
        }
        currentLitColumn = -1; // Reset the lit column tracker
    }



    public void CheckNextColumn(int currentColumn)
    {
        // Check if the currentColumn is the last column
        if (currentColumn + 1 < columnsEnabled.Length)
        {
            // Proceed only if the next column exists
            if (columnsEnabled[currentColumn + 1] == false)
            {
                for (int r = 0; r < rows; r++)
                {
                    cells[r, currentColumn + 1].EnableCell();
                }

                // Mark the column as enabled
                columnsEnabled[currentColumn + 1] = true;
            }
        }
        else
        {
            Debug.LogWarning("No next column exists. Already at the last column.");
        }
    }
    // If user triggers "next instrument" globally, we call:
    public void NextInstrument()
    {
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

}




