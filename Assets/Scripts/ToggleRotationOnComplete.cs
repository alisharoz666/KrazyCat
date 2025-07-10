using UnityEngine;

public class ToggleRotationOnComplete : MonoBehaviour
{
    // This function can be called from DOTween's OnComplete
    public void ToggleYRotation()
    {
        Vector3 currentRotation = transform.localEulerAngles;

        // Normalize Y angle to either 0 or 180
        float yRotation = Mathf.Round(currentRotation.y);

        if (Mathf.Approximately(yRotation, 0f))
        {
            transform.localEulerAngles = new Vector3(currentRotation.x, 180f, currentRotation.z);
        }
        else if (Mathf.Approximately(yRotation, 180f))
        {
            transform.localEulerAngles = new Vector3(currentRotation.x, 0f, currentRotation.z);
        }
        else
        {
            Debug.LogWarning("Rotation not 0 or 180, skipping toggle. Current Y: " + yRotation);
        }
    }
}
