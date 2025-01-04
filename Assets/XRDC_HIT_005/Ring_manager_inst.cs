using UnityEngine;

public class RingGridManager : MonoBehaviour
{
    [Header("Ring Settings")]
    public int rows = 7;               // Number of vertical cells in each segment
    public int columns = 12;           // Number of segments (columns) around the ring
    public float radius = 5f;          // Radius of the ring
    public float verticalSpacing = 1f; // Spacing between vertical cells
    public GameObject cellPrefab;      // Prefab for each cell

    public NoteCell[,] cells;          // 2D array to hold references to cell objects
    public NotePlayer notePlayer; 

    void Start()
    {
        // Initialize the 2D array to match the rows and columns
        cells = new NoteCell[rows, columns];

        // Generate the ring with vertical stacks
        for (int c = 0; c < columns; c++)
        {
            // Calculate the angle for this column
            float angle = 2 * Mathf.PI * c / columns;

            // Compute the base position for this column on the ring
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            for (int r = 0; r < rows; r++)
            {
                // Calculate the vertical offset for each row
                float y = r * verticalSpacing;

                // Final position for each cell
                Vector3 position = new Vector3(x, y, z);

                // Instantiate the prefab and set its parent to this manager
                GameObject newCellGO = Instantiate(cellPrefab, position, Quaternion.identity, transform);

                // Rotate the cell to face the center of the ring
                newCellGO.transform.LookAt(new Vector3(0, y, 0));

                // Optional: Adjust rotation for better alignment
                newCellGO.transform.Rotate(0, 0, 90);

                // Name the cell for clarity
                newCellGO.name = $"Cell_{r}_{c}";

                // Create a reference to the NoteCell script
                NoteCell cellScript = newCellGO.GetComponent<NoteCell>();
                cellScript.gridRow = r;
                cellScript.gridColumn = c;
                cellScript.notePlayer = notePlayer;

                // Store the cell in the 2D array
                cells[r, c] = cellScript;
            }
        }
    }
}


