using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class ZombieMovement : MonoBehaviour
{
    private const float detectionRadius = 20f;
    private float attackradius = 2f;
    private const float attackCoolDown = 1f;
    private const int zombieDamage = 10;

    public GameObject dieParticleEffect;
    private Transform player;
    private NavMeshAgent zombieMeshAgent;
    private Animator zombieAnimation;

    private float attackInterval = 1f;
    private float moveCD = -1f;
    private float timeToDestroy = 3.5f;
    private float zombieHealth = 2f;

    public PlayerScore playerScoreScript;
    private int scoreFlag = 0;

    public GameObject lootModel;
    public int dropChance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        zombieMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimation = GetComponent<Animator>();
        dieParticleEffect.SetActive(false);
    }

    void Update()
    {
        attackInterval -= (attackInterval > 0) ? Time.deltaTime : 0;
        if (zombieHealth < 0f)
        {
            zombieAnimation.SetBool("isAttack", false);
            zombieAnimation.SetBool("isMoving", false);
            zombieAnimation.SetBool("isDead", true);

            timeToDestroy -= Time.deltaTime;

            dieParticleEffect.SetActive(true);
            dropLoot();

            if (transform.position.y < 0)
            {
                Destroy(this.gameObject);
                scoreFlag = 0;
            }

            if (timeToDestroy < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f * Time.deltaTime, transform.position.z);
                zombieMeshAgent.enabled = false;
            }
            else
                zombieMeshAgent.SetDestination(transform.position);

        }
        else if (Vector3.Distance(transform.position, player.position) <= attackradius)
        {
            zombieAnimation.SetBool("isAttack", true);
            zombieAnimation.SetBool("isMoving", false);
            zombieAnimation.SetBool("isDead", false);

            AttackPlayer();
        }
        else if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            zombieAnimation.SetBool("isMoving", true);
            zombieAnimation.SetBool("isAttack", false);
            zombieAnimation.SetBool("isDead", false);

            zombieMeshAgent.SetDestination(player.position);
        }
        else
        {
            zombieAnimation.SetBool("isMoving", false);
            zombieAnimation.SetBool("isAttack", false);
            zombieAnimation.SetBool("isDead", false);

            if (moveCD < 0f)
            {
                float randX = Random.Range(-100f, 100f);
                float randZ = Random.Range(-100f, 100f);

                zombieMeshAgent.SetDestination(new Vector3(randX + transform.position.x, transform.position.y, randZ + transform.position.z));
                moveCD = Random.Range(10f, 15f);
            }
            else
                moveCD -= Time.deltaTime;
        }

    }
    void AttackPlayer()
    {
        if (attackInterval < 0)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            playerHealth.damagePlayer(zombieDamage);
            attackInterval = attackCoolDown;
        }
    }
    public void decreaseHealth()
    {
        zombieHealth--;
        if (zombieHealth < 0f && scoreFlag == 0)
        {
            PlayerScore.score += 10;
            scoreFlag = 1;
        }
    }

    public void dropLoot()
    {
        Vector3 position = transform.position;
        GameObject loot = Instantiate(lootModel, position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
        loot.SetActive(true);
    }
}
