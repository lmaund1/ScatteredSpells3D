using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public float life = 10f;

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
        transform.Translate(transform.forward * bulletSpeed * Time.deltaTime);
    }
}
