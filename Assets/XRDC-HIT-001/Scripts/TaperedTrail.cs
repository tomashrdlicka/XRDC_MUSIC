using UnityEngine;

public class TaperedTrail : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float trailDuration = 2f; 
    public int maxTrailPoints = 50;  
    public float trailWidth = 0.2f; 
    private float pointSpacing = 0.1f; 
    
    private Vector3 lastPosition;
    private float timeSinceLastPoint;

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 0;
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Add a new point if the object moves enough
        float distance = Vector3.Distance(transform.position, lastPosition);
        if (distance >= pointSpacing)
        {
            AddTrailPoint(transform.position);
            lastPosition = transform.position;
        }

        // Update the trail over time
        UpdateTrail();
    }

    private void AddTrailPoint(Vector3 position)
    {
        // Add a new point to the LineRenderer
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    private void UpdateTrail()
    {
        if (lineRenderer.positionCount == 0) return;

        // Reduce trail width over time
        float[] widths = new float[lineRenderer.positionCount];
        for (int i = 0; i < widths.Length; i++)
        {
            float timeFactor = (float)i / (widths.Length - 1);
            widths[i] = Mathf.Lerp(0, trailWidth, timeFactor);
        }

        lineRenderer.widthCurve = CreateWidthCurve(widths);

        // Remove oldest points if trail is too long or has existed too long
        float elapsedTime = timeSinceLastPoint / trailDuration;
        if (elapsedTime > 1 || lineRenderer.positionCount > maxTrailPoints)
        {
            RemoveOldestPoint();
        }
    }

    private void RemoveOldestPoint()
    {
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i + 1));
        }
        lineRenderer.positionCount--;
    }

    private AnimationCurve CreateWidthCurve(float[] widths)
    {
        AnimationCurve curve = new AnimationCurve();
        for (int i = 0; i < widths.Length; i++)
        {
            float time = (float)i / (widths.Length - 1);
            curve.AddKey(time, widths[i]);
        }
        return curve;
    }
}