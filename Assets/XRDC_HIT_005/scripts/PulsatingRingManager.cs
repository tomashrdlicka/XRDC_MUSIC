using UnityEngine;
using System.Collections.Generic;

public class PulsatingRingsManager : MonoBehaviour
{
    public GameObject ringPrefab; // Assign your ring prefab
    public Transform cameraTransform; // Assign your camera transform
    public float spawnInterval = 1.0f; // Time between each ring spawn
    public float ringLifetime = 3.0f; // How long a ring lasts before being destroyed
    public float pulseHeight = 2.0f; // Distance rings move upward
    public float fadeSpeed = 1.0f; // Speed at which rings fade out
    public float floorOffset = 0.1f; // Initial height offset from the floor
    public Vector3 rotationOffset = new Vector3(90f, 0f, 0f); // 90º rotation for rings

    private bool isSpawning = false;
    private List<GameObject> activeRings = new List<GameObject>(); // Track spawned rings

    void Start()
    {
        // Optionally start spawning at runtime
        StartSpawning();
    }

    /// <summary>
    /// Starts spawning rings.
    /// </summary>
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnRing), 0f, spawnInterval);
        }
    }

    /// <summary>
    /// Stops spawning rings and destroys all active rings.
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(SpawnRing));

        // Destroy all active rings
        foreach (GameObject ring in activeRings)
        {
            if (ring != null)
                Destroy(ring);
        }

        // Clear the list of active rings
        activeRings.Clear();
    }

    /// <summary>
    /// Spawns a new ring at the camera's position aligned with the floor.
    /// </summary>
    private void SpawnRing()
    {
        Vector3 spawnPosition = new Vector3(cameraTransform.position.x, floorOffset, cameraTransform.position.z);
        Quaternion spawnRotation = Quaternion.Euler(rotationOffset);
        GameObject newRing = Instantiate(ringPrefab, spawnPosition, spawnRotation);

        // Add to the list of active rings
        activeRings.Add(newRing);

        // Start the ring's upward movement and fade-out coroutine
        StartCoroutine(MoveAndFadeRing(newRing));
    }

    /// <summary>
    /// Moves the ring upward and fades it out over time.
    /// </summary>
    private System.Collections.IEnumerator MoveAndFadeRing(GameObject ring)
    {
        float timer = 0f;
        Renderer ringRenderer = ring.GetComponent<Renderer>();
        if (ringRenderer == null)
        {
            Debug.LogWarning("Ring prefab is missing a Renderer component.");
            Destroy(ring);
            activeRings.Remove(ring);
            yield break;
        }

        Color originalColor = ringRenderer.material.color;

        while (timer < ringLifetime)
        {
            // Check if the ring was removed (e.g., when StopSpawning is called)
            if (ring == null) yield break;

            // Move upward
            ring.transform.position += Vector3.up * (pulseHeight / ringLifetime) * Time.deltaTime;

            // Fade out
            float fadeAmount = 1f - (timer / ringLifetime);
            Color fadedColor = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAmount);
            ringRenderer.material.color = fadedColor;

            timer += Time.deltaTime;
            yield return null;
        }

        // Destroy the ring when its lifetime is over
        if (ring != null)
        {
            Destroy(ring);
            activeRings.Remove(ring);
        }
    }
}