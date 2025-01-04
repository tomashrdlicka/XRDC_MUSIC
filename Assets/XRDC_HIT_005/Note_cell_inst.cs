using UnityEngine;



public class NoteCell : MonoBehaviour
{
    public int gridRow;        // Existing: row index in the grid
    public int gridColumn;     // Existing: column index in the grid
    
    private bool hasNote = false;
    private float volume = 0f; // Volume in range [0.0, 1.0]
    private int maxInstrumentCount = System.Enum.GetValues(typeof(InstrumentType)).Length;
    private MeshRenderer meshRenderer;
    public NotePlayer notePlayer;

    // -------------------------------------------------------------
    // NEW FIELDS: Instrument & Pitch
    // -------------------------------------------------------------
    [SerializeField] private InstrumentType globalInstrument = InstrumentType.Piano;
    [SerializeField] private InstrumentType cellInstrument = InstrumentType.Piano;
    [SerializeField] private int pitchIndex = 0; // You could use this to select from pitch clips
    // -------------------------------------------------------------

    // Existing color fields
    [SerializeField] private Color baseGreen = new Color(0f, 0.3f, 0f, 1f); 
    [SerializeField] private Color maxGreen = Color.green;
    [SerializeField] private Color baseRed = new Color(0.3f, 0f, 0f, 1f);  // Darker red
    [SerializeField] private Color maxRed = Color.red;                    // Full red (Unity's predefined red)


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateColor();
    }

    // Toggle note on/off
    public void ToggleNote()
    {
        hasNote = !hasNote;
        if (hasNote)
        {
            // Default volume to 0.5
            volume = 0.5f;
            cellInstrument = globalInstrument;
            notePlayer.PlaySingleNote(this);

        }
        else
        {
            // If no note, set volume to 0
            volume = 0f;
        }
        UpdateColor();
    }

    // Update color reflecting note volume
    private void UpdateColor()
    {
        if (hasNote)
        {
            if (cellInstrument == InstrumentType.Piano)
            {
            // Simple color interpolation from dark green -> bright green
            meshRenderer.material.color = new Color(
                0f, 
                0.3f + (0.7f * volume), 
                0f, 
                1f
            );    
            }
            if (cellInstrument == InstrumentType.Guitar){
            // Simple color interpolation from dark red -> bright red
            meshRenderer.material.color = new Color(
                0.3f + (0.7f * volume), 
                0.3f, 
                0f, 
                1f
            );    
            }
        }
        else
        {
            // If no note -> white color
            meshRenderer.material.color = Color.white;
        }
    }

    // Adjust volume by a certain amount
    public void AdjustVolume(float delta)
    {
        if (!hasNote) return; // Only adjust volume if there's a note

        volume += delta;
        volume = Mathf.Clamp01(volume); // Keep volume in [0, 1]
        UpdateColor();
    }

    // -------------------------------------------------------------
    // NEW METHODS: Changing Instrument and Pitch
    // -------------------------------------------------------------
    public void SetInstrument(InstrumentType newInstrument)
    {
        globalInstrument = newInstrument;
    }

    public void NextInstrument()
    {
    int currentIndex = (int)globalInstrument;
    currentIndex = (currentIndex + 1) % maxInstrumentCount;
    globalInstrument = (InstrumentType)currentIndex;
    Debug.Log($"instrument switched");
    }

    public void PreviousInstrument()
    {
    int currentIndex = (int)globalInstrument;
    currentIndex = (currentIndex - 1 + maxInstrumentCount) % maxInstrumentCount;
    globalInstrument = (InstrumentType)currentIndex;
    Debug.Log($"instrument switched");
    }

    public void SetPitch(int newPitchIndex)
    {
        pitchIndex = newPitchIndex;
    }

    public void NextPitch(int totalPitches)
    {
        // Cycle through pitch indices up to totalPitches
        pitchIndex = (pitchIndex + 1) % totalPitches;
    }


    // -------------------------------------------------------------

    // Existing getters
    public bool HasNote() => hasNote;
    public float GetVolume() => volume;

    // -------------------------------------------------------------
    // NEW GETTERS: Instrument & Pitch
    // -------------------------------------------------------------
    public InstrumentType GetCellInstrument() => cellInstrument;
    public int GetPitchIndex() => pitchIndex;
    // -------------------------------------------------------------
}




