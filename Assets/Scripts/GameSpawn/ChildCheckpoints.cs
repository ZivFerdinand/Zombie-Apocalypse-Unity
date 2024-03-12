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
    public void spawnZombies(GameObject[] zombiePrefabs, Transform parent)
    {
        for (int i = 0; i < spawnerCount; i++)
        {
            Instantiate(zombiePrefabs[2], transform.position, Quaternion.identity).transform.SetParent(parent);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<SpawnerScript>().setTouched(gameObject.name);
        }
    }
}
