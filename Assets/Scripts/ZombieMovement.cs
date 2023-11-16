using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class ZombieMovement : MonoBehaviour
{
    private NavMeshAgent zombie;
    private Animator animator;
    private Transform player;
    public float detectionRadius = 20f;
    public float attackradius = 2f;
    public const float attackCoolDown = 1f;
    public float attackInterval;
    public int zombieDamage = 10;
    private float moveCD = -1f;
  
    // Start is called before the first frame update
    void Start()
    {
        moveCD = -1f;
        attackInterval = attackCoolDown;
        zombie = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackInterval > 0) { attackInterval -= Time.deltaTime; }
        if (Vector3.Distance(transform.position, player.position) <= attackradius)
        {
            animator.SetBool("isAttack", true);
            animator.SetBool("isMoving", false);

            //Masukin damage player ke sini
            AttackPlayer();
        }
        else if(Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            zombie.SetDestination(player.position);
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttack", false);
        }
        else
        {
            if (moveCD < 0f)
            {
                zombie.SetDestination(new Vector3(Random.Range(-100f, 100f) + transform.position.x, transform.position.y, Random.Range(-100f, 100f) + transform.position.z));
                moveCD = Random.Range(10f, 15f);
            }
            else
            {
                moveCD -= Time.deltaTime;
            }

            animator.SetBool("isMoving", false);
            animator.SetBool("isAttack", false);
        }

    }
    void AttackPlayer()
    {
        if (attackInterval < 0)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            playerHealth.DamagePlayer(zombieDamage);
            attackInterval = attackCoolDown;
        }
    }
}
