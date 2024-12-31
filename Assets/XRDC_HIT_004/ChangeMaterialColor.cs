using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    [SerializeField] private Color newColor = Color.red;

    public void SetColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = newColor;
        }
    }
}