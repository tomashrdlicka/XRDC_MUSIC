using UnityEngine;

public class BPMGradientController : MonoBehaviour
{
    public Material targetMaterial;
    public float bpm = 120f;
    public float amplitude = 1.0f;
    public Texture2D colorGradient;

    private float secondsPerBeat;
    private float beatTimer;

    void Start()
    {
        secondsPerBeat = 60f / bpm;
        targetMaterial.SetFloat("_Amplitude", amplitude);
        if (colorGradient != null)
        {
            targetMaterial.SetTexture("_ColorGradient", colorGradient);
        }
    }

    void Update()
    {
        beatTimer += Time.deltaTime;
        if (beatTimer >= secondsPerBeat)
        {
            beatTimer -= secondsPerBeat;
        }

        float beatProgress = beatTimer / secondsPerBeat; // Progress 0 to 1 over the beat
        float timeMultiplier = Mathf.Sin(beatProgress * Mathf.PI * 2); // Smooth looping
        targetMaterial.SetFloat("_TimeMultiplier", timeMultiplier);
    }
}