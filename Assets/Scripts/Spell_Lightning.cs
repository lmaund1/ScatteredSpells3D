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

    public void Awake()
    {
   


    }

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
        //Vector3 startP = new Vector3(start.x, start.y, start.z);
        goStart.transform.position = transform.position;
        

        
        RaycastHit hit;

        //Vector3 hitPoint = new Vector3(transform.position.x, 2f, transform.position.z);
        if (Physics.Raycast(end, Vector3.forward, out hit, lightningLength))
        {
            end = transform.position + transform.forward * hit.distance;
            //_end = new Vector3(_end.x, transform.position.y, _end.z);

            // _end = hit.point;

            if (hit.rigidbody is not null)
            {
                if (hit.rigidbody.CompareTag("Enemy"))
                {
                    SkeletonController skeletonController = hit.rigidbody.GetComponent<SkeletonController>();
                    if (skeletonController != null)
                    {
                        skeletonController.HitByRayCast(1f);

                    }
                }
            }
        }
        else
        {
            Physics.Raycast(transform.position, Vector3.down, out hit);
            //_end = transform.position + transform.up * lightningLength;
            end = new Vector3(end.x, hit.point.y, end.z);
        }
        goEnd.transform.position = end;
    }
}
