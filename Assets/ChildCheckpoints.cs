using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCheckpoints : MonoBehaviour
{
    public GameObject zombiePrefab;
    // Start is called before the first frame update
    void Start()
    {
        zombiePrefab = GetComponentInParent<SpawnerScript>().zombiePrefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnZombies()
    {
        Instantiate(zombiePrefab, transform.position, Quaternion.identity); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Test");
            GetComponentInParent<SpawnerScript>().setTouched(gameObject.name);
        }
    }
}
