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
    public AudioClip[] leadClips;
    public AudioClip[] bassClips;
    public AudioClip[] drumsClips;

    private bool infinite;
    private bool stop;
    private bool playing;


    // TODO: Add more arrays if you have more instruments

    private InputActions inputActions;
    public OnboardingManager onboarding; 

    void Awake()
    {
        // Initialize the InputActions
        inputActions = new InputActions();
        infinite = true;
        stop = false;
    }

    void OnEnable()
    {
        // Enable InputActions and subscribe to the PlayComposition action
        inputActions.Enable();
        inputActions.Gameplay.PlayComposition.performed += OnPlayComposition;
        inputActions.Gameplay.OnStop.performed += OnStop; 
        inputActions.Gameplay.OnReset.performed += OnReset; 
        
    }

    void OnDisable()
    {
        // Unsubscribe and disable InputActions
        inputActions.Gameplay.PlayComposition.performed -= OnPlayComposition;
        inputActions.Gameplay.OnStop.performed -= OnStop;
        inputActions.Gameplay.OnReset.performed -= OnReset;
        inputActions.Disable();
    }

    private void OnPlayComposition(InputAction.CallbackContext context)
    {
        // Trigger PlayComposition when the input is performed
        PlayComposition();
    }

    void OnStop(InputAction.CallbackContext context)
    {
        // Trigger PlayComposition when the input is performed
        StopSequence();
    }

    void OnReset(InputAction.CallbackContext context)
    {
        gridManager.ResetAll();
    }

    public void StopSequence()

    {
        if (onboarding != null && onboarding.onboardingMode && onboarding.isMelodyComplete && onboarding.isPlayStepDone && !onboarding.isPauseStepDone)
        {
            onboarding.isPauseStepDone = true;
            Debug.Log("Onboarding: Marking Pause step as done!");
        }
        // Trigger PlayComposition when the input is performed
        stop = true;
        playing = false;
    }



    public void PlayComposition()
    {
        
        if (onboarding != null && onboarding.onboardingMode && onboarding.isMelodyComplete && !onboarding.isPlayStepDone)
        {
            onboarding.isPlayStepDone = true;
            Debug.Log("Onboarding: Marking Play step as done!");
        }
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        if (playing == true)
        {
            yield break;
        }
        
        int rows = gridManager.rows;
        int columns = gridManager.columns;
        playing = true;
        stop = false;
        
        // Outer loop for infinite play
        while (infinite)
        {
            for (int c = 0; c < columns; c++)
            {
                // Check if the stop condition is met
                if (stop)
                {
                    gridManager.ResetAllColumns(); 
                    yield break; // Exit the coroutine
                }

                gridManager.MoveColumn(c);

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
                                AudioSource newSource = Instantiate(audioSourcePrefab, transform);
                                newSource.clip = chosenClip;
                                newSource.volume = volume;
                                newSource.Play();

                                // Instead of destroying immediately,
                                // fade out and then destroy after 'noteDuration'.
                                float fadeDuration = 0.1f; 
                                StartCoroutine(FadeOutAndDestroy(newSource, noteDuration, fadeDuration));
                            }
                            else
                            {
                                Debug.LogWarning($"No clip set for {instrument} pitchIndex={pitchIndex}");
                            }
                        }
                    }
                }

                // Wait for the length of the note before moving on
                yield return new WaitForSeconds(noteDuration);
            }
        }

        // Cleanup if somehow we exit the while loop
        gridManager.ResetAllColumns(); 
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

                // Start fade-out coroutine instead of directly destroying the AudioSource
                float fadeDuration = 0.1f; // Adjust fade duration as needed
                StartCoroutine(FadeOutAndDestroy(newSource, noteDuration, fadeDuration));
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

            case InstrumentType.Lead:
                if (pitchIndex >= 0 && pitchIndex < leadClips.Length)
                    return leadClips[pitchIndex];
                break;
            
            case InstrumentType.Bass:
                if (pitchIndex >= 0 && pitchIndex < bassClips.Length)
                    return bassClips[pitchIndex];
                break;

            case InstrumentType.Drums:
                if (pitchIndex >= 0 && pitchIndex < drumsClips.Length)
                    return drumsClips[pitchIndex];
                break;

            // TODO: Add more instruments here
        }

        return null;
    }

    private IEnumerator FadeOutAndDestroy(AudioSource source, float noteDuration, float fadeDuration)
    {
        // Wait until it's time to start fading out
        // (For example, start the fade a little before the noteDuration finishes.)
        float waitTime = Mathf.Max(noteDuration - fadeDuration, 0f);
        yield return new WaitForSeconds(waitTime);

        float startVolume = source.volume;
        float timeElapsed = 0f;

        // Fade out over 'fadeDuration' seconds
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(timeElapsed / fadeDuration);
            source.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }

        // Done fading—destroy the GameObject
        Destroy(source.gameObject);
    }
}



