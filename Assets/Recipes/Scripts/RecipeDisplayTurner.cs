using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool lockYAxis = true;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform mainCameraTransform;

    void Start()
    {
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (mainCameraTransform == null) return;

        // Calculate direction to camera
        Vector3 targetDirection = mainCameraTransform.position - transform.position;

        // Keep object upright if Y-axis locking is enabled
        if (lockYAxis)
        {
            targetDirection.x = 0;
        }

        if (targetDirection != Vector3.zero)
        {
            // Calculate target rotation (negated so the front faces the camera, not the back)
            Quaternion targetRotation = Quaternion.LookRotation(-targetDirection);

            // Smoothly interpolate to the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}