using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagikaBarController : MonoBehaviour
{
    public Slider magikaSlider;
    public Slider easeMagikaSlider;
    public float maxMagika = 100f;
    
    private float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.magika = maxMagika;
        GameController.Instance.maxMagika = maxMagika;
    }

    // Update is called once per frame
    void Update()
    {
        if (magikaSlider.value != GameController.Instance.magika)
        {
            magikaSlider.value = GameController.Instance.magika;
        }

        if (magikaSlider.value != easeMagikaSlider.value)
        {
            easeMagikaSlider.value = Mathf.Lerp(easeMagikaSlider.value, GameController.Instance.magika, lerpSpeed);
        }
    }

  
}
