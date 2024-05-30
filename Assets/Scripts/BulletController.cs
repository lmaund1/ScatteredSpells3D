using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>
    /// The speed at which the bullet moves.
    /// </summary>
    public float bulletSpeed = 50f;

    /// <summary>
    /// The lifespan of the bullet in seconds.
    /// </summary>
    public float life = 10f;

    /// <summary>
    /// The strength of the bullet.
    /// </summary>
    public int strength = 20;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, life);
    }

    /// <summary>
    /// Called when the bullet collides with another object.
    /// </summary>
    /// <param name="collision">The collision data.</param>
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
