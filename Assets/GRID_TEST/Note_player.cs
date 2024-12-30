using UnityEngine;
using System.Collections;

public class NotePlayer : MonoBehaviour
{
    public NoteGridManager gridManager;
    public AudioSource audioSourcePrefab;  // A simple AudioSource prefab
    public AudioClip[] pitchClips;         // 7 pitch clips
    public float noteDuration = 0.5f;      // Each column plays for 0.5s

    public void PlayComposition()
    {
        // Start a coroutine that goes column by column
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        int rows = gridManager.rows;
        int columns = gridManager.columns;

        for (int c = 0; c < columns; c++)
        {
            // For each column, check all rows
            for (int r = 0; r < rows; r++)
            {
                if (gridManager.cells[r, c].HasNote())
                {
                    // Create an AudioSource instance for this note
                    AudioSource newSource = Instantiate(audioSourcePrefab, transform);
                    newSource.clip = pitchClips[r]; // row r => pitch r
                    newSource.Play();
                    
                    // Optionally destroy it after it finishes playing
                    Destroy(newSource.gameObject, noteDuration + 0.2f);
                }
            }
            // Wait for the note duration before moving to the next column
            yield return new WaitForSeconds(noteDuration);
        }
    }
}
