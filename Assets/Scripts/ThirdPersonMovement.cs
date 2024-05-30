using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    // References to other components and objects
    public CharacterController characterController;
    public Transform playerCamera;
    public Animator animator;
    
    public GameObject playerStart;

    // Movement variables
    public float moveSpeed = 6f;
    private int gameOverLoop = 0;
    Vector3 _previousPosition;

    // Gravity variables
    public float smoothing = 0.1f;
    float _smoothingVelocity;
    float _gravity = -9.81f;
    [SerializeField] float gravityMultiplier = 3f;
    float _velocity;

    // Projectile variables
    public float bulletSpeed = 50f;
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public GameObject firePoint;
    public GameObject lightningPoint;

    // Sound effect variables
    private AudioSource audioSource;
    public AudioClip audioLightningStart;
    public AudioClip audioLightningLoop;
    public AudioClip audioLightningEnd;
    public AudioClip audioPotionPickUp;
    public AudioClip audioDeath;
    public AudioClip audioHit;
    public AudioClip audioFireball;

    // UI variables
    public GameObject textMesh;
    private TextMeshProUGUI goStartText;

    // Spell variables
    public string CurrentSpell { get; set; } = "lightning";
    private bool isFiring = false;
    public float currentSpellCost = 10f;
    private GameObject lightning;
    public GameObject lightningPrefab;

    // Damage variables
    private bool takingDamage = false;

    // Player state enum
    private enum PlayerState
    {
        starting, idle, walking, running, casting, dying, dead
    }
    private PlayerState _playerState = PlayerState.starting;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Store previous position for animation purposes
        Vector3 _previousPosition = characterController.transform.position;

        switch (_playerState)
        {
            case PlayerState.starting:
                // Show start text and wait for fire button press
                if (goStartText == null)
                {
                    goStartText = textMesh.GetComponent<TextMeshProUGUI>();
                    goStartText.text = "Press fire to start";
                    goStartText.enabled = true;
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    _playerState = PlayerState.idle;
                    goStartText.enabled = false;
                    goStartText = null;
                }
                break;

            case PlayerState.dying:
                // Handle dying state
                break;

            default:
                // Apply gravity, movement, animation, and check for spell casting
                ApplyGravity();
                ApplyMovement();
                ApplyAnimation();
                CheckSpell();
                CheckCast();

                // Check if player is taking damage
                if (takingDamage)
                    GameController.Instance.TakeDamage(10f * Time.deltaTime);
                break;
        }
    }

    // Apply gravity to the character controller
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

    // Apply movement to the character controller
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

    // Apply animation based on character movement
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

    // Check for spell selection
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

    // Check for spell casting
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
                    animator.SetBool("IsLighting", true);
                }
            }
            else
            {
                if (Input.GetButtonUp("Fire1"))
                {
                    isFiring = false;
                    animator.SetBool("IsLighting", false);
                    PlayLightningEnd();
                    Destroy(lightning);
                    lightning = null;
                }
                else if (lightning is not null && !audioSource.isPlaying)
                {
                    PlayLightningLoop();
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
                    audioSource.clip = audioFireball;
                    audioSource.loop = false;
                    audioSource.Play();

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

    // Handle trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag.ToLower())
        {
            case "enemy":
                takingDamage = true;
                audioSource.clip = audioHit;
                audioSource.loop = false;
                audioSource.Play();
                break;

            case "greenkey":
                GameController.Instance.PickUpKey("green");
                Destroy(other.gameObject);
                break;

            case "healthpotion":
                HealthPotionController healthPotionController = other.gameObject.GetComponent<HealthPotionController>();
                if (GameController.Instance.health < GameController.Instance.maxHealth)
                {
                    audioSource.clip = audioPotionPickUp;
                    audioSource.loop = false;
                    audioSource.Play();
                    GameController.Instance.RestoreHealth(healthPotionController.healthBoost);
                    Destroy(other.gameObject);
                }
                break;

            case "magikapotion":
                MagikaPotionController magikaPotionController = other.gameObject.GetComponent<MagikaPotionController>();
                if (GameController.Instance.magika < GameController.Instance.maxMagika)
                {
                    audioSource.clip = audioPotionPickUp;
                    audioSource.loop = false;
                    audioSource.Play();
                    GameController.Instance.RestoreMagika(magikaPotionController.magikaBoost);
                    Destroy(other.gameObject);
                }
                break;
        }
    }

    // Handle trigger exit collisions
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            takingDamage = false;
        }
    }

    // Show lightning effect
    public void ShowLightning()
    {
        if (isFiring)
        {
            lightning = GameObject.Instantiate(lightningPrefab);
            lightning.transform.SetParent(lightningPoint.transform, false);
            PlayLightningStart();
        }
    }

    // Play lightning start sound effect
    private void PlayLightningStart()
    {
        audioSource.clip = audioLightningStart;
        audioSource.loop = false;
        audioSource.Play();
    }

    // Play lightning end sound effect
    private void PlayLightningEnd()
    {
        audioSource.clip = audioLightningEnd;
        audioSource.loop = false;
        audioSource.Play();
    }

    // Play lightning loop sound effect
    private void PlayLightningLoop()
    {
        audioSource.clip = audioLightningLoop;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Handle player death
    public void Death()
    {
        _playerState = PlayerState.dying;
        if (lightning)
        {
            isFiring = false;
            Destroy(lightning);
        }
        animator.SetBool("isDead", true);
    }

    // Handle witch death
    public void WitchDeath()
    {
        animator.SetBool("isLaying", true);
        audioSource.clip = audioDeath;
        audioSource.loop = false;
        audioSource.Play();
    }

    // Handle game over
    public void GameOver()
    {
        ResetPlayer();
        _playerState = PlayerState.starting;
    }

    // Reset player state and position
    private void ResetPlayer()
    {
        animator.SetBool("isLaying", false);
        audioSource.Stop();
        isFiring = false;
        GameController.Instance.health = 100;
        GameController.Instance.magika = 100;
        transform.position = playerStart.transform.position;
        GameController.Instance.keysHeld.Clear();
        animator.SetBool("isDead", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("IsLightning", false);
        animator.SetBool("isIdle", true);
    }
}
