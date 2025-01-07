using UnityEngine;
using System.Collections;

public class NotePlayerOld : MonoBehaviour
{
    public RingGridManager gridManager;       // Reference to the manager
    public AudioSource audioSourcePrefab; // A simple AudioSource prefab
    public AudioClip[] pitchClips;        // 7 pitch samples
    public float noteDuration = 0.5f;     // Duration per column

    void Update()
    {
        //if the 'G' key is pressed, start playing the composition
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayComposition();
        }
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
            // Check each row in this column
            for (int r = 0; r < rows; r++)
            {
                //if (gridManager.cells[r, c].HasNote())
                //{
                    //play notes in given time step
                    AudioSource newSource = Instantiate(audioSourcePrefab, transform);
                    newSource.clip = pitchClips[r];
                    //newSource.volume = gridManager.cells[r, c].GetVolume();
                    newSource.Play();

                    //destroy after playing
                    Destroy(newSource.gameObject, noteDuration + 0.2f);
                //}
            }
            //wait for the note duration before moving on to the next column
            yield return new WaitForSeconds(noteDuration);
        }
    }
}



