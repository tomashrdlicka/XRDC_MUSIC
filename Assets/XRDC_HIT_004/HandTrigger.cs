using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onCollisionEvent; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            onCollisionEvent?.Invoke(); 
        }
    }
}