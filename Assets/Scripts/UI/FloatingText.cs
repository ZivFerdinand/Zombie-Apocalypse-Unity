using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FloatingText : MonoBehaviour
{
    private Transform player;
    public float DestroyTime = 3f;
    public Vector3 Offsset = new Vector3 (0, 3, 0);
    public Vector3 RandomizeIntensity = new Vector3((float)0.5, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, DestroyTime);

        transform.localPosition += Offsset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - player.position);
    }
}
