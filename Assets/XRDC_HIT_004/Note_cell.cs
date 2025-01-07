using UnityEngine;

public class NoteCellOld : MonoBehaviour
{
    public int gridRow;
    public int gridColumn;

    public bool hasNote = false;
    public float volume = 0f; //range: 0.0 - 1.0 (for example)
    private MeshRenderer meshRenderer;

    //min/mas colors
    [SerializeField] private Color baseGreen = new Color(0f, 0.3f, 0f, 1f); 
    [SerializeField] private Color maxGreen = Color.green;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateColor();
    }

    //toggle note
    public void ToggleNote()
    {
        hasNote = !hasNote;
        if (hasNote)
        {
            //set default volume to 0.5
            volume = 0.5f;
        }
        else
        {
            //if no note, set volume to 0
            volume = 0f;
        }
        UpdateColor();
    }

    //update color reflecting note volume
    private void UpdateColor()
    {
        if (hasNote)
        {
            //interpolate
            meshRenderer.material.color = new Color(0f, 0.3f + (0.7f * volume), 0f, 1f);
        }
        else
        {
            //if no note–> white color
            meshRenderer.material.color = Color.white;
        }
    }

    //adjust volume be a certain amount
    public void AdjustVolume(float delta)
    {
        if (!hasNote) return; //only adjust volume if there's a note

        volume += delta;
        volume = Mathf.Clamp01(volume); //keep volume in [0, 1]
        UpdateColor();
    }

 

    //get whether we have a note
    public bool HasNote() => hasNote;

    //get the volume for playback
    public float GetVolume() => volume;
}



