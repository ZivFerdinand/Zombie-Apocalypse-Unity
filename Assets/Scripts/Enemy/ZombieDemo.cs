using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieDemo : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 initPosition;
    private float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator.SetBool("isMoving", true);
        setDes();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= 15)
        {
            resetPosition();
        }
    }
    void resetPosition()
    {
        speed = Random.Range(1f, 3f);
        transform.position = new Vector3(-15f, -1.8f, transform.position.z);
        setDes();
    }
    void setDes()
    {
        navMeshAgent.SetDestination(new Vector3(16, transform.position.y, transform.position.z));
    }
}
