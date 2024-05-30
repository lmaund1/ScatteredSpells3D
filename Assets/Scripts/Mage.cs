using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    private GameController gameController;

    // Controls all animation events 
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
    }

    public void ShowLightning()
    {
        gameController.ShowLightning();
    }

    public void WitchDeath()
    {
        gameController.WitchDeath();
    }

    public void GameOver()
    {
        gameController.GameOver();
    }
}
