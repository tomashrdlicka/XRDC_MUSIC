using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent

public class HandTriggerEvent : MonoBehaviour
{
    public string handTag = "Hand";

    public UnityEvent onTouchEnter; // Event triggered on touch
    public UnityEvent onTouchExit; // Event triggered on touch end

    private bool isTouched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag)) // Triggered by tagged "Hand"
        {
            isTouched = !isTouched;

            if (isTouched)
            {
                onTouchEnter?.Invoke(); // Trigger the onTouchEnter event
            }
            else
            {
                onTouchExit?.Invoke(); // Trigger the onTouchExit event
            }
        }
    }
}