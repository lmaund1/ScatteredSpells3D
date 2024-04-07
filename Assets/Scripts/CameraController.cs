using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float distanceY;
    public float distanceZ;

    public float rotationSpeed = 70f;
    public float smoothing = 0.5f;

    private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - target.position;
        
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        //float horizRotation = Mathf.Clamp(mouseX * rotationSpeed * Time.deltaTime, -90f, 90f);
        float horizRotation = mouseX * rotationSpeed * Time.deltaTime;
        Quaternion turnAngleX = Quaternion.AngleAxis(horizRotation, Vector3.up);
        cameraOffset = turnAngleX * cameraOffset;

        Vector3 newPos = target.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothing);
        transform.LookAt(target.position);

        Debug.Log(mouseX);

        /*
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        transform.LookAt(target);

        
        Vector3 fixedCameraPosition = new Vector3(0, distanceY, distanceZ);

        // Calculate the desired camera position
        Vector3 desiredPosition = target.position + fixedCameraPosition;
        desiredPosition.y = fixedCameraPosition.y;
        transform.position = desiredPosition;
        */


    }
}
