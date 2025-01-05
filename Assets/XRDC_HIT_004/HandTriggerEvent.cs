using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent

public class HandTriggerEvent : MonoBehaviour
{
    public string handTag = "Hand";

    public UnityEvent onTouchEnter; // Event triggered on touch
    public UnityEvent onTouchExit; // Event triggered on touch end

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag)) // Triggered by tagged "Hand"
        {
            onTouchEnter?.Invoke(); // Trigger the onTouchEnter event
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag)) // Triggered by tagged "Hand"
        {
            onTouchExit?.Invoke(); // Trigger the onTouchExit event
        }
    }
}