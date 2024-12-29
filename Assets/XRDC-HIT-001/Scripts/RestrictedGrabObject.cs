using System.Collections;
using UnityEngine;

public class RestrictedGrabObject : MonoBehaviour
{
    [SerializeField] private float maxGrabDistance = 1.5f; // Maximum distance the object can be dragged.
    [SerializeField] private float snapBackSpeed = 5f; // Speed at which the object snaps back.

    private Vector3 originalPosition; // Original position of the object.
    private bool isGrabbed = false; // Tracks whether the object is currently grabbed.

    private void Start()
    {
        // Store the initial position of the object.
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isGrabbed)
        {
            // Restrict the object's position to the maximum grab distance.
            Vector3 direction = transform.position - originalPosition;
            if (direction.magnitude > maxGrabDistance)
            {
                transform.position = originalPosition + direction.normalized * maxGrabDistance;
            }
        }
    }

    // Call this method when the grab interaction starts.
    public void OnGrabBegin()
    {
        isGrabbed = true;
    }

    // Call this method when the grab interaction ends.
    public void OnGrabEnd()
    {
        isGrabbed = false;
        StartCoroutine(SnapBack());
    }

    // Coroutine to smoothly snap the object back to its original position.
    private IEnumerator SnapBack()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, snapBackSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure it snaps exactly to the original position.
        transform.position = originalPosition;
    }
}