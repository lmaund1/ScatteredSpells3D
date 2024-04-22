using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    private float lerpSpeed = 0.05f;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        gameController.health = maxHealth;
        gameController.maxHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value != gameController.health)
        {
            healthSlider.value = gameController.health;
        }

        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, gameController.health, lerpSpeed);
        }
    }
}
