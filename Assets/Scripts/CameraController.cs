using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float distanceY;
    public float distanceZ;



    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 fixedCameraPosition = new Vector3(0, distanceY, distanceZ);

        // Calculate the desired camera position
        Vector3 desiredPosition = target.position + fixedCameraPosition;
        desiredPosition.y = fixedCameraPosition.y;
        transform.position = desiredPosition;
    }
}
