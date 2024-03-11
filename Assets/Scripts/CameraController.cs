using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    private Vector3 fixedCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        fixedCameraPosition = transform.position;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        // Calculate the desired camera position
        Vector3 desiredPosition = target.position + fixedCameraPosition;
        desiredPosition.y = fixedCameraPosition.y;
        transform.position = desiredPosition;
    }
}
