using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour
{
    public Transform projectileSpawnPoint; // The spawn point of the projectile
    public GameObject projectilePrefab; // The prefab of the projectile
    public float projectileSpeed = 10f; // The speed at which the projectile moves
    private GameController _gameController; // Reference to the game controller

    public void Awake()
    {
        // This method is called when the script instance is being loaded
        // You can initialize any variables or perform setup tasks here
    }

    // Start is called before the first frame update
    void Start()
    {
        // This method is called before the first frame update
        // You can initialize any variables or perform setup tasks here
        _gameController = GameController.Instance; // Get a reference to the game controller
    }

    // Update is called once per frame
    void Update()
    {
        // This method is called once per frame
        // You can update game logic or respond to user input here

        // Check if enough time has passed since the last fire and if there is enough magika to cast the spell
        if (_gameController.lastFire <= 0 && _gameController.magika > _gameController.currentSpellCost)
        {
            // Check if the Return key is pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Instantiate a new projectile at the spawn point position and rotation
                var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                // Set the velocity of the projectile to move in the forward direction of the spawn point with the specified speed
                projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
            }
        }
    }
}
