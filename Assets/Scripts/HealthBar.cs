using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerHealth playerHealth;
    public int currHp;

    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = currHp = playerHealth.maxHealth;
    }
    public void Update()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, currHp, 5f * Time.deltaTime);
    }
    public void SetHealth(int hp)
    {
        currHp = hp;
    }
}