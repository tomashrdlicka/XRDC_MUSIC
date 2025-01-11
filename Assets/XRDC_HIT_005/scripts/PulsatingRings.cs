using UnityEngine;
using System.Collections.Generic;

public class PulsatingRings : MonoBehaviour
{
    public GameObject ringPrefab;       // Prefab for the ring (a flat circle)
    public float pulseDuration = 1.5f; // Time for one pulse cycle (expand and fade)
    public float spawnInterval = 0.5f; // Time between new ring spawns
    public float maxScale = 3f;        // Maximum size of the ring
    public float minScale = 0.5f;      // Starting size of the ring
    public Color startColor = Color.white; // Color of the ring at full opacity
    public Color endColor = new Color(1, 1, 1, 0); // Fully transparent color

    private Transform cameraTransform; // Reference to the camera transform
    private List<GameObject> activeRings = new List<GameObject>(); // Track spawned rings
    private bool spawningEnabled = true; // Control spawning state

    void Start()
    {
        cameraTransform = Camera.main.transform;
        InvokeRepeating(nameof(SpawnRing), 0f, spawnInterval);
    }

    void SpawnRing()
    {
        if (!spawningEnabled) return;

        // Instantiate the ring prefab
        GameObject ring = Instantiate(ringPrefab, GetRingSpawnPosition(), Quaternion.Euler(90f, 0f, 0f));
        ring.transform.localScale = Vector3.one * minScale;

        // Add the ring to the list of active rings
        activeRings.Add(ring);

        // Start the pulsating effect
        StartCoroutine(PulsateRing(ring));
    }

    Vector3 GetRingSpawnPosition()
    {
        // Position centered to the camera on the X and Z plane, aligned with the floor
        Vector3 position = cameraTransform.position;
        position.y = 0; // Set y to floor level
        return position;
    }

    System.Collections.IEnumerator PulsateRing(GameObject ring)
    {
        float elapsedTime = 0f;
        SpriteRenderer renderer = ring.GetComponent<SpriteRenderer>();

        while (spawningEnabled && ring != null) // Pulsate while spawning is enabled
        {
            elapsedTime += Time.deltaTime;

            // Calculate pulsation progress
            float t = (elapsedTime % pulseDuration) / pulseDuration;

            // Scale the ring
            ring.transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, t);

            // Adjust color based on progress
            if (renderer != null)
            {
                renderer.color = Color.Lerp(startColor, endColor, t);
            }

            yield return null;
        }
    }

    public void ClearRings()
    {
        // Ensure all active rings are destroyed
        for (int i = activeRings.Count - 1; i >= 0; i--) // Iterate backward to avoid index shifting
        {
            GameObject ring = activeRings[i];
            if (ring != null)
            {
                Destroy(ring);
            }
        }

        // Clear the list after destroying all rings
        activeRings.Clear();
    }

    public void ToggleSpawning(bool enabled)
    {
        spawningEnabled = enabled;

        if (!spawningEnabled)
        {
            ClearRings(); // Optionally clear rings when disabling
        }
    }
}