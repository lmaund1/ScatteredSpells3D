using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public Image greenKey;
    public GameObject greenKeyUI;
    public GameObject magikaBar;
    public GameObject healthBar;

    // required for spells
    public GameObject player;
    public GameObject firePoint;
    
    public List<string> keysHeld = new();

    public float health;
    public float maxHealth;
    
    public float currentSpellCost = 10f;
    public float magika;
    public float maxMagika = 0f;
    public float lastFire = 0f;

    private HealthBarController healthBarController;
    private MagikaBarController magikaBarController;
    private ThirdPersonMovement thirdPersonMovement;

    private void Awake()
    {


        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            thirdPersonMovement = player.GetComponent<ThirdPersonMovement>();
            if(magikaBar != null)
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
        
    }

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
    
    public string Keys()
    {
        string keys = string.Empty;
        foreach(string keyCarried in keysHeld)
        {
            if(keys.Length > 1)
            {
                keys += ", ";
            }
            keys += keyCarried;
        }
        return keys;
    }

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

    public void RestoreHealth(float healthBoost)
    {
        health += healthBoost;
        if(health >= healthBarController.maxHealth)
        {
            health = healthBarController.maxHealth;
        }
    }

    public void UseMagika(float usage)
    {
        magika -= usage;
        if(magika < 0)
        {
            magika = 0;
        }
    }

    public void RestoreMagika(float magikaBoost)
    {
        magika += magikaBoost;
        if(magika >= magikaBarController.maxMagika)
        {
            magika = magikaBarController.maxMagika;
        }
    }

    public void ShowLightning()
    {
        thirdPersonMovement.ShowLightning();
        
    }

    public void WitchDeath()
    {
        thirdPersonMovement.WitchDeath();
    }

    public void GameOver()
    {
        thirdPersonMovement.GameOver();
    }
}
