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

    private void Start()
    {
        checkpoints = new List<GameObject>();
        zombiePrefabs = Resources.LoadAll<GameObject>("Zombies");
        touchedIndex = lastTouchedIndex = -1;

        foreach (Transform childTransform in transform)
        {
            checkpoints.Add(childTransform.gameObject);
        }
    }

    private void Update()
    {
        if (touchedIndex > 0 && lastTouchedIndex != touchedIndex)
        {
            if (touchedIndex >= checkpoints.Count) { touchedIndex = 0; }

            int reflect = touchedIndex - 2;
            reflect += (reflect < 0) ? checkpoints.Count - 1 : 0;

            checkpoints[touchedIndex].GetComponent<ChildCheckpoints>().spawnZombies(zombiePrefabs, zombieClone);
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
