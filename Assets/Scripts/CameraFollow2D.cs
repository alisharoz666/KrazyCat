using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // The player

    [Header("Follow Settings")]
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // Adjust if you want the player not centered

    [Header("Bounds")]
    public float minX;
    public float maxX;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Clamp camera's x-position to min/max bounds
        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        Vector3 clampedPosition = new Vector3(clampedX, desiredPosition.y, desiredPosition.z);

        // Smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}

