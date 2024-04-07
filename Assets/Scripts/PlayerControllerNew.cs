using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerNew : MonoBehaviour
{
    public float rotationSpeed = 70f;

    public GameObject bullet;
    public float moveSpeed = 5f;
    public float fireRate = 10f;

    public GameObject healthBar;

    private Rigidbody rb;
    public Animator animator;
    private float halfSpeed;
    private GameObject theBullet;
    public GameObject firePoint;

    private float lastFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        halfSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (lastFire <= 0)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                Instantiate<GameObject>(bullet, firePoint.transform.position, firePoint.transform.rotation);
                lastFire = fireRate;
            }
        }
        else
        {
            lastFire -= Time.deltaTime;
        }
        */
        float mouseX = Input.GetAxis("Right Stick X");
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);

        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        /*
        if (horizontalInput < 0 || horizontalInput > 0 || verticalInput < 0 || verticalInput > 0)
        {
            // Calculate the movement direction
            Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            var targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            rb.AddForce(moveDirection * moveSpeed);

        }
        */

        Vector3 forwardDirection = transform.forward;
        rb.AddForce(forwardDirection * verticalInput * moveSpeed * Time.deltaTime);

        float velocity = rb.velocity.magnitude;

        if (velocity >= -halfSpeed && velocity <= halfSpeed)
        {
            if (animator.GetBool("isWalking") || animator.GetBool("isRunning"))
            {
                // Idle
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            // Walking
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
    }
}
