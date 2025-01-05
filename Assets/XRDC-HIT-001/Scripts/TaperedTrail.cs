using UnityEngine;

public class TaperedTrail : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float trailDuration = 2f;   // Time for the trail to disappear
    public float pointSpacing = 0.05f; // Distance between points for smoothness
    public float trailWidth = 0.2f;    // Initial width of the trail
    public int maxTrailPoints = 100;   // Maximum number of points in the trail

    private Vector3 lastPosition;
    private float timeSinceLastPoint;
    private float[] times; // Time each point has existed
    private Vector3[] points; // Stored points for smooth interpolation

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 0;

        // Initialize points array
        points = new Vector3[maxTrailPoints];
        times = new float[maxTrailPoints];
    }

    private void Update()
    {
        // Add points dynamically if the object moves enough
        float distance = Vector3.Distance(transform.position, lastPosition);
        if (distance >= pointSpacing)
        {
            AddTrailPoint(transform.position);
            lastPosition = transform.position;
        }

        // Update the trail width and fade effect
        UpdateTrail();
    }

    private void AddTrailPoint(Vector3 position)
    {
        // Shift older points and add the new one at the end
        for (int i = 0; i < maxTrailPoints - 1; i++)
        {
            points[i] = points[i + 1];
            times[i] = times[i + 1];
        }
        points[maxTrailPoints - 1] = position;
        times[maxTrailPoints - 1] = Time.time;

        // Update LineRenderer points
        UpdateLineRenderer();
    }

    private void UpdateTrail()
    {
        // Remove points that are too old
        float currentTime = Time.time;
        for (int i = 0; i < maxTrailPoints; i++)
        {
            if (currentTime - times[i] > trailDuration)
            {
                RemoveOldestPoint();
            }
        }

        // Smooth width transition
        UpdateWidth();
    }

    private void RemoveOldestPoint()
    {
        for (int i = 1; i < maxTrailPoints; i++)
        {
            points[i - 1] = points[i];
            times[i - 1] = times[i];
        }
        points[maxTrailPoints - 1] = Vector3.zero;
        times[maxTrailPoints - 1] = 0;
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        int activePointCount = 0;
        for (int i = 0; i < maxTrailPoints; i++)
        {
            if (times[i] != 0)
            {
                activePointCount++;
            }
        }

        lineRenderer.positionCount = activePointCount;
        for (int i = 0; i < activePointCount; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    private void UpdateWidth()
    {
        AnimationCurve widthCurve = new AnimationCurve();
        int activePointCount = lineRenderer.positionCount;

        for (int i = 0; i < activePointCount; i++)
        {
            float timeFactor = (float)i / (activePointCount - 1);
            float width = Mathf.Lerp(0, trailWidth, timeFactor);
            widthCurve.AddKey(timeFactor, width);
        }

        lineRenderer.widthCurve = widthCurve;
    }
}