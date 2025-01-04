using UnityEngine;
using System.Collections;

public class NotePlayer : MonoBehaviour
{
    public RingGridManager gridManager;
    public AudioSource audioSourcePrefab;
    public float noteDuration = 0.5f;

    // ───────────────────────────────────────────────────────────────────
    // Separate arrays for each instrument
    // ───────────────────────────────────────────────────────────────────
    [Header("Instrument Clips")]
    public AudioClip[] pianoClips;
    public AudioClip[] guitarClips;
    // etc.

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayComposition();
        }
    }

    public void PlayComposition()
    {
        StartCoroutine(PlaySequence());
    }

    public void PlaySingleNote(NoteCell cell)
    {
        // 1) Retrieve instrument, pitch, volume from the cell
        InstrumentType instrument = cell.GetCellInstrument();
        int pitchIndex = cell.GetPitchIndex();
        float volume = cell.GetVolume();

        // 2) Get the correct clip
        AudioClip chosenClip = GetClipByInstrument(instrument, pitchIndex);

        if (chosenClip != null)
        {
            // 3) Instantiate an AudioSource to play the note
            AudioSource newSource = Instantiate(audioSourcePrefab, transform);
            newSource.clip = chosenClip;
            newSource.volume = volume;
            newSource.Play();

            // 4) Destroy it after it finishes
            Destroy(newSource.gameObject, noteDuration + 0.1f);
        }
        else
        {
            Debug.LogWarning($"Could not find clip for instrument {instrument} with pitchIndex {pitchIndex}");
        }
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
                if (cell.HasNote())
                {
                    // 1) Get the instrument & pitch index
                    InstrumentType instrument = cell.GetCellInstrument();
                    Debug.Log(instrument);
                    int pitchIndex = cell.GetPitchIndex();
                    float volume = cell.GetVolume();

                    // 2) Retrieve the correct clip array
                    AudioClip chosenClip = GetClipByInstrument(instrument, pitchIndex);

                    if (chosenClip != null)
                    {
                        // 3) Instantiate and play
                        AudioSource newSource = Instantiate(audioSourcePrefab, transform);
                        newSource.clip = chosenClip;
                        newSource.volume = volume;
                        newSource.Play();

                        // 4) Clean up
                        Destroy(newSource.gameObject, noteDuration + 0.1f);
                    }
                    else
                    {
                        Debug.LogWarning($"{instrument} or pitchIndex {pitchIndex} not set correctly.");
                    }
                }
            }
            // Wait for the note duration before moving on
            yield return new WaitForSeconds(noteDuration);
        }
    }

    // ───────────────────────────────────────────────────────────────────
    // Helper Method: Return the correct clip based on instrument & pitch
    // ───────────────────────────────────────────────────────────────────
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
            // Add more instruments here as needed
        }
        // If out of range or not assigned
        return null;
    }
}




