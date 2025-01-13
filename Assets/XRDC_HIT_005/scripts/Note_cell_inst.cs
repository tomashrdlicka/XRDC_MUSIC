using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MeshRenderer), typeof(BoxCollider))]
public class NoteCell : MonoBehaviour
{
    // --- References ---
    [Header("References")]
    public MeshRenderer meshRenderer;
    public BoxCollider boxCollider;
    public NotePlayer notePlayer;             // Reference to your NotePlayer script
    private RingGridManager ringGridManager;  // Found automatically in Awake (parent)
    private OnboardingManager onboardingManager;

    // --- Grid Indices ---
    [Header("Grid Settings")]
    public int gridRow;
    public int gridColumn;

    // --- Instrument Management ---
    [SerializeField] private InstrumentType activeInstrument;

    // A dictionary from instrument type to that instrument's note data
    private Dictionary<InstrumentType, InstrumentNoteData> instrumentData;

    // --- Child Renderers (e.g., for Piano, Lead, Bass, Pad) ---
    private Renderer pianoRenderer;
    private Renderer leadRenderer;
    private Renderer bassRenderer;
    private Renderer padRenderer;
    public Renderer controlRenderer;

    private MeshRenderer[] cornerRenderers;


    private Vector3 originalPosition;

   

    // --- Unity Lifecycle ---
    private void Awake()
    {
        // 1. Find the RingGridManager in the parent chain
        ringGridManager = GetComponentInParent<RingGridManager>();

        onboardingManager = FindObjectOfType<OnboardingManager>();

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
        padRenderer = TryGetChildRenderer("Pad_Prefab");
        controlRenderer = TryGetChildRenderer("Control_Sphere");

         Transform corners = transform.Find("Corners");
         cornerRenderers = corners.GetComponentsInChildren<MeshRenderer>();

        // 4. Get references to this GameObject's MeshRenderer and BoxCollider
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider  = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        // Set default instrument
        activeInstrument = InstrumentType.Piano;
        originalPosition = transform.position;

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
        meshRenderer.material.color = new Color(1f, 1f, 1f, 0.03f);   // faint white
        meshRenderer.enabled = true;
        boxCollider.enabled = false;
        SetCornerVisibility(false);

        // Hide all child MeshRenderers (if any)
        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in childRenderers)
        {
            mr.enabled = false;
        }
    }

    public void SetCornerVisibility(bool isVisible)
    {
        if (cornerRenderers != null)
        {
            foreach (MeshRenderer renderer in cornerRenderers)
            {
                renderer.enabled = isVisible;
            }
        }
    }

    public void EnableCell()
    {
        // Show the parent MeshRenderer + BoxCollider
        //meshRenderer.enabled = true;
        boxCollider.enabled = true;
        controlRenderer.enabled = true;
        SetCornerVisibility(true);

        // Show all child MeshRenderers
    }


    // --- Note / Instrument Logic ---
    public void ToggleNote()
    {
        if (onboardingManager.onboardingMode && ringGridManager.GetGlobalInstrument() != onboardingManager.requiredInstrument)
        {
            onboardingManager.PlayNarrativeClip(19); 
            Debug.Log("You're not on the correct instrument right now!");
            return;
        }

        bool oldHasNote = instrumentData[activeInstrument].hasNote;
        instrumentData[activeInstrument].hasNote = !oldHasNote;

        if (instrumentData[activeInstrument].hasNote)
        {
            //if in onboard mode
            OnboardingManager onboardingManager = FindObjectOfType<OnboardingManager>();
            if (onboardingManager != null)
            {
                onboardingManager.OnCellPressed(this);
            }
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
        UpdateChildVisuals();
    }

    public void AdjustVolume(float delta)
    {
        if (!HasNote()) return; // no note to adjust

        float newVolume = GetVolume() + delta;
        instrumentData[activeInstrument].volume = Mathf.Clamp01(newVolume);
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

    public void SequenceMove( bool switchColor)
    {
        // Change the primary MeshRenderer color based on whether there's a note
        float offset = 0.2f; 
        Vector3 directionFromCenter = transform.position - transform.parent.position;
        directionFromCenter.Normalize();

        if (switchColor)
        {
            meshRenderer.material.color = new Color(1f, 0.843f, 0f, 1f); // gold
            transform.position += directionFromCenter * offset;
        }
        else
        {
            meshRenderer.material.color = new Color(1f, 1f, 1f, 0.03f);   // faint white
            transform.position -= directionFromCenter * offset;
        }
    }

    public void ResetToOriginalPosition()
{
    meshRenderer.material.color = new Color(1f, 1f, 1f, 0.1f);
    transform.position = originalPosition;
}

private void UpdateRendererAndChild(Renderer parentRenderer, bool isEnabled)
{
    if (parentRenderer != null)
    {
        // Enable or disable the parent renderer
        parentRenderer.enabled = isEnabled;

        // Enable or disable the child renderer (exactly one child assumed)
        Transform child = parentRenderer.transform.childCount > 0 ? parentRenderer.transform.GetChild(0) : null;
        if (child != null)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = isEnabled;
            }
        }
    }
}

    private void UpdateChildVisuals()
    {
        // Update the parent and child renderers for each instrument

        // Piano
        bool pianoHasNote = instrumentData[InstrumentType.Piano].hasNote;
        UpdateRendererAndChild(pianoRenderer, pianoHasNote);

        // Lead
        bool leadHasNote = instrumentData[InstrumentType.Lead].hasNote;
        UpdateRendererAndChild(leadRenderer, leadHasNote);

        // Bass
        bool bassHasNote = instrumentData[InstrumentType.Bass].hasNote;
        UpdateRendererAndChild(bassRenderer, bassHasNote);

        // Padd
        bool padHasNote = instrumentData[InstrumentType.Pad].hasNote;
        UpdateRendererAndChild(padRenderer, padHasNote);
    }


}






