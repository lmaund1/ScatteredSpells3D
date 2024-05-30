using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Singleton instance of the GameController
    public static GameController Instance { get; private set; }

    // UI elements
    public Image greenKey; // Image for the green key
    public GameObject greenKeyUI; // UI element for displaying the green key
    public GameObject magikaBar; // UI element for the magika bar
    public GameObject healthBar; // UI element for the health bar

    // Required game objects for spells
    public GameObject player; // Player game object
    public GameObject firePoint; // Fire point game object

    // List to store keys held by the player
    public List<string> keysHeld = new();

    // Health variables
    public float health; // Current health
    public float maxHealth; // Maximum health

    // Magika variables
    public float currentSpellCost = 10f; // Cost of the current spell
    public float magika; // Current magika
    public float maxMagika = 0f; // Maximum magika
    public float lastFire = 0f; // Time of the last fire

    // References to other scripts
    private HealthBarController healthBarController; // Reference to the HealthBarController script
    private MagikaBarController magikaBarController; // Reference to the MagikaBarController script
    private ThirdPersonMovement thirdPersonMovement; // Reference to the ThirdPersonMovement script

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Get references to other scripts
            thirdPersonMovement = player.GetComponent<ThirdPersonMovement>();

            // Check if magikaBar is not null before getting references to other scripts
            if (magikaBar != null)
            {
                healthBarController = healthBar.GetComponent<HealthBarController>();
                magikaBarController = magikaBar.GetComponent<MagikaBarController>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Method to pick up a key
    public void PickUpKey(string key)
    {
        switch (key)
        {
            case "green":
                if (!keysHeld.Contains(key))
                {
                    keysHeld.Add(key);
                    Image keySlot = greenKeyUI.GetComponent<Image>();
                    keySlot.color = new Color(1f, 1f, 1f, 1f);
                    keySlot.sprite = greenKey.sprite;
                }
                break;
        }
    }

    // Method to get a string representation of the keys held by the player
    public string Keys()
    {
        string keys = string.Empty;
        foreach (string keyCarried in keysHeld)
        {
            if (keys.Length > 1)
            {
                keys += ", ";
            }
            keys += keyCarried;
        }
        return keys;
    }

    // Method to take damage
    public void TakeDamage(float damage)
    {
        if (health != 0)
        {
            health -= damage;
            if (health < 0)
            {
                health = 0;
                thirdPersonMovement.Death();
            }
        }
    }

    // Method to restore health
    public void RestoreHealth(float healthBoost)
    {
        health += healthBoost;
        if (health >= healthBarController.maxHealth)
        {
            health = healthBarController.maxHealth;
        }
    }

    // Method to use magika
    public void UseMagika(float usage)
    {
        magika -= usage;
        if (magika < 0)
        {
            magika = 0;
        }
    }

    // Method to restore magika
    public void RestoreMagika(float magikaBoost)
    {
        magika += magikaBoost;
        if (magika >= magikaBarController.maxMagika)
        {
            magika = magikaBarController.maxMagika;
        }
    }

    // Method to show lightning effect
    public void ShowLightning()
    {
        thirdPersonMovement.ShowLightning();
    }

    // Method to handle witch death
    public void WitchDeath()
    {
        thirdPersonMovement.WitchDeath();
    }

    // Method to handle game over
    public void GameOver()
    {
        thirdPersonMovement.GameOver();
    }
}
