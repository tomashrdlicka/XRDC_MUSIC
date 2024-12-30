using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ThereminController : MonoBehaviour
{
    [Tooltip("Left Elements")]
    [Header("Left Hand Settings")]
    public Transform leftHandObject;
    public Material leftTrailMaterial;
    public Material leftSpiralTrailMaterial;
    public float leftSpiralRadius = 0.1f;
    public float leftSpiralSpeed = 5.0f;

    [Tooltip("Right Elements")]
    [Header("Right Hand Settings")]
    public Transform rightHandObject;
    public Material rightTrailMaterial;
    public Material rightSpiralTrailMaterial;
    public float rightSpiralRadius = 0.1f;
    public float rightSpiralSpeed = 5.0f;

    [Header("Scene Settings")]
    [Tooltip("Interaction zone should be centered to user")]
    public Vector3 interactionZoneCenter = Vector3.zero;
    [Tooltip("Size of interaction zone")]
    public Vector3 interactionZoneSize = new Vector3(2, 2, 2);
    [Tooltip("Legth of hand trails (Line renderer)")]
    public float trailTime = 0.5f;
    [Tooltip("Audio min frequency")]
    public float minFrequency = 220.0f;
    [Tooltip("Audio max frequency")]
    public float maxFrequency = 880.0f;
    [Tooltip("Audio min volume")]
    public float minVolume = 0.0f;
    [Tooltip("Audio max volume - BE CAREFUL!!!")]
    public float maxVolume = 0.8f;

    private AudioSource leftAudioSource;
    private AudioSource rightAudioSource;
    private float leftFrequency = 440.0f;
    private float leftVolume = 0.0f;
    private float rightFrequency = 440.0f;
    private float rightVolume = 0.0f;

    private double leftPhase = 0.0;
    private double rightPhase = 0.0;
    private TrailRenderer leftTrailRenderer, leftSpiralTrailRenderer;
    private TrailRenderer rightTrailRenderer, rightSpiralTrailRenderer;
    private float leftSpiralAngle = 0.0f;
    private float rightSpiralAngle = 0.0f;

    private float sampleRate;

    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;

        // Left hand setup
        if (leftHandObject != null)
        {
            leftAudioSource = CreateAudioSource("LeftAudioSource", LeftHandPCMReader);
            leftTrailRenderer = AddTrailRenderer(leftHandObject, leftTrailMaterial);
            leftSpiralTrailRenderer = AddSpiralTrailRenderer(leftHandObject, leftSpiralTrailMaterial);
        }

        // Right hand setup
        if (rightHandObject != null)
        {
            rightAudioSource = CreateAudioSource("RightAudioSource", RightHandPCMReader);
            rightTrailRenderer = AddTrailRenderer(rightHandObject, rightTrailMaterial);
            rightSpiralTrailRenderer = AddSpiralTrailRenderer(rightHandObject, rightSpiralTrailMaterial);
        }
    }

    void Update()
    {
        // Update left hand
        if (leftHandObject != null)
        {
            UpdateTheremin(leftHandObject, ref leftFrequency, ref leftVolume, leftTrailRenderer, leftSpiralTrailRenderer, ref leftSpiralAngle, leftSpiralRadius, leftSpiralSpeed);
        }

        // Update right hand
        if (rightHandObject != null)
        {
            UpdateTheremin(rightHandObject, ref rightFrequency, ref rightVolume, rightTrailRenderer, rightSpiralTrailRenderer, ref rightSpiralAngle, rightSpiralRadius, rightSpiralSpeed);
        }
    }

    private AudioSource CreateAudioSource(string name, AudioClip.PCMReaderCallback pcmReaderCallback)
    {
        GameObject audioSourceObject = new GameObject(name);
        audioSourceObject.transform.parent = transform;
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.clip = AudioClip.Create(name + "Clip", 44100, 1, 44100, true, pcmReaderCallback);
        audioSource.Play();
        return audioSource;
    }

    private TrailRenderer AddTrailRenderer(Transform handObject, Material material)
    {
        var trail = handObject.gameObject.AddComponent<TrailRenderer>();
        trail.material = material;
        trail.time = 0;
        trail.startWidth = 0.05f;
        trail.endWidth = 0.01f;
        return trail;
    }

    private TrailRenderer AddSpiralTrailRenderer(Transform handObject, Material material)
    {
        GameObject spiralTrailObject = new GameObject("SpiralTrail");
        spiralTrailObject.transform.parent = handObject;
        spiralTrailObject.transform.localPosition = Vector3.zero;

        var trail = spiralTrailObject.AddComponent<TrailRenderer>();
        trail.material = material;
        trail.time = 0;
        trail.startWidth = 0.03f;
        trail.endWidth = 0.01f;
        return trail;
    }

    private void UpdateTheremin(
        Transform handObject,
        ref float frequency,
        ref float volume,
        TrailRenderer trailRenderer,
        TrailRenderer spiralTrailRenderer,
        ref float spiralAngle,
        float spiralRadius,
        float spiralSpeed)
    {
        Vector3 objectPosition = handObject.position;

        bool isInInteractionZone = IsInInteractionZone(objectPosition);

        if (isInInteractionZone)
        {
            float pitch = Mathf.InverseLerp(interactionZoneCenter.y - interactionZoneSize.y / 2, interactionZoneCenter.y + interactionZoneSize.y / 2, objectPosition.y);
            frequency = Mathf.Lerp(minFrequency, maxFrequency, pitch);

            float volumeControl = Mathf.InverseLerp(interactionZoneCenter.x - interactionZoneSize.x / 2, interactionZoneCenter.x + interactionZoneSize.x / 2, objectPosition.x);
            volume = Mathf.Lerp(minVolume, maxVolume, volumeControl);

            if (trailRenderer != null) trailRenderer.time = trailTime;
            if (spiralTrailRenderer != null)
            {
                spiralTrailRenderer.time = trailTime;
                spiralAngle += spiralSpeed * Time.deltaTime;
                float xOffset = Mathf.Cos(spiralAngle) * spiralRadius;
                float yOffset = Mathf.Sin(spiralAngle) * spiralRadius;
                spiralTrailRenderer.transform.localPosition = new Vector3(xOffset, yOffset, 0);
            }
        }
        else
        {
            volume = 0.0f;
            if (trailRenderer != null) trailRenderer.time = 0;
            if (spiralTrailRenderer != null) spiralTrailRenderer.time = 0;
        }
    }

    private void LeftHandPCMReader(float[] data)
    {
        GenerateSineWave(data, ref leftPhase, leftFrequency, leftVolume);
    }

    private void RightHandPCMReader(float[] data)
    {
        GenerateSineWave(data, ref rightPhase, rightFrequency, rightVolume);
    }

    private void GenerateSineWave(float[] data, ref double phase, float frequency, float volume)
    {
        double increment = frequency * 2.0 * Mathf.PI / sampleRate;
        for (int i = 0; i < data.Length; i++)
        {
            phase += increment;
            if (phase > 2.0 * Mathf.PI) phase -= 2.0 * Mathf.PI;
            data[i] = Mathf.Sin((float)phase) * volume;
        }
    }

    private bool IsInInteractionZone(Vector3 position)
    {
        return (position.x >= interactionZoneCenter.x - interactionZoneSize.x / 2 &&
                position.x <= interactionZoneCenter.x + interactionZoneSize.x / 2 &&
                position.y >= interactionZoneCenter.y - interactionZoneSize.y / 2 &&
                position.y <= interactionZoneCenter.y + interactionZoneSize.y / 2 &&
                position.z >= interactionZoneCenter.z - interactionZoneSize.z / 2 &&
                position.z <= interactionZoneCenter.z + interactionZoneSize.z / 2);
    }
}