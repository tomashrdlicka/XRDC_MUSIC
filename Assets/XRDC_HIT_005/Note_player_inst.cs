using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class NotePlayer : MonoBehaviour
{
    public RingGridManager gridManager;
    public AudioSource audioSourcePrefab;
    public float noteDuration = 0.5f;

    [Header("Instrument Clips")]
    public AudioClip[] pianoClips;
    public AudioClip[] guitarClips;
    // TODO: Add more arrays if you have more instruments

    private InputActions inputActions;

    void Awake()
    {
        // Initialize the InputActions
        inputActions = new InputActions();
    }

    void OnEnable()
    {
        // Enable InputActions and subscribe to the PlayComposition action
        inputActions.Enable();
        inputActions.Gameplay.PlayComposition.performed += OnPlayComposition;
    }

    void OnDisable()
    {
        // Unsubscribe and disable InputActions
        inputActions.Gameplay.PlayComposition.performed -= OnPlayComposition;
        inputActions.Disable();
    }

    private void OnPlayComposition(InputAction.CallbackContext context)
    {
        // Trigger PlayComposition when the input is performed
        PlayComposition();
    }

    public void PlayComposition()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        int rows = gridManager.rows;
        int columns = gridManager.columns;

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                NoteCell cell = gridManager.cells[r, c];

                // Check every instrument in the enum
                foreach (InstrumentType instrument in System.Enum.GetValues(typeof(InstrumentType)))
                {
                    if (cell.HasNoteForInstrument(instrument))
                    {
                        int pitchIndex = cell.GetPitchIndexForInstrument(instrument);
                        float volume = cell.GetVolumeForInstrument(instrument);

                        // Get the correct clip
                        AudioClip chosenClip = GetClipByInstrument(instrument, pitchIndex);

                        if (chosenClip != null)
                        {
                            Debug.Log($" Row: {r}, Column: {c}  Playing Note - Instrument: {instrument}, PitchIndex: {pitchIndex}, Volume: {volume:F2}");
                            AudioSource newSource = Instantiate(audioSourcePrefab, transform);
                            newSource.clip = chosenClip;
                            newSource.volume = volume;
                            newSource.Play();

                            Destroy(newSource.gameObject, noteDuration + 0.1f);
                        }
                        else
                        {
                            Debug.LogWarning($"No clip set for {instrument} pitchIndex={pitchIndex}");
                        }
                    }
                }
            }
            yield return new WaitForSeconds(noteDuration);
        }
    }

    public void PlaySingleNote(NoteCell cell)
    {
        InstrumentType instrument = cell.GetActiveInstrument();
        if (cell.HasNoteForInstrument(instrument))
        {
            int pitchIndex = cell.GetPitchIndexForInstrument(instrument);
            float volume = cell.GetVolumeForInstrument(instrument);

            AudioClip chosenClip = GetClipByInstrument(instrument, pitchIndex);
            if (chosenClip != null)
            {
                AudioSource newSource = Instantiate(audioSourcePrefab, transform);
                newSource.clip = chosenClip;
                newSource.volume = volume;
                newSource.Play();

                Destroy(newSource.gameObject, noteDuration + 0.1f);
            }
        }
    }

    private AudioClip GetClipByInstrument(InstrumentType instrument, int pitchIndex)
    {
        switch (instrument)
        {
            case InstrumentType.Piano:
                if (pitchIndex >= 0 && pitchIndex < pianoClips.Length)
                    return pianoClips[pitchIndex];
                break;

            case InstrumentType.Guitar:
                if (pitchIndex >= 0 && pitchIndex < guitarClips.Length)
                    return guitarClips[pitchIndex];
                break;

            // TODO: Add more instruments here
        }

        return null;
    }
}



