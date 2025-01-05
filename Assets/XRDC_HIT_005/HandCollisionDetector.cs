using UnityEngine;
using UnityEngine.Events;

public class CollisionEventTrigger : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onCollisionEnterEvent;
    public UnityEvent onTriggerEnterEvent;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);
            onCollisionEnterEvent?.Invoke(); // Fire UnityEvent
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            Debug.Log("Trigger entered by: " + other.gameObject.name);
            onTriggerEnterEvent?.Invoke(); // Fire UnityEvent
        }
    }
}