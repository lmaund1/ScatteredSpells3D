using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of a skeleton enemy in the game.
/// </summary>
public class SkeletonController : MonoBehaviour
{
    // Public variables
    public GameObject player;
    public Transform[] wayPoints;
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float sightRadius = 10f;
    public float attackRadius = 2f;
    public float horizontalFoV = 0f;
    public float verticalFoV = 0f;
    public LayerMask playerMask;
    public int health = 100;
    public GameObject enemyHealthBar;
    public AudioClip audioHit;

    // Private variables
    private AudioSource audioSource;
    private EnemyHealthBar enemyHealthController;
    private int currentHealth;
    private float deadFrames;
    private int currentWayPointIndex = 0;
    private Animator animator;

    // Enumeration for the different states of the skeleton
    public enum SkeletonState
    {
        idle, walking, engaging, running, attacking, takingDamage, dying, dead
    }

    private SkeletonState skeletonState = SkeletonState.idle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyHealthController = enemyHealthBar.GetComponent<EnemyHealthBar>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = health;

        animator.SetBool("isIdle", true);
    }

    // Update is called once per frame
    void Update()
    {

        switch (skeletonState)
        {
            case SkeletonState.walking:
                if (wayPoints.Length > 0)
                    MoveToWayPoint();

                if (IsPlayerVisible())
                {
                    ChangeState(SkeletonState.engaging);
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isWalking", false);
                    animator.SetTrigger("isEngaging");
                }
                break;

            case SkeletonState.engaging:
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                {
                    ChangeState(SkeletonState.running);
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", false);
                }
                break;

            case SkeletonState.dying:
                deadFrames -= Time.deltaTime;
                if (deadFrames < 0)
                {
                    skeletonState = SkeletonState.dead;
                }
                break;

            case SkeletonState.dead:
                Destroy(gameObject);
                break;

            case SkeletonState.running:
                if (RunToPlayer())
                {
                    ChangeState(SkeletonState.attacking);
                    animator.SetTrigger("isAttacking");
                }
                break;

            case SkeletonState.attacking:
                transform.LookAt(player.transform);
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                {
                    ChangeState(SkeletonState.running);
                }
                break;

            case SkeletonState.takingDamage:
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                {
                    ChangeState(SkeletonState.engaging);
                    audioSource.clip = audioHit;
                    audioSource.loop = false;
                    audioSource.Play();
                }
                break;

            default:
                if (animator.GetBool("isWalking") == false && wayPoints.Length > 0)
                {
                    animator.SetBool("isWalking", true);
                    ChangeState(SkeletonState.walking);
                }
                break;
        }
    }

    /// <summary>
    /// Moves the skeleton towards the current waypoint.
    /// </summary>
    private void MoveToWayPoint()
    {
        Vector3 targetPosition = wayPoints[currentWayPointIndex].position;
        float step = walkSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        Vector3 directionToWayPoint = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToWayPoint);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, walkSpeed * Time.deltaTime);

        // Check if waypoint reached
        if (transform.position == targetPosition)
        {
            // Increases or resets index for current waypoint
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
        }
    }

    /// <summary>
    /// Moves the skeleton towards the player.
    /// </summary>
    /// <returns>True if the player is caught, false otherwise.</returns>
    private bool RunToPlayer()
    {
        bool caughtPlayer = false;
        Vector3 targetPosition = player.transform.position;
        float step = runSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        Vector3 directionToWayPoint = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToWayPoint);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, runSpeed * Time.deltaTime);

        // Check if player is caught or out of sight
        float playerDistance = Vector3.Distance(transform.position, targetPosition);
        if (playerDistance <= attackRadius)
        {
            caughtPlayer = true;
        }
        else
        {
            if (playerDistance > sightRadius)
            {
                ChangeState(SkeletonState.walking);
            }
        }
        return caughtPlayer;
    }

    /// <summary>
    /// Checks if the player is visible within the sight radius.
    /// </summary>
    /// <returns>True if the player is visible, false otherwise.</returns>
    private bool IsPlayerVisible()
    {
        bool isVisible = false;

        Collider[] targetsInSightRadius = Physics.OverlapSphere(transform.position, sightRadius, playerMask);
        isVisible = targetsInSightRadius.Length > 0;
        return isVisible;
    }

    /// <summary>
    /// Handles the collision with other objects.
    /// </summary>
    /// <param name="other">The collider of the other object.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (skeletonState != SkeletonState.dying)
        {
            if (other.CompareTag("Bullet"))
            {
                int strength = other.gameObject.GetComponent<BulletController>().strength;
                currentHealth = currentHealth - strength;
                ChangeState(SkeletonState.takingDamage);
                animator.SetTrigger("isHit");
                enemyHealthController.takeDamage(strength);
                Destroy(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Handles the skeleton being hit by a raycast.
    /// </summary>
    /// <param name="strength">The strength of the raycast hit.</param>
    public void HitByRayCast(float strength)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("isDead"))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("isHit"))
            {
                animator.SetTrigger("isHit");
                ChangeState(SkeletonState.takingDamage);
            }

            currentHealth = currentHealth - (int)strength;
            enemyHealthController.takeDamage(strength);
        }
    }

    /// <summary>
    /// Changes the state of the skeleton.
    /// </summary>
    /// <param name="newState">The new state to change to.</param>
    public void ChangeState(SkeletonState newState)
    {
        if (currentHealth <= 0)
        {
            deadFrames = 3f;
            skeletonState = SkeletonState.dying;
            animator.SetTrigger("isDead");
        }
        else
        {
            skeletonState = newState;
        }
    }
}
