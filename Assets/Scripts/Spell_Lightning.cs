using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Spell_Lightning : MonoBehaviour
{
    public GameObject lightning;
    public GameObject spherePrefab;

    public float distance = 10f;

    private VisualEffect _lightningFX;
    //private Transform playerTransform;

    private GameObject sphere;
    // not updating player's position needs to be paired to the firepoint

    // Start is called before the first frame update
    void Awake()
    {
        _lightningFX = lightning.GetComponent<VisualEffect>();
        GameObject go = gameObject;
        while (go.CompareTag("Player"))
        {
            go = go.transform.parent.gameObject;
        }
        //playerTransform = go.transform;

        // this is test only 
        sphere = GameObject.Instantiate(spherePrefab);
        GameObject.Instantiate(spherePrefab).transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 spherePos = transform.position + transform.forward * 2f;
        sphere.transform.position = spherePos;
        _lightningFX.SetVector3("Pos1", Vector3.zero);
        _lightningFX.SetVector3("Pos2", sphere.transform.position);
    }

    void _Update()
    {
        float endDistance = distance;
        Vector3 _end;
        RaycastHit hit;

        Vector3 hitPoint = new Vector3(transform.position.x, 2f, transform.position.z);
        if (Physics.Raycast(hitPoint, Vector3.up, out hit, distance))
        {
            _end = transform.position + transform.up * hit.distance;
            _end = new Vector3(_end.x, transform.position.y, _end.z);

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
            _end = transform.position + transform.up * distance;
            _end = new Vector3(_end.x, hit.point.y, _end.z);
        }

        ///_lightningFX.SetVector3("Pos1", transform.position + transform.forward);
        _lightningFX.SetVector3("Pos2", _end);
    }
}
