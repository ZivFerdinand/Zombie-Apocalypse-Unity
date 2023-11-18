using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCheckpoints : MonoBehaviour
{
    public void spawnZombies(GameObject[] zombiePrefabs, Transform parent)
    {
        Instantiate(zombiePrefabs[2], transform.position, Quaternion.identity).transform.SetParent(parent); 

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<SpawnerScript>().setTouched(gameObject.name);
        }
    }
}
