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
    public float attackradius = 2f;
  
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
        
        if (Vector3.Distance(transform.position, player.position) <= attackradius)
        {
            animator.SetBool("isAttack", true);
            animator.SetBool("isMoving", false);
            //Masukin damage player ke sini
        }
        else if(Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            zombie.SetDestination(player.position);
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttack", false);
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttack", false);
        }

    }
}
