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

    private Vector3 originalPosition;

   

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

        //UpdateCellOn();
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
    private Coroutine scaleCoroutine; // To track the running coroutine

    private void UpdateCellOn()
    {
        // Check if the cell has a note
        if (HasNote())
        {
            // Start expanding the prefab
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine); // Stop any running coroutine to avoid conflicts
            }
            scaleCoroutine = StartCoroutine(ScalePrefab(1.10f, 0.15f)); // Expand to 105% over 0.5 seconds
        }
        else
        {
            // Reset to original size if no note is present
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            transform.localScale = Vector3.one; // Reset to original size
        }
    }

    private IEnumerator ScalePrefab(float targetScaleMultiplier, float duration)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * targetScaleMultiplier;

        float elapsedTime = 0f;

        // Smoothly scale up
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.localScale = targetScale;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Smoothly scale back down
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.localScale = originalScale;
    }


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
            meshRenderer.material.color = new Color(1f, 1f, 1f, 0.1f);   // faint white
            transform.position -= directionFromCenter * offset;
        }
    }

    public void ResetToOriginalPosition()
{
    meshRenderer.material.color = new Color(1f, 1f, 1f, 0.1f);
    transform.position = originalPosition;
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






