using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class ZombieMovement : MonoBehaviour
{
    public GameObject particle;
    private NavMeshAgent zombie;
    private Animator animator;
    private Transform player;
    public float detectionRadius = 20f;
    public float attackradius = 2f;
    public const float attackCoolDown = 1f;
    public float attackInterval;
    public int zombieDamage = 10;
    private float moveCD = -1f;
    private float timeToDestroy = 3.5f;
    private float zombHealth = 2f;
  
    // Start is called before the first frame update
    void Start()
    {
        moveCD = -1f;
        attackInterval = attackCoolDown;
        zombie = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        particle.SetActive(false);
    }
    public void decreaseHealth()
    {
        zombHealth--;
    }
    // Update is called once per frame
    void Update()
    {
        
        if(attackInterval > 0) { attackInterval -= Time.deltaTime; }
        if (zombHealth < 0f)
        {

            animator.SetBool("isAttack", false);
            animator.SetBool("isMoving", false);
            animator.SetBool("isDead", true);
            timeToDestroy-=Time.deltaTime;
            if (particle.activeInHierarchy == false)
                particle.SetActive(true);
            if (timeToDestroy < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 2f * Time.deltaTime, transform.position.z);
                zombie.GetComponent<NavMeshAgent>().enabled = false;
                //zombie.SetDestination(new Vector3(transform.position.x, transform.position.y + 5f * Time.deltaTime, transform.position.z));
            }
            else
            {
                zombie.SetDestination(transform.position);

            }
            if (transform.position.y < 0)
                Destroy(this.gameObject);
        }
        else if (Vector3.Distance(transform.position, player.position) <= attackradius)
        {
            animator.SetBool("isAttack", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isDead", false);

            //Masukin damage player ke sini
            AttackPlayer();
        }
        else if(Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            zombie.SetDestination(player.position);
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttack", false);
            animator.SetBool("isDead", false);
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
            animator.SetBool("isDead", false);
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
