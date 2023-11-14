using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieMovement : MonoBehaviour
{
    private NavMeshAgent zombie;
    private Animator animator;
    private Transform player;
    public float detectionRadius = 20f;
    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            zombie.SetDestination(player.position);
            animator.SetBool("isMoving", true);
        }else
        {

            animator.SetBool("isMoving", false);
        }

    }
}
