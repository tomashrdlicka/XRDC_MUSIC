/*
 ___    ___ ________  ________  ________          ________       ___    ___ ________   _________  ___  ___     
|\  \  /  /|\   __  \|\   ___ \|\   ____\        |\   ____\     |\  \  /  /|\   ___  \|\___   ___\\  \|\  \    
\ \  \/  / | \  \|\  \ \  \_|\ \ \  \___|        \ \  \___|_    \ \  \/  / | \  \\ \  \|___ \  \_\ \  \\\  \   
 \ \    / / \ \   _  _\ \  \ \\ \ \  \            \ \_____  \    \ \    / / \ \  \\ \  \   \ \  \ \ \   __  \  
  /     \/   \ \  \\  \\ \  \_\\ \ \  \____        \|____|\  \    \/  /  /   \ \  \\ \  \   \ \  \ \ \  \ \  \ 
 /  /\   \    \ \__\\ _\\ \_______\ \_______\        ____\_\  \ __/  / /      \ \__\\ \__\   \ \__\ \ \__\ \__\
/__/ /\ __\    \|__|\|__|\|_______|\|_______|       |\_________\\___/ /        \|__| \|__|    \|__|  \|__|\|__|
|__|/ \|__|                                         \|_________\|___|/                                         
                                                                                                               
                                                                                                               
// Tom - How to setup with this script
// Import meta all in one sdk
// Add CameraRig, Passthrough & handtracking (Meta blocks)
// Add 3d object, audioSource, 2xMaterials (1xCurrent object mat, 2xMat to change to)
// add a tag to hand (#Hand)
// add this script to the 3D object and add the audio source & second mat to it

*/

using UnityEngine;

public class ToggleInteraction : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public string handTag = "Hand";

    public Material newMaterial;
    private Material originalMaterial;
    private Renderer objectRenderer;

    public GameObject particlesOn;

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
        if (other.CompareTag(handTag)) // added collider to hand and object - added a 'Hand' tag to hand 
        {

            isTouched = !isTouched;

            if (isTouched)
            {
                // Play
                if (audioSource != null && audioClip != null)
                {
                    // Had an issue with audio not playing because the audio source was already playing. 
                    // By changing to one shot it seems to work (https://discussions.unity.com/t/how-does-audiosource-playoneshot-works/787345) 

                    audioSource.Stop(); // Had 
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
            }
        }
    }
}
