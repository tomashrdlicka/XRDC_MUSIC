using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    public float lineLength = 5f; 
    public float spiralRadius = 0.2f; 
    public float spiralSpeed = 5f; 
    public float fadeSpeed = 1f; 

    private LineRenderer[] lineRenderers;
    private Material lineMaterial;
    private float timeElapsed;

    void Start()
    {
        // Create three LineRenderers
        lineRenderers = new LineRenderer[3];
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            GameObject lineObject = new GameObject($"LineRenderer_{i}");
            lineObject.transform.parent = transform;

            LineRenderer lr = lineObject.AddComponent<LineRenderer>();
            lr.positionCount = 2; // Two points for each line
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.useWorldSpace = false;
            lr.material = new Material(Shader.Find("Sprites/Default")); // Simple material
            lr.material.color = new Color(1f, 1f, 1f, 1f); // Fully opaque initially

            lineRenderers[i] = lr;
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Update the straight line
        lineRenderers[0].SetPosition(0, Vector3.zero);
        lineRenderers[0].SetPosition(1, Vector3.forward * lineLength);

        // Update the spiral lines
        for (int i = 1; i < lineRenderers.Length; i++)
        {
            float angle = timeElapsed * spiralSpeed + (i - 1) * Mathf.PI; // Offset for second spiral
            Vector3 startPos = new Vector3(Mathf.Cos(angle) * spiralRadius, Mathf.Sin(angle) * spiralRadius, 0f);
            Vector3 endPos = new Vector3(Mathf.Cos(angle) * spiralRadius, Mathf.Sin(angle) * spiralRadius, lineLength);

            lineRenderers[i].SetPosition(0, startPos);
            lineRenderers[i].SetPosition(1, endPos);
        }

        // Fade out the lines
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            Color currentColor = lineRenderers[i].material.color;
            currentColor.a = Mathf.Max(0, currentColor.a - fadeSpeed * Time.deltaTime);
            lineRenderers[i].material.color = currentColor;
        }
    }
}