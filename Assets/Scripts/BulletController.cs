using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 50f;
    public float life = 10f;
    public int strength = 20;

    private float timeLeft = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = life;
        Destroy(gameObject, life);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

 

}
