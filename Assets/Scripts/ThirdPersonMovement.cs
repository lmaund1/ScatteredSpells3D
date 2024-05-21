using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Transform playerCamera;
    public Animator animator;
    public float currentSpellCost = 10f;

    public float moveSpeed = 6f;

    public float smoothing = 0.1f;
    float _smoothingVelocity;

    float _gravity = -9.81f;
    [SerializeField] float gravityMultiplier = 3f;
    float _velocity;

    Vector3 _previousPosition;

    public float bulletSpeed = 50f;
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    public GameObject firePoint;
    public GameObject lightningPoint;


    public GameObject healthBar;

    public string CurrentSpell { get; set; } = "lightning";
    private bool isFiring = false;
    private GameObject lightning;
    public GameObject lightningPrefab;

    private bool takingDamage = false;

    private enum PlayerState
    {
        idle, walking, running, casting, dying, dead
    }

    private PlayerState _playerState = PlayerState.idle;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        

        _previousPosition = characterController.transform.position;

        switch (_playerState)
        {
            //case PlayerState.casting:
                //ApplySpell();
               // break;

            default:
                ApplyGravity();
                ApplyMovement();
                ApplyAnimation();
                CheckSpell();
                CheckCast();

                //is player taking damage
                if(takingDamage)
                    GameController.Instance.TakeDamage(10f * Time.deltaTime);
                break;
        }


        
    }


    void ApplyGravity()
    {
        if (characterController.isGrounded && _velocity < 0f)
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        
        Vector3 _direction = new Vector3(0f, _velocity, 0f);
        characterController.Move(_direction);
    }

    void ApplyMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 _direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (_direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _smoothingVelocity, smoothing);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
    }

    void ApplyAnimation()
    {
        if (characterController.transform.position == _previousPosition)
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

    void CheckSpell()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            CurrentSpell = "default";
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            CurrentSpell = "lightning";
        }
    }

    void ApplySpell()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdling", true);
            _playerState = PlayerState.idle;
        }
    }

    void CheckCast()
    {
        if (GameController.Instance.magika < currentSpellCost)
        {
            GameController.Instance.lastFire = 0f;
            return;
        }

        if (CurrentSpell == "lightning")
        {
            if (!isFiring)
            {
                if (Input.GetButtonDown(buttonName: "Fire1"))
                {
                    isFiring = true;
                    lightning = GameObject.Instantiate(lightningPrefab, lightningPoint.transform.position, lightningPoint.transform.rotation);
                    lightning.transform.parent = lightningPoint.transform;
                }
            }
            else
            {
                if (Input.GetButtonUp("Fire1"))
                {
                    isFiring = false;
                    Destroy(lightning);

                }
            }
        }
        else
        {
            if (GameController.Instance.lastFire <= 0)
            {
                if (Input.GetButton("Fire1"))
                {
                    _playerState = PlayerState.casting;
                    animator.SetTrigger("isShooting");

                    var projectile = Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
                    projectile.GetComponent<Rigidbody>().velocity = firePoint.transform.forward * projectileSpeed;

                    GameController.Instance.lastFire = fireRate;
                    GameController.Instance.magika -= currentSpellCost;
                }
            }
            else
            {
                GameController.Instance.lastFire -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);

        switch (other.gameObject.tag.ToLower())
        {
            case "enemy":
                takingDamage = true;
                break;

            case "greenkey":
                GameController.Instance.PickUpKey("green");
                Destroy(other.gameObject);
                break;

            case "healthpotion":
                HealthPotionController healthPotionController = other.gameObject.GetComponent<HealthPotionController>();
                if(GameController.Instance.health < GameController.Instance.maxHealth)
                {
                    GameController.Instance.RestoreHealth(healthPotionController.healthBoost);
                    Destroy(other.gameObject);
                }
                break;

            case "magikapotion":
                MagikaPotionController magikaPotionController = other.gameObject.GetComponent<MagikaPotionController>();
                if(GameController.Instance.magika < GameController.Instance.maxMagika)
                {
                    GameController.Instance.RestoreMagika(magikaPotionController.magikaBoost);
                    Destroy(other.gameObject);
                }
                break;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            takingDamage = false;
        }
    }

}



