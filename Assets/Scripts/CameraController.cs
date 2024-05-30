using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// The target object that the camera will follow.
    /// </summary>
    public Transform target;

    /// <summary>
    /// The distance of the camera along the Y-axis from the target object.
    /// </summary>
    public float distanceY;

    /// <summary>
    /// The distance of the camera along the Z-axis from the target object.
    /// </summary>
    public float distanceZ;

    /// <summary>
    /// The rotation speed of the camera.
    /// </summary>
    public float rotationSpeed = 70f;

    /// <summary>
    /// The smoothing factor for camera movement.
    /// </summary>
    public float smoothing = 0.5f;

    private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - target.position;
    }
    
    // Update is called once per frame
    // LateUpdate is called once per frame, after all Update functions have been called
    void LateUpdate()
    {
        // Get the mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the horizontal rotation based on mouse input and rotation speed
        float horizRotation = mouseX * rotationSpeed * Time.deltaTime;

        // Create a rotation quaternion around the up axis
        Quaternion turnAngleX = Quaternion.AngleAxis(horizRotation, Vector3.up);

        // Apply the rotation to the camera offset
        cameraOffset = turnAngleX * cameraOffset;

        // Calculate the new position of the camera
        Vector3 newPos = target.position + cameraOffset;

        // Smoothly move the camera to the new position
        transform.position = Vector3.Slerp(transform.position, newPos, smoothing);

        // Make the camera look at the target object
        transform.LookAt(target.position);
    }
}
