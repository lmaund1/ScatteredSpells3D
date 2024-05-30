using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagikaBarController : MonoBehaviour
{
    public Slider magikaSlider; // Reference to the slider for displaying magika
    public Slider easeMagikaSlider; // Reference to the slider for easing the magika value
    public float maxMagika = 100f; // Maximum magika value
    
    private float lerpSpeed = 0.05f; // Speed at which the easeMagikaSlider value changes

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial magika values in the GameController instance
        GameController.Instance.magika = maxMagika;
        GameController.Instance.maxMagika = maxMagika;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the magikaSlider value if it is different from the GameController instance value
        if (magikaSlider.value != GameController.Instance.magika)
        {
            magikaSlider.value = GameController.Instance.magika;
        }

        // Update the easeMagikaSlider value using a lerp function to smoothly transition to the GameController instance value
        if (magikaSlider.value != easeMagikaSlider.value)
        {
            easeMagikaSlider.value = Mathf.Lerp(easeMagikaSlider.value, GameController.Instance.magika, lerpSpeed);
        }
    }
}
