using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    /// <summary>
    /// The slider representing the current health of the enemy.
    /// </summary>
    public Slider healthSlider;

    /// <summary>
    /// The slider used for smooth transitioning of health values.
    /// </summary>
    public Slider easeHealthSlider;

    /// <summary>
    /// The maximum health value for the enemy.
    /// </summary>
    public float maxHealth = 100f;

    /// <summary>
    /// The current health of the enemy.
    /// </summary>
    public float health;

    private float lerpSpeed = 0.05f;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCamera.transform.rotation;

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }

    /// <summary>
    /// Inflicts damage to the enemy's health.
    /// </summary>
    /// <param name="damage">The amount of damage to be inflicted.</param>
    public void takeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
    }
}
