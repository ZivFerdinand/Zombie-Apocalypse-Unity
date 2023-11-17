using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject zombiePrefab;
    private List<GameObject> checkpoints;
    private int touchedIndex;
    private int lastTouchedIndex;
    // Start is called before the first frame update
    void Start()
    {
        checkpoints = new List<GameObject>();
        for(int i=0;i<transform.childCount;i++)
        {
            checkpoints.Add(transform.GetChild(i).GetComponent<Transform>().gameObject);
        }
        touchedIndex =lastTouchedIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(touchedIndex > 0 && lastTouchedIndex != touchedIndex)
        {
            if (touchedIndex >= checkpoints.Count) { touchedIndex = 0; }
            checkpoints[touchedIndex].GetComponent<ChildCheckpoints>().spawnZombies();
            //Instantiate(zombiePrefab, checkpoints[touchedIndex].GetComponent<GameObject>().transform.position, Quaternion.identity);
            lastTouchedIndex = touchedIndex;
            touchedIndex = -1;
        }
    }

    public void setTouched(string name)
    {
        touchedIndex = int.Parse(name);
    }
}
