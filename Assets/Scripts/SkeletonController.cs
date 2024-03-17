using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public GameObject player;
    public Transform[] wayPoints;
    public float moveSpeed = 1f;
    public float sightRange = 10f;

    private int currentWayPointIndex = 0;
    private Animator animator;

    private enum SkeletonState
    {
        idle, walking, engaging, attacking, dying, dead 
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
                    animator.SetBool("isEngaging", true);
                }
                break;

            default:
                if (animator.GetBool("isWalking") == false && wayPoints.Length > 0)
                {
                    animator.SetBool("isWalking", true);
                    skeletonState = SkeletonState.walking;
                }
                IsPlayerVisible();
                break;
        }
        // 
        

        
        
    }

    private void MoveToWayPoint()
    {
        Vector3 targetPosition = wayPoints[currentWayPointIndex].position;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        Vector3 directionToWayPoint = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToWayPoint);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
        

        // check if waypoint reached
        if(transform.position == targetPosition)
        {
            // increases or resets index for current way point 
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
        }
    }

    private bool IsPlayerVisible()
    {
        bool isVisible = false;

        Vector3 directionToPlayer = player.transform.position - transform.position;

        Ray ray = new Ray(transform.position, transform.forward);
        // cast a ray in the direction of the player
        if(Physics.Raycast(ray, sightRange, 7))
        {
                       
                // the enemy can see the player
                isVisible = true;
                Debug.Log("hit");

        }
        return isVisible;
    }
}
