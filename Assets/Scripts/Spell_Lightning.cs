using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Spell_Lightning : MonoBehaviour
{
    public GameObject lightning;

    public float distance = 2.0f;

    private VisualEffect _lightningFX;
    private Transform playerTransform;

    // not updating player's position needs to be paired to the firepoint

    // Start is called before the first frame update
    void Start()
    {
        _lightningFX = lightning.GetComponent<VisualEffect>();
        GameObject go = gameObject;
        while (go.CompareTag("Player"))
        {
            go = go.transform.parent.gameObject;
        }
        playerTransform = go.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float endDistance = distance;
        Vector3 _end;
        RaycastHit hit;


        if (Physics.Raycast(transform.position, transform.up, out hit, distance))
        {
            _end = transform.position + transform.up * hit.distance;
            _end = new Vector3(_end.x, transform.position.y, _end.z);
            if (hit.rigidbody.CompareTag("Enemy"))
            {
                SkeletonController skeletonController = hit.rigidbody.GetComponent<SkeletonController>();
                if(skeletonController != null)
                {
                    skeletonController.HitByRayCast(1f);
                }
            }
        }
        else
        {
            Physics.Raycast(playerTransform.position, Vector3.down, out hit);
            _end = transform.position + transform.up * distance;
            _end = new Vector3(_end.x, hit.point.y, _end.z);
        }

        _lightningFX.SetVector3("Pos1", transform.position);
        _lightningFX.SetVector3("Pos2", _end);
    }
}
