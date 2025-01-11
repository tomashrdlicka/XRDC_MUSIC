using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(BoxCollider))]
public class NoteCell : MonoBehaviour
{
    // --- References ---
    [Header("References")]
    public MeshRenderer meshRenderer;
    public BoxCollider boxCollider;
    public NotePlayer notePlayer;             // Reference to your NotePlayer script
    private RingGridManager ringGridManager;  // Found automatically in Awake (parent)

    // --- Grid Indices ---
    [Header("Grid Settings")]
    public int gridRow;
    public int gridColumn;

    // --- Instrument Management ---
    [SerializeField] private InstrumentType activeInstrument;

    // A dictionary from instrument type to that instrument's note data
    private Dictionary<InstrumentType, InstrumentNoteData> instrumentData;

    // --- Child Renderers (e.g., for Piano, Lead, Bass, Drums) ---
    private Renderer pianoRenderer;
    private Renderer leadRenderer;
    private Renderer bassRenderer;
    private Renderer drumsRenderer;

   

    // --- Unity Lifecycle ---
    private void Awake()
    {
        // 1. Find the RingGridManager in the parent chain
        ringGridManager = GetComponentInParent<RingGridManager>();
        if (ringGridManager == null)
        {
            Debug.LogError($"RingGridManager not found for {gameObject.name}. " +
                           "Ensure this cell is a child of the RingGridManager GameObject.");
        }

        // 2. Initialize the instrument data dictionary
        instrumentData = new Dictionary<InstrumentType, InstrumentNoteData>();
        foreach (InstrumentType instType in Enum.GetValues(typeof(InstrumentType)))
        {
            instrumentData[instType] = new InstrumentNoteData();
        }

        // 3. Find child renderers by name (if they exist)
        pianoRenderer = TryGetChildRenderer("Piano_Prefab");
        leadRenderer  = TryGetChildRenderer("Lead_Prefab");
        bassRenderer  = TryGetChildRenderer("Bass_Prefab");
        drumsRenderer = TryGetChildRenderer("Drums_Prefab");

        // 4. Get references to this GameObject's MeshRenderer and BoxCollider
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider  = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        // Set default instrument
        activeInstrument = InstrumentType.Piano;

        // Initial visual updates
        
    }

    // --- Child Renderer Helper ---
   private Renderer TryGetChildRenderer(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            return child.GetComponent<Renderer>();
        }
        return null;
    }

    // --- Public API to Link to Manager ---
    public void SetRingGridManager(RingGridManager manager)
    {
        ringGridManager = manager;
    }

    // --- Visibility / Interactivity ---
    public void DisableRenderers()
    {
        // Hide the parent MeshRenderer + BoxCollider
        meshRenderer.enabled = false;
        boxCollider.enabled = false;

        // Hide all child MeshRenderers (if any)
        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in childRenderers)
        {
            mr.enabled = false;
        }
    }

    public void EnableCell()
    {
        // Show the parent MeshRenderer + BoxCollider
        meshRenderer.enabled = true;
        boxCollider.enabled = true;

        // Show all child MeshRenderers
    }


    // --- Note / Instrument Logic ---
    public void ToggleNote()
    {
        bool oldHasNote = instrumentData[activeInstrument].hasNote;
        instrumentData[activeInstrument].hasNote = !oldHasNote;

        ringGridManager.CheckNextColumn(this.gridColumn);

        if (instrumentData[activeInstrument].hasNote)
        {
            // Set default volume
            instrumentData[activeInstrument].volume = 0.5f;
            // Optionally trigger NotePlayer
            notePlayer?.PlaySingleNote(this);
        }
        else
        {
            // Turn off volume if note is removed
            instrumentData[activeInstrument].volume = 0f;
        }

        UpdateColor();
        UpdateChildVisuals();
    }

    public void SetInstrument(InstrumentType newInstrument)
    {
        activeInstrument = newInstrument;
    }

    public void InitializeInstrumentData(InstrumentType instrumentType, bool hasNote, float volume, int pitchIndex)
    {
        instrumentData[instrumentType].hasNote = hasNote;
        instrumentData[instrumentType].volume = volume;
        instrumentData[instrumentType].pitchIndex = pitchIndex;

        // Optionally update visuals
        UpdateColor();
        UpdateChildVisuals();
    }

    public void AdjustVolume(float delta)
    {
        if (!HasNote()) return; // no note to adjust

        float newVolume = GetVolume() + delta;
        instrumentData[activeInstrument].volume = Mathf.Clamp01(newVolume);
        UpdateColor();
        UpdateChildVisuals();
    }

    // --- Note Data Helpers ---
    private bool HasNote() => instrumentData[activeInstrument].hasNote;
    private float GetVolume() => instrumentData[activeInstrument].volume;
    private void SetVolume(float newVolume) => instrumentData[activeInstrument].volume = Mathf.Clamp01(newVolume);
    private int GetPitchIndex() => instrumentData[activeInstrument].pitchIndex;

    public void SetPitchIndex(int newPitchIndex)
    {
        instrumentData[activeInstrument].pitchIndex = newPitchIndex;
    }

    public void SetPitchIndices(int newPitchIndex)
    {
        // Set for all instruments
        foreach (InstrumentType instType in Enum.GetValues(typeof(InstrumentType)))
        {
            if (instrumentData.ContainsKey(instType))
            {
                instrumentData[instType].pitchIndex = newPitchIndex;
            }
        }
    }

    // --- Public Getters for other scripts ---
    public bool HasNoteForInstrument(InstrumentType instrument) => instrumentData[instrument].hasNote;
    public float GetVolumeForInstrument(InstrumentType instrument) => instrumentData[instrument].volume;
    public int GetPitchIndexForInstrument(InstrumentType instrument) => instrumentData[instrument].pitchIndex;
    public InstrumentType GetActiveInstrument() => activeInstrument;

    // --- Visual Updates ---
    private void UpdateColor()
    {
        // Change the primary MeshRenderer color based on whether there's a note
        if (HasNote())
        {
            meshRenderer.material.color = new Color(1f, 0.843f, 0f, 1f); // gold
        }
        else
        {
            meshRenderer.material.color = new Color(1f, 1f, 1f, 0.1f);   // faint white
        }
    }

    public void SequenceColor( bool switchColor)
    {
        // Change the primary MeshRenderer color based on whether there's a note
        if (switchColor)
        {
            meshRenderer.material.color = new Color(1f, 0.843f, 0f, 1f); // gold
        }
        else
        {
            meshRenderer.material.color = new Color(1f, 1f, 1f, 0.1f);   // faint white
        }
    }

    private void UpdateChildVisuals()
    {
        // Piano
        bool pianoHasNote = instrumentData[InstrumentType.Piano].hasNote;
        if (pianoRenderer != null)
        {
                pianoRenderer.enabled = pianoHasNote;
               
        }

        // Lead
        bool leadHasNote = instrumentData[InstrumentType.Lead].hasNote;
        if (leadRenderer != null)
        {
            leadRenderer.enabled = leadHasNote;
        }

        // Bass
        bool bassHasNote = instrumentData[InstrumentType.Bass].hasNote;
        if (bassRenderer != null)
        {
            bassRenderer.enabled = bassHasNote;
        }

        // Drums
        bool drumsHasNote = instrumentData[InstrumentType.Drums].hasNote;
        if (drumsRenderer != null)
        {
            drumsRenderer.enabled = drumsHasNote;
        }
    }

}






