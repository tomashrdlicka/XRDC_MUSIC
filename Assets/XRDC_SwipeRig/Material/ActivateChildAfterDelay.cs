using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChildAfterDelay : MonoBehaviour
{
    public GameObject colliderObject;
    public float delayTime = 1f; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayChildOn());
    }

    public IEnumerator DelayChildOn()
    {
        yield return new WaitForSeconds(delayTime);
        colliderObject.SetActive(true);
    }
}
