using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    public GameObject dieParticleEffect;
    public GameObject[] lootModel;
    private int[] dropChancePerLevel = { 10, 20, 30, 40, 50, 60 };
    public float[] dropChancePerLevelPrice = { 100f, 150f, 200f, 250f, 300f };

    private const float detectionRadius = 20f;
    private const float attackradius = 2f;
    private const float attackCoolDown = 1f;
    private const int zombieDamage = 10;

    private Transform player;
    private PlayerHealth playerHealth;
    private NavMeshAgent zombieMeshAgent;
    private Animator zombieAnimation;
    private BoxCollider zombieCollider;

    private float barScore = 40f;
    private int scoreNow = 0;

    private float attackInterval = 1f;
    private float moveCooldown = -1f;
    private float timeToDestroy = 3.5f;
    private float zombieHealth = 9f;
    private bool scoreAndLootFlag = false;
    private float skillCD;

    public GameObject FloatingTextPrefab;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerHealth = player.GetComponent<PlayerHealth>();
        zombieMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimation = GetComponent<Animator>();
        zombieCollider = GetComponent<BoxCollider>();

        dieParticleEffect.SetActive(false);
    }

    private void Update()
    {
        attackInterval -= (attackInterval > 0) ? Time.deltaTime : 0;

        float zombiePlayerDistance = Vector3.Distance(transform.position, player.position);
        if(skillCD > 0)
        {

        }
        else if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
        else if (zombieHealth < 0f)
        {
            zombieCollider.enabled = false; 
            setAttackMovingDead(false, false, true);

            timeToDestroy -= Time.deltaTime;
            if (timeToDestroy < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f * Time.deltaTime, transform.position.z);
                zombieMeshAgent.enabled = false;
            }
            else
                zombieMeshAgent.SetDestination(transform.position);

        }
        else if (zombiePlayerDistance <= attackradius)
        {
            setAttackMovingDead(true, false, false);

            attackPlayer();
        }
        else if (zombiePlayerDistance <= detectionRadius)
        {
            setAttackMovingDead(false, true, false);
           
            zombieMeshAgent.SetDestination(player.position);
            
        }
        else
        {
            setAttackMovingDead(false, false, false);

            setWalkAround();
        }
        skillCD -= Time.deltaTime;
        if (skillCD < 0)
        {
            zombieMeshAgent.speed = 0.5f;
        }
        else
        {
            zombieMeshAgent.speed = 0.1f;
        }
    }

    public void showFloatingText()
    {
        Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
    }

    public void slowSpeed()
    {
        zombieMeshAgent.SetDestination(transform.position);
        skillCD = 10f;
    }
    public void decreaseHealth(int amount)
    {
        zombieHealth -= amount;

        if (zombieHealth < 0f && !scoreAndLootFlag)
        {
            dieParticleEffect.SetActive(true);
            dropLoot();
            if (FloatingTextPrefab)
                showFloatingText();

            ComboBar.comboCurrentValue += barScore;
            scoreNow = (int) (ZombieApocalypse.GameData.currentMultiplier * 10);

            ZombieApocalypse.GameData.coinCounter += scoreNow;
            ZombieApocalypse.GameData.gameScore += scoreNow;
            scoreAndLootFlag = true;
        }
    }
    private void setWalkAround()
    {
        if (moveCooldown < 0f)
        {
            float randX = Random.Range(-100f, 100f);
            float randZ = Random.Range(-100f, 100f);

            zombieMeshAgent.SetDestination(new Vector3(randX + transform.position.x, transform.position.y, randZ + transform.position.z));

            moveCooldown = Random.Range(10f, 15f);
        }
        else
            moveCooldown -= Time.deltaTime;
    }
    private void attackPlayer()
    {
        if (attackInterval < 0)
        {
            playerHealth.damagePlayer(zombieDamage);
            attackInterval = attackCoolDown;
        }
    }
    private void dropLoot()
    {
        // Test Instantiate Ice
        //GameObject loot = Instantiate(lootModel[1],
        //                                    transform.position + new Vector3(1.0f, 1.0f, 0.0f),
        //                                    Quaternion.identity);
        int random = Random.Range(1, 101);
        if (random <= dropChancePerLevel[ZombieApocalypse.GameShopInfo.item_drop_chance_level])
        {
            GameObject loot = Instantiate(lootModel[Random.Range(0, lootModel.Length)],
                                            transform.position + new Vector3(1.0f, 1.0f, 0.0f),
                                            Quaternion.identity);
            loot.SetActive(true);
            Destroy(loot, 15f);
        }
    }
    private void setAttackMovingDead(bool a, bool b, bool c)
    {
        zombieAnimation.SetBool("isAttack", a);
        zombieAnimation.SetBool("isMoving", b);
        zombieAnimation.SetBool("isDead", c);
    }
}
