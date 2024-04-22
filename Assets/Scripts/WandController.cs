using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    private GameController _gameController;

    public void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.lastFire <= 0 && _gameController.magika > _gameController.currentSpellCost)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
            }
        }
    }
}
