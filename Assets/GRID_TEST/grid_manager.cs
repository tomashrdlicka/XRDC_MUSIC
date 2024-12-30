using UnityEngine;

public class NoteGridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int rows = 7;          // Because you have 7 possible pitches
    public int columns = 8;       // Adjust columns to however many time steps you want
    public float cellSize = 0.1f; // The size (in meters) of each cell
    public GameObject cellPrefab; // A prefab with a collider and script
    
    public NoteCell[,] cells;     // 2D array to hold references to cell objects
    
    void Start()
    {
        cells = new NoteCell[rows, columns];
        
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // Position each cell so that row 0 is at the bottom or top as you see fit
                Vector3 position = new Vector3(c * cellSize, r * cellSize, 0f);
                GameObject newCellGO = Instantiate(cellPrefab, transform);
                newCellGO.transform.localPosition = position;
                
                // Optionally give it a name for debugging
                newCellGO.name = $"Cell_{r}_{c}";
                
                // Get the NoteCell script reference
                NoteCell cellScript = newCellGO.GetComponent<NoteCell>();
                cellScript.gridRow = r;
                cellScript.gridColumn = c;
                
                cells[r, c] = cellScript;
            }
        }
    }
}
