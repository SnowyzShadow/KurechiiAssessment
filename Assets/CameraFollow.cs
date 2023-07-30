using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;        // The target object to follow
    [SerializeField] private float smoothSpeed = 0.125f;  // The smoothness of camera movement. Lower values mean smoother movement.
    [SerializeField] private Vector3 offset = Vector3.zero;  // The offset between the camera and the target.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not assigned. Please assign it in the Inspector.");
            return;
        }

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
