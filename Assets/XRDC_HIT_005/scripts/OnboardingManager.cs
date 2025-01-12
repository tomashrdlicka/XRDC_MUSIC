using UnityEngine;
using System.Collections;

public class OnboardingManager : MonoBehaviour
{
    [Header("References")]
    public RingGridManager ringGridManager;  // Assign in Inspector
    public bool onboardingMode = false;

    // Flags for the onboarding flow
    public bool isMelodyComplete = false; 
    public bool isPlayStepDone   = false; 
    public bool isPauseStepDone  = false;  

    [Header("Audio Settings")]
    public AudioSource audioSourceNarrative;            // Assign via Inspector or dynamically create one
    public AudioClip[] narrativeClips;         // Array of narrative voiceover clips
    private int currentAudioIndex = 0;         // Track the current audio clip

    public InstrumentType requiredInstrument = InstrumentType.Piano;
    GameObject instrumentMenuObj;

    // Piano melody steps: E E F G | G F E D | C C D E | D C C C
    [System.Serializable]
    public struct NoteStep
    {
        public int row;
        public int column;
    }

    // --- FIRST MELODY (e.g., for Piano) ---
    public NoteStep[] pianoSteps = new NoteStep[]
    {
        // E E F G
        new NoteStep{ row = 2, column = 0 }, 
        new NoteStep{ row = 2, column = 1 }, 
        new NoteStep{ row = 3, column = 2 }, 
        new NoteStep{ row = 4, column = 3 },

        // G F E D
        new NoteStep{ row = 4, column = 4 }, 
        new NoteStep{ row = 3, column = 5 }, 
        new NoteStep{ row = 2, column = 6 }, 
        new NoteStep{ row = 1, column = 7 },

        // C C D E
        new NoteStep{ row = 0, column = 8 }, 
        new NoteStep{ row = 0, column = 9 }, 
        new NoteStep{ row = 1, column = 10},
        new NoteStep{ row = 2, column = 11},

        // D C C C
        new NoteStep{ row = 1, column = 12},
        new NoteStep{ row = 0, column = 13},
        new NoteStep{ row = 0, column = 14},
        new NoteStep{ row = 0, column = 15},
    };

    // --- SECOND MELODY (e.g., for Bass) ---
    // C C G G | F F C C | F F G G | C C C C
    // Assuming row=0 -> C, row=3 -> F, row=4 -> G
    public NoteStep[] bassSteps = new NoteStep[]
    {
        // C C G G
        new NoteStep{ row = 0, column = 0 }, 
        new NoteStep{ row = 0, column = 1 }, 
        new NoteStep{ row = 4, column = 2 }, 
        new NoteStep{ row = 4, column = 3 },

        // F F C C
        new NoteStep{ row = 3, column = 4 },
        new NoteStep{ row = 3, column = 5 },
        new NoteStep{ row = 0, column = 6 },
        new NoteStep{ row = 0, column = 7 },

        // F F G G
        new NoteStep{ row = 3, column = 8 },
        new NoteStep{ row = 3, column = 9 },
        new NoteStep{ row = 4, column = 10 },
        new NoteStep{ row = 4, column = 11 },

        // C C C C
        new NoteStep{ row = 0, column = 12 },
        new NoteStep{ row = 0, column = 13 },
        new NoteStep{ row = 0, column = 14 },
        new NoteStep{ row = 0, column = 15 },
    };

    // Internal tracking
    private int currentStepIndex = -1;
    private bool stepInProgress  = false;

    // Are we on the first melody (pianoSteps) or second (bassSteps)?
    private bool doingFirstMelody = true; 

    private void Start()
    {   
        audioSourceNarrative = gameObject.AddComponent<AudioSource>();
        instrumentMenuObj = GameObject.Find("InstrumentMenu");
        instrumentMenuObj.SetActive(false);

        onboardingMode = true;
        StartCoroutine(WaitForGridInitialization());

    }

    private IEnumerator WaitForGridInitialization()
    {
        // Wait until RingGridManager has finished setting up
        while (ringGridManager.cells == null || ringGridManager.cells.Length == 0)
        {
            Debug.Log("Waiting for RingGridManager to initialize...");
            yield return null;
        }

        Debug.Log("RingGridManager initialized. Starting onboarding...");
        if (onboardingMode)
        {
            DisableAllCells();
            yield return StartCoroutine(WaitForSeconds(3f));
            PlayNarrativeClip(0); 
            yield return WaitForAudioClipToFinish();
            PlayNarrativeClip(1); 
            yield return WaitForAudioClipToFinish();
            PlayNarrativeClip(24); 
            yield return WaitForAudioClipToFinish();
            Debug.Log("wait over");
            StartCoroutine(StartPianoMelodyPlacing());
        }
    }

    //--------------------------------------------------------------------------
    //  FIRST MELODY FLOW (Piano)
    //--------------------------------------------------------------------------

    /// <summary>
    /// Called to begin placing the first melody (e.g., Piano).
    /// </summary>
    public IEnumerator StartPianoMelodyPlacing()
    {
        
        Debug.Log("started");
        yield return StartCoroutine(WaitForSeconds(2f));
        PlayNarrativeClip(2); 
        yield return WaitForAudioClipToFinish();
        PlayNarrativeClip(3); 
        yield return WaitForAudioClipToFinish();
        PlayNarrativeClip(4); 
        yield return WaitForAudioClipToFinish();

        doingFirstMelody = true;
        isMelodyComplete = false;
        currentStepIndex = -1;
        yield return StartCoroutine(WaitForSeconds(1f));
        PlayNarrativeClip(5); 
        yield return WaitForAudioClipToFinish();
        GoToNextStep();
    }

    /// <summary>
    /// Steps through the currently active melody (pianoSteps if doingFirstMelody, else bassSteps).
    /// </summary>
    private bool isFirstGoToNextStepCall = true;

    public void GoToNextStep()
{
    // If this is the first call, skip the random value logic

    // Increment the step index
    currentStepIndex++;

    // Determine the melody array
    NoteStep[] currentMelody = doingFirstMelody ? pianoSteps : bassSteps;

    // Check if we've placed all notes in the current melody
    if (currentStepIndex >= currentMelody.Length)
    {
        isFirstGoToNextStepCall = true;
        EndMelodyPlacing();
        return;
    }

    if (isFirstGoToNextStepCall)
    {
        isFirstGoToNextStepCall = false; // Set the flag to false for subsequent calls
    }
    else
    {
        // Generate a random value (0 to 4)
        int randomValue = Random.Range(0, 5);
        Debug.Log($"Random value: {randomValue}");

        // Play narrative clip based on the random value
        if (randomValue == 0)
        {
            PlayNarrativeClip(6);
        }
        else if (randomValue == 1)
        {
            PlayNarrativeClip(7);
        }
        else if (randomValue == 2)
        {
            PlayNarrativeClip(8);
        }
        else if (randomValue == 3)
        {
            PlayNarrativeClip(9);
        }
        else if (randomValue == 4)
        {
            PlayNarrativeClip(10);
        }
    }

    // Process the current step
    NoteStep step = currentMelody[currentStepIndex];

    // Lock all cells and highlight the target cell
    DisableAllCells();
    DisplayColumn(step.column);
    HighlightAndEnableCell(step.row, step.column);
}

    /// <summary>
    /// Called after finishing all notes in the current melody.
    /// </summary>
    private void EndMelodyPlacing()
    {
        isMelodyComplete = true;
        // Lock cells
        DisableAllCells();

        // Now instruct user to do Play/Pause
        StartCoroutine(TeachPlayPauseRoutine());
    }

    //--------------------------------------------------------------------------
    //  PLAY/PAUSE + POST-MELODY FLOW
    //--------------------------------------------------------------------------

    /// <summary>
    /// The routine that waits for user to press Play, then Pause.
    /// After that, we decide whether to move on to instrument switching or finish altogether.
    /// </summary>
    private IEnumerator TeachPlayPauseRoutine()
    {
        // Reset these if we want the user to do them fresh each melody
        isPlayStepDone  = false;
        isPauseStepDone = false;

        if (ringGridManager.GetGlobalInstrument() == InstrumentType.Piano)
        {
            yield return StartCoroutine(WaitForSeconds(1f));
            PlayNarrativeClip(11); 
            yield return WaitForAudioClipToFinish();
            PlayNarrativeClip(12); 
            yield return WaitForAudioClipToFinish();
        }
        if (ringGridManager.GetGlobalInstrument() == InstrumentType.Bass)
        {
            yield return StartCoroutine(WaitForSeconds(1f));
            PlayNarrativeClip(12); 
            yield return WaitForAudioClipToFinish();
        }
        
        
        Debug.Log("Please press Play to hear the sequence...");

        // Wait until user pressed Play
        while (!isPlayStepDone)
        {
            yield return null;
        }
        if (ringGridManager.GetGlobalInstrument() == InstrumentType.Piano)
        {
            yield return StartCoroutine(WaitForSeconds(4f));
            PlayNarrativeClip(13); 
            yield return WaitForAudioClipToFinish();
            yield return StartCoroutine(WaitForSeconds(4f));
            PlayNarrativeClip(14); 
            yield return WaitForAudioClipToFinish();
        }
        if (ringGridManager.GetGlobalInstrument() == InstrumentType.Bass)
        {
            yield return StartCoroutine(WaitForSeconds(4f));
            yield return StartCoroutine(WaitForSeconds(4f));
            PlayNarrativeClip(22); 
            yield return WaitForAudioClipToFinish();
        }
        Debug.Log("Sequence playing! Now press Pause.");
        // Wait until user pressed Pause
        while (!isPauseStepDone)
        {
            yield return null;
        }

        // At this point, the user did play & pause for this melody
        Debug.Log("Play/Pause steps complete for this melody.");
        if (ringGridManager.GetGlobalInstrument() == InstrumentType.Piano)
        {
            yield return StartCoroutine(WaitForSeconds(1f));
            PlayNarrativeClip(15); 
            yield return WaitForAudioClipToFinish();
            PlayNarrativeClip(16); 
            yield return WaitForAudioClipToFinish();
            yield return StartCoroutine(WaitForSeconds(2f));
            PlayNarrativeClip(17); 
            yield return WaitForAudioClipToFinish();
        }


        ringGridManager.SetSwitchInstruments(true);
        instrumentMenuObj.SetActive(true);

        if (doingFirstMelody)
        {
            // If we just finished the first melody, let's move to the instrument-switching step
            doingFirstMelody = false;
            StartCoroutine(WaitForUserToSwitchToBass());
        }
        else
        {
            // We finished the second melody (Bass). Now we can finalize.
            Debug.Log("Done with second melody. Resetting grid & ending onboarding...");
            yield return StartCoroutine(WaitForSeconds(1f));
            PlayNarrativeClip(23); 
            yield return WaitForAudioClipToFinish();
            yield return StartCoroutine(WaitForSeconds(2f));
            PlayNarrativeClip(25); 
            yield return WaitForAudioClipToFinish();
            yield return StartCoroutine(WaitForSeconds(1f));
            PlayNarrativeClip(26); 
            yield return WaitForAudioClipToFinish(); 
            yield return StartCoroutine(WaitForSeconds(1f));
            PlayNarrativeClip(27); 
            yield return WaitForAudioClipToFinish(); 
            ringGridManager.ResetAll();  // Or ringGridManager.ResetAll() if you have that
            yield return StartCoroutine(WaitForSeconds(1f));
            PlayNarrativeClip(1); 
            yield return WaitForAudioClipToFinish(); 
            EndOnboarding();
        }
    }

    //--------------------------------------------------------------------------
    //  SWITCH TO BASS INSTRUMENT
    //--------------------------------------------------------------------------

    /// <summary>
    /// After finishing the first melody, wait for user to switch to Bass instrument.
    /// Then start placing the second melody.
    /// </summary>
    private IEnumerator WaitForUserToSwitchToBass()
    {
        Debug.Log("Please switch to Bass instrument (using Next or Previous).");

        // Wait until ringGridManager's globalInstrument is Bass
        while (ringGridManager.GetGlobalInstrument() != InstrumentType.Bass)
        {
            yield return null;
        }
         PlayNarrativeClip(18); 
        yield return WaitForAudioClipToFinish();
        PlayNarrativeClip(20); 
        yield return WaitForAudioClipToFinish();
        PlayNarrativeClip(21); 
        yield return WaitForAudioClipToFinish();
        Debug.Log("Bass instrument selected! Start placing the second melody.");
        StartBassMelodyPlacing();
    }

    /// <summary>
    /// Begins the second melody steps (on Bass).
    /// </summary>
    public void StartBassMelodyPlacing()
    {
        requiredInstrument = InstrumentType.Bass;
        isMelodyComplete = false;
        currentStepIndex = -1;
        GoToNextStep(); // This time it will use bassSteps
    }

    //--------------------------------------------------------------------------
    //  UTILITY METHODS
    //--------------------------------------------------------------------------

    private void DisplayColumn(int column)
    {
        Debug.Log($"Displaying column {column}...");

        for (int r = 0; r < ringGridManager.rows; r++)
        {
            if (!IsOutOfRange(r, column))
            {
                NoteCell cell = ringGridManager.cells[r, column];
                if (cell != null)
                {
                    // Make them visible, but not interactable
                    cell.meshRenderer.enabled = true;
                    cell.boxCollider.enabled  = false;
                    cell.SetCornerVisibility(true);

                    if (cell.controlRenderer != null)
                        cell.controlRenderer.enabled = true;
                }
            }
        }
    }

    private void HighlightAndEnableCell(int row, int column)
    {
        if (IsOutOfRange(row, column))
        {
            Debug.LogError($"Onboarding: Invalid cell [{row}, {column}]");
            return;
        }

        NoteCell cell = ringGridManager.cells[row, column];
        if (cell == null)
        {
            Debug.LogError($"Onboarding: Cell script is null at [{row}, {column}]!");
            return;
        }

        // Highlight & enable the cell
        cell.meshRenderer.enabled = true;
        cell.boxCollider.enabled  = true;
        cell.SetCornerVisibility(true);
        cell.meshRenderer.material.color = Color.yellow;

        stepInProgress = true;

        Debug.Log($"Highlighted cell [{row}, {column}]");
    }

    public void OnCellPressed(NoteCell cell)
    {
        if (!onboardingMode) return;
        if (!stepInProgress) return;

        // Determine if we're placing the first or second melody
        NoteStep[] currentMelody = doingFirstMelody ? pianoSteps : bassSteps;

        // Check if user pressed the correct cell
        NoteStep currentStep = currentMelody[currentStepIndex];
        if (cell.gridRow == currentStep.row && cell.gridColumn == currentStep.column)
        {
            stepInProgress = false;
            // Reset color
            cell.meshRenderer.material.color = new Color(1f, 1f, 1f, 0.03f);

            GoToNextStep();
        }
        else
        {
            Debug.Log("Wrong cell pressed! Please press the highlighted cell.");
        }
    }

    private void DisableAllCells()
    {
        Debug.Log("Disabling all cells...");
        for (int r = 0; r < ringGridManager.rows; r++)
        {
            for (int c = 0; c < ringGridManager.columns; c++)
            {
                if (!IsOutOfRange(r, c) && ringGridManager.cells[r, c] != null)
                {
                    ringGridManager.cells[r, c].boxCollider.enabled  = false;
                    // Optionally hide them:
                    // ringGridManager.cells[r, c].meshRenderer.enabled = false;
                }
            }
        }
    }

    private void EnableAllCells()
    {
        for (int r = 0; r < ringGridManager.rows; r++)
        {
            for (int c = 0; c < ringGridManager.columns; c++)
            {
                if (!IsOutOfRange(r, c) && ringGridManager.cells[r, c] != null)
                {
                    ringGridManager.cells[r, c].boxCollider.enabled  = true;
                    ringGridManager.cells[r, c].meshRenderer.enabled = true;
                    ringGridManager.cells[r, c].SetCornerVisibility(true);
                }
            }
        }
    }

    private bool IsOutOfRange(int r, int c)
    {
        return r < 0 || r >= ringGridManager.rows || c < 0 || c >= ringGridManager.columns;
    }

    private void EndOnboarding()
    {
        Debug.Log("Onboarding complete! All steps finished.");
        onboardingMode  = false;
        stepInProgress  = false;
        isMelodyComplete = false;
        EnableAllCells(); // Re-enable everything if desired
    }

    //NARRATIVE FUNCTIONS
    public void PlayNarrativeClip(int index)
    {
        if (index < 0 || index >= narrativeClips.Length)
        {
            Debug.LogWarning($"Invalid narrative clip index: {index}");
            return;
        }

        currentAudioIndex = index;
        audioSourceNarrative.clip = narrativeClips[index];
        audioSourceNarrative.Play();
    }

    /// <summary>
    /// Wait for the current audio clip to finish before triggering the next action.
    /// </summary>
    private IEnumerator WaitForAudioClipToFinish()
    {
        while (audioSourceNarrative.isPlaying)
        {
            yield return null; // Wait until the clip is done playing
        }
    }
    private IEnumerator WaitForSeconds(float waitTime)
    {
        Debug.Log($"Waiting for {waitTime} seconds...");
        
        // Wait for the specified amount of time
        yield return new WaitForSeconds(waitTime);
        
        // Code to execute after waiting
        Debug.Log("Wait complete!");
    }


}

