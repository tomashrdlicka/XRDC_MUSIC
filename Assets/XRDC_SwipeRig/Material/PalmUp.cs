using UnityEngine;
using UnityEngine.Events;

public class PalmUp : MonoBehaviour
{
    public OVRSkeleton skeleton; // Assign the hand skeleton (left or right)
    public float angleThreshold = 30f; // Threshold for detecting palm up

    // Events for palm up and palm down
    public UnityEvent OnPalmUp;
    public UnityEvent OnPalmDown;

    private bool isPalmUp = false; // Track whether the palm is currently up

    void Update()
    {
        if (skeleton != null && skeleton.IsDataValid)
        {
            // Get the wrist bone's transform
            Transform wristTransform = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot]?.Transform;

            if (wristTransform != null)
            {
                // Palm normal direction
                Vector3 palmNormal = wristTransform.up;

                // World up direction
                Vector3 worldUp = Vector3.up;

                // Calculate the angle between the palm normal and the world up vector
                float angle = Vector3.Angle(palmNormal, worldUp);

                // Check if the palm is up
                if (angle < angleThreshold)
                {
                    if (!isPalmUp)
                    {
                        isPalmUp = true;
                        Debug.Log("Palm is up!");
                        OnPalmUp?.Invoke(); // Trigger the "palm up" event
                    }
                }
                else
                {
                    if (isPalmUp)
                    {
                        isPalmUp = false;
                        Debug.Log("Palm is no longer up!");
                        OnPalmDown?.Invoke(); // Trigger the "palm down" event
                    }
                }
            }
        }
    }
}