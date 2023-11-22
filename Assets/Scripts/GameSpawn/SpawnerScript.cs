using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] zombiePrefabs;
    
    public Transform zombieClone;

    private List<GameObject> checkpoints;
    private int touchedIndex;
    private int lastTouchedIndex;

    void Start()
    {
        checkpoints = new List<GameObject>();
        zombiePrefabs = Resources.LoadAll<GameObject>("Zombies");
        touchedIndex = lastTouchedIndex = -1;

        for (int i = 0; i < transform.childCount; i++)
            checkpoints.Add(transform.GetChild(i).GetComponent<Transform>().gameObject);
    }

    void Update()
    {
        if (touchedIndex > 0 && lastTouchedIndex != touchedIndex)
        {
            if (touchedIndex >= checkpoints.Count) { touchedIndex = 0; }

            checkpoints[touchedIndex].GetComponent<ChildCheckpoints>().spawnZombies(zombiePrefabs, zombieClone);

            int reflect = touchedIndex - 2;
            if (reflect < 0) reflect += checkpoints.Count - 1;
            checkpoints[reflect].GetComponent<ChildCheckpoints>().spawnZombies(zombiePrefabs, zombieClone);
            lastTouchedIndex = touchedIndex;

            touchedIndex = -1;
        }
    }

    public void setTouched(string name)
    {
        touchedIndex = int.Parse(name);
    }
}
