using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieDemo : MonoBehaviour
{
    private const float travelPositionLimit = 20;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();


        animator.SetBool("isMoving", true);

        setDestination();
    }

    private void Update()
    {
        if(transform.position.x >= travelPositionLimit)
            resetPosition();
        
    }

    /// <summary>
    /// This function resets the position of the zombie to the starting point again.
    /// </summary>
    private void resetPosition()
    {
        navMeshAgent.speed = Random.Range(1f, 3f);
        transform.position = new Vector3(-travelPositionLimit, transform.position.y, transform.position.z);

        setDestination();
    }

    /// <summary>
    /// This function sets the direction of the zombie's next movement after the reset.
    /// </summary>
    private void setDestination()
    { 
        navMeshAgent.SetDestination(new Vector3(travelPositionLimit + 1f, transform.position.y, transform.position.z));
    }
}
