using UnityEngine;

public class PulsatingSphereController : MonoBehaviour
{
    [SerializeField] private Material pulsatingMaterial;
    [SerializeField] private float bpm = 120f; // Beats per minute
    [SerializeField] private float scaleIntensity = 1f; // Maximum scale
    [SerializeField] private float fadeMultiplier = 1f; // Fade intensity

    private void Update()
    {
        if (pulsatingMaterial != null)
        {
            pulsatingMaterial.SetFloat("_BPM", bpm);
            pulsatingMaterial.SetFloat("_ScaleIntensity", scaleIntensity);
            pulsatingMaterial.SetFloat("_FadeMultiplier", fadeMultiplier);
        }
    }
}