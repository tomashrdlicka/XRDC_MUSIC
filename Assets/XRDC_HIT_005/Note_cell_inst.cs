using UnityEngine;
using System.Collections.Generic;



public class NoteCell : MonoBehaviour
{
    
    private MeshRenderer meshRenderer;
    public NotePlayer notePlayer;
    public int gridRow;        // Existing: row index in the grid
    public int gridColumn;
    // -------------------------------------------------------------
    //Instrument
    // -------------------------------------------------------------
    [SerializeField] private InstrumentType activeInstrument;
    // -------------------------------------------------------------

    // A dictionary from instrument type to that instrument's note data
    private Dictionary<InstrumentType, InstrumentNoteData> instrumentData;
  

    [System.Serializable]
    public class InstrumentNoteData
    {
        public bool hasNote;
        public float volume;
        public int pitchIndex;

        public InstrumentNoteData(bool hasNote = false, float volume = 0f, int pitchIndex = 0)
        {
            this.hasNote = hasNote;
            this.volume = volume;
            this.pitchIndex = pitchIndex;
        }
    }

    private Renderer pianoRenderer;
    private Renderer guitarRenderer;
    private Renderer bassRenderer;
    private Renderer drumsRenderer;

    private Color pianoOriginalColor;
    private Color guitarOriginalColor;
    private Color bassOriginalColor;
    private Color drumsOriginalColor;

    // Existing color fields
    [SerializeField] private Color baseGreen = new Color(0f, 0.3f, 0f, 1f); 
    [SerializeField] private Color maxGreen = Color.green;
    [SerializeField] private Color baseRed = new Color(0.3f, 0f, 0f, 1f);  // Darker red
    [SerializeField] private Color maxRed = Color.red;                    // Full red (Unity's predefined red)
    

    void Awake()
    {
        // Initialize the dictionary with one entry per instrument
        instrumentData = new Dictionary<InstrumentType, InstrumentNoteData>();
        foreach (InstrumentType instType in System.Enum.GetValues(typeof(InstrumentType)))
        {
            instrumentData[instType] = new InstrumentNoteData();
        }

            // Attempt to find the child by name
        Transform pianoChild = transform.Find("Piano_Prefab");
        if (pianoChild != null)
            pianoRenderer = pianoChild.GetComponent<Renderer>();
            Debug.Log("found piano child");

        // Repeat for the others
        Transform guitarChild = transform.Find("Guitar_Prefab");
        if (guitarChild != null)
            guitarRenderer = guitarChild.GetComponent<Renderer>();
            Debug.Log("found guitar child");
        
    }

    void Start()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();

        // Set a default state for the first active instrument
        activeInstrument = InstrumentType.Piano;

        if (pianoRenderer != null)
            pianoOriginalColor = pianoRenderer.material.color;

        if (guitarRenderer != null)
            guitarOriginalColor = guitarRenderer.material.color;

        // Perform any post-initialization operations
        UpdateColor();
        UpdateChildVisuals();
    }

    //get and set cell properties
    private bool HasNote()
    {
        return instrumentData[activeInstrument].hasNote;
    }

    private float GetVolume()
    {
        return instrumentData[activeInstrument].volume;
    }

    private void SetVolume(float newVolume)
    {
        instrumentData[activeInstrument].volume = Mathf.Clamp01(newVolume);
    }

    private int GetPitchIndex()
    {
        return instrumentData[activeInstrument].pitchIndex;
    }

    public void SetPitchIndex(int newPitchIndex)   
    {   
        instrumentData[activeInstrument].pitchIndex = newPitchIndex;
    }
    public void SetPitchIndices(int newPitchIndex)
    {
        foreach (InstrumentType instType in System.Enum.GetValues(typeof(InstrumentType)))
        {
        if (instrumentData.ContainsKey(instType))
        {  
            instrumentData[instType].pitchIndex = newPitchIndex;
        }
    }
}


    public void InitializeInstrumentData(InstrumentType instrumentType, bool hasNote, float volume, int pitchIndex)
{
    // You can set data on ALL instruments or just one; here’s just the active one
    instrumentData[instrumentType].hasNote = hasNote;
    instrumentData[instrumentType].volume = volume;
    instrumentData[instrumentType].pitchIndex = pitchIndex;

    // Optionally update color if you want
    UpdateColor();
    UpdateChildVisuals();
}
    // Toggle note on/off
    public void ToggleNote()
    {
        // Flip hasNote for the CURRENT instrument
        bool oldHasNote = instrumentData[activeInstrument].hasNote;
        instrumentData[activeInstrument].hasNote = !oldHasNote;
        
        if (instrumentData[activeInstrument].hasNote)
        {
            // If we just turned on the note, set default volume
            instrumentData[activeInstrument].volume = 0.5f;

            // Tell the NotePlayer to play (or schedule) the note
            // NOTE: That method might still want the pitch index 
            // or volume from the dictionary
            notePlayer.PlaySingleNote(this);
        }
        else
        {
            // If no note, set volume to 0
            instrumentData[activeInstrument].volume = 0f;
        }
        UpdateColor();
        UpdateChildVisuals();
    }

    // Update color reflecting note volume
     private void UpdateColor()
    {
        if (HasNote())
        {
            // Example color logic depending on which instrument
            // is currently selected for this cell
            switch (activeInstrument)
            {
                case InstrumentType.Piano:
                    // Simple color interpolation from dark green -> bright green
                    float pianoV = GetVolume(); 
                    meshRenderer.material.color = new Color(
                        0f,
                        0.3f + (0.7f * pianoV),
                        0f,
                        1f
                    );
                    break;

                case InstrumentType.Guitar:
                    // Simple color interpolation from dark red -> bright red
                    float guitarV = GetVolume();
                    meshRenderer.material.color = new Color(
                        0.3f + (0.7f * guitarV),
                        0.3f,
                        0f,
                        1f
                    );
                    break;
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
        if (!HasNote()) return; // Only adjust volume if there's a note

        float volume = GetVolume() + delta;
        volume = Mathf.Clamp01(volume); // Keep volume in [0, 1]
        SetVolume(volume);
        UpdateColor();
        UpdateChildVisuals();
    }

    // -------------------------------------------------------------
    // NEW METHODS: Changing Instrument and Pitch
    // -------------------------------------------------------------
    public void SetInstrument(InstrumentType newInstrument)
    {
        activeInstrument = newInstrument;
    }





    // -------------------------------------------------------------
    //public getters
    // Return whether the cell has a note for the given instrument
    public bool HasNoteForInstrument(InstrumentType instrument)
    {
        return instrumentData[instrument].hasNote;
    }

    // Return the volume for the given instrument
    public float GetVolumeForInstrument(InstrumentType instrument)
    {
        return instrumentData[instrument].volume;
    }

    // Return the pitch index for the given instrument
    public int GetPitchIndexForInstrument(InstrumentType instrument)
    {
        return instrumentData[instrument].pitchIndex;
    }

    public InstrumentType GetActiveInstrument() => activeInstrument;


    private void UpdateChildVisuals()
    {
        // For PIANO child
        if (pianoRenderer != null)
        {
            // If the dictionary says there's a note for Piano
            bool pianoHasNote = instrumentData[InstrumentType.Piano].hasNote;

            // Example: Multiply original color by 2.0f to brighten it
            if (pianoHasNote)
                pianoRenderer.material.color = pianoOriginalColor * 3.0f;
            else
                pianoRenderer.material.color = pianoOriginalColor;
        }

        // For GUITAR child
        if (guitarRenderer != null)
        {
            bool guitarHasNote = instrumentData[InstrumentType.Guitar].hasNote;

            if (guitarHasNote)
                guitarRenderer.material.color = guitarOriginalColor * 2.0f;
            else
                guitarRenderer.material.color = guitarOriginalColor;
        }

    }

}




