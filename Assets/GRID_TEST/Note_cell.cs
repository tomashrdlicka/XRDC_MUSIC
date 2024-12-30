using UnityEngine;

public class NoteCell : MonoBehaviour
{
    public int gridRow;
    public int gridColumn;
    
    private bool hasNote = false;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (hasNote)
            meshRenderer.material.color = Color.green;
        else
            meshRenderer.material.color = Color.white; // or some default
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the hand
        if (other.gameObject.CompareTag("Hand")) 
        {
            // Toggle note
            hasNote = !hasNote;
            UpdateColor();
        }
    }

    public bool HasNote()
    {
        return hasNote;
    }

    public void RemoveNote()
    {
        hasNote = false;
        UpdateColor();
    }

    public void ToggleNote()
{
    hasNote = !hasNote;
    UpdateColor();
}
}

