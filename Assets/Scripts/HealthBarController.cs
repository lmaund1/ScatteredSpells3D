using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider healthSlider; // Reference to the health slider UI element
    public Slider easeHealthSlider; // Reference to the ease health slider UI element
    public float maxHealth = 100f; // Maximum health value
    private float lerpSpeed = 0.05f; // Speed at which the ease health slider value changes

    private GameController gameController; // Reference to the game controller script

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance; // Get the instance of the game controller script
        gameController.health = maxHealth; // Set the initial health value in the game controller
        gameController.maxHealth = maxHealth; // Set the maximum health value in the game controller
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health slider value if it's different from the game controller's health value
        if(healthSlider.value != gameController.health)
        {
            healthSlider.value = gameController.health;
        }

        // Update the ease health slider value using a smooth interpolation if it's different from the game controller's health value
        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, gameController.health, lerpSpeed);
        }
    }
}
