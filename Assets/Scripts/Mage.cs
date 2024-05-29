using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLightning()
    {
        gameController.ShowLightning();
    }

    public void WitchDeath()
    {
        gameController.WitchDeath();
        Debug.Log("witch death");
    }

    public void GameOver()
    {
        gameController.GameOver();
        Debug.Log("game over");
    }
}
