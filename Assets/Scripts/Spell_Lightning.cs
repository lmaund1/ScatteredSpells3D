using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Spell_Lightning : MonoBehaviour
{
    public float lightningLength = 2f;
    public float offset = 1f;

    private GameController gameController;
    private Vector3 start;
    private GameObject goStart;
    private GameObject goEnd;

    /// <summary>
    /// This method is called when the object is first enabled in the scene.
    /// It initializes the game controller, finds the start and end game objects,
    /// and sets the start position to the current position of the object.
    /// </summary>
    public void Start()
    {
        gameController = GameController.Instance;
        goStart = transform.Find("Start").gameObject;
        goEnd = transform.Find("End").gameObject;

        start = transform.position;
    }

    public void Update()
    {
        Vector3 end = transform.position + transform.forward * lightningLength;
        goStart.transform.position = transform.position;



        RaycastHit hit;  // Declare a variable named "hit" of type RaycastHit.

        if (Physics.Raycast(end, Vector3.forward, out hit, lightningLength))
        {
            // If a raycast from the "end" position in the forward direction hits something within the "lightningLength" distance:
            end = transform.position + transform.forward * hit.distance;
            // Update the "end" position to be the point where the raycast hit.

            if (hit.rigidbody is not null)
            {
                // If the hit object has a Rigidbody component:
                if (hit.rigidbody.CompareTag("Enemy"))
                {
                    // If the hit object's tag is "Enemy":
                    SkeletonController skeletonController = hit.rigidbody.GetComponent<SkeletonController>();
                    if (skeletonController != null)
                    {
                        // Get the SkeletonController component from the hit object's Rigidbody.
                        // If it exists, call the HitByRayCast method with a damage value of 1f.
                        skeletonController.HitByRayCast(1f);
                    }
                }
            }
        }
        else
        {
            // If the raycast did not hit anything:
            Physics.Raycast(transform.position, Vector3.down, out hit);
            // Perform a raycast from the current position downward.

            end = new Vector3(end.x, hit.point.y, end.z);
            // Update the "end" position to be at the same x and z coordinates but with the y coordinate adjusted to the hit point's y value.
        }

        goEnd.transform.position = end;
        // Update the position of the "goEnd" object to match the updated "end" position.

    }
}
