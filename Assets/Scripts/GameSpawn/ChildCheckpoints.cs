using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCheckpoints : MonoBehaviour
{
    private int difficulty;
    private int spawnerCount;
    
    private void Start()
    {
        difficulty = ZombieApocalypse.GameStatus.gameMode;

        spawnerCount = (difficulty == 0) ? 10 : 20;
    }

    /// <summary>
    /// This function creates the zombies inside the map.
    /// </summary>
    /// <param name="zombiePrefabs">Instance of the zombie model.</param>
    /// <param name="parent">The position of the spawner.</param>
    public void spawnZombies(GameObject[] zombiePrefabs, Transform parent)
    {
        for (int i = 0; i < spawnerCount; i++)
        {
            Instantiate(zombiePrefabs[2], transform.position, Quaternion.identity).transform.SetParent(parent);
        }
    }

    /// <summary>
    /// This function detects if the player have entered the zombie spawn point.
    /// </summary>
    /// <param name="other">The object's collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<SpawnerScript>().setTouched(gameObject.name);
        }
    }
}
