using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent

public class HitPose : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public string handTag = "Hand";

    public Material newMaterial;
    private Material originalMaterial;
    private Renderer objectRenderer;

    public GameObject particlesOn;

    public UnityEvent onTouchEnter; // Event triggered on touch
    public UnityEvent onTouchExit; // Event triggered on touch end

    private bool isTouched = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag)) // Triggered by tagged "Hand"
        {
            isTouched = !isTouched;

            if (isTouched)
            {
                // Play
                if (audioSource != null && audioClip != null)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(audioClip); // Play the clip
                }

                if (objectRenderer != null && newMaterial != null)
                {
                    objectRenderer.material = newMaterial;
                }

                if (particlesOn != null)
                {
                    particlesOn.SetActive(true);
                }

                onTouchEnter?.Invoke(); // Trigger the onTouchEnter event
            }
            else
            {
                // Stop
                if (audioSource != null)
                {
                    audioSource.Stop();
                }

                if (objectRenderer != null)
                {
                    objectRenderer.material = originalMaterial;
                }

                if (particlesOn != null)
                {
                    particlesOn.SetActive(false);
                }

                onTouchExit?.Invoke(); // Trigger the onTouchExit event
            }
        }
    }
}