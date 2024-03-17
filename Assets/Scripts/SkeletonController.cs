using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public GameObject player;
    public Transform[] wayPoints;
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float sightRadius = 10f;
    public float horizontalFoV = 0f;
    public float verticalFoV = 0f;
    public LayerMask playerMask;

    private int currentWayPointIndex = 0;
    private Animator animator;

    private enum SkeletonState
    {
        idle, walking, engaging, running, attacking, dying, dead 
    }

    private SkeletonState skeletonState = SkeletonState.idle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (skeletonState)
        {
            case SkeletonState.walking:
                MoveToWayPoint();
                if (IsPlayerVisible())
                {
                    skeletonState = SkeletonState.engaging;
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isWalking", false);
                    animator.SetTrigger("isEngaging");
                }
                break;

            case SkeletonState.engaging:
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                {
                    skeletonState = SkeletonState.running;
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", false);
                }
                break;

            case SkeletonState.running:
               RunToPlayer();
                break;

            default:
                if (animator.GetBool("isWalking") == false && wayPoints.Length > 0)
                {
                    animator.SetBool("isWalking", true);
                    skeletonState = SkeletonState.walking;
                }
                break;
        }
        // 
        

        
        
    }

    private void MoveToWayPoint()
    {
        Vector3 targetPosition = wayPoints[currentWayPointIndex].position;
        float step = walkSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        Vector3 directionToWayPoint = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToWayPoint);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, walkSpeed * Time.deltaTime);
        

        // check if waypoint reached
        if(transform.position == targetPosition)
        {
            // increases or resets index for current way point 
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
        }
    }

    private bool RunToPlayer()
    {
        bool caughtPlayer = false;
        Vector3 targetPosition = player.transform.position;
        float step = runSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        Vector3 directionToWayPoint = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToWayPoint);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, runSpeed * Time.deltaTime);


        // check if waypoint reached
        if (transform.position == targetPosition)
        {
            caughtPlayer = true;
        }
        return caughtPlayer;
    }

    private bool IsPlayerVisible()
    {
        bool isVisible = false;

        Collider[] targetsInSightRadius = Physics.OverlapSphere(transform.position, sightRadius, playerMask);
        isVisible = targetsInSightRadius.Length > 0;
        return isVisible;
    }
}
