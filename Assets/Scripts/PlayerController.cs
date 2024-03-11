using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;

    private Rigidbody rb;
    public Animator animator;
    private float halfSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        halfSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;


        var targetRotation = Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        rb.AddForce(moveDirection * moveSpeed);

        


        float velocity = rb.velocity.magnitude;

        if(velocity >= -halfSpeed && velocity <= halfSpeed )
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