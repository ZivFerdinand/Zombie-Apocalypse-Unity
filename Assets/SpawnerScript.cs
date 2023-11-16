using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject zombiePrefab;
    private List<GameObject> checkpoints;
    private int touchedIndex;
    // Start is called before the first frame update
    void Start()
    {
        checkpoints = new List<GameObject>();
        for(int i=0;i<transform.childCount;i++)
        {
            checkpoints.Add(transform.GetChild(i).gameObject);
        }
        touchedIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(touchedIndex);
        if(touchedIndex > 0)
        {
            Debug.Log(checkpoints.Count);
            //checkpoints[touchedIndex].GetComponent<ChildCheckpoints>().spawnZombies();
            Instantiate(zombiePrefab, checkpoints[touchedIndex].GetComponent<GameObject>().transform.position, Quaternion.identity);
            touchedIndex = -1;
        }
    }

    public void setTouched(string name)
    {
        touchedIndex = int.Parse(name);
    }
}
