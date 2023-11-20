using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Bar")]
    public int currentHealth;
    public int maxHealth = 100;
    public HealthBar healthBar;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;
    private float dmgSpeed = 1f;
    private float updtDmg;

    private float durationTimer;


    void Start()
    {
        currentHealth = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        updtDmg = fadeSpeed;
    }

    void Update()
    {
        if(currentHealth < 30)
        {
            float tempAlpha = overlay.color.a;
            tempAlpha += Time.deltaTime * updtDmg;
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);

            if(overlay.color.a > 1 || overlay.color.a < 0)
            {
                if (overlay.color.a < 0)
                {
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
                    updtDmg = dmgSpeed;
                }
                else
                {
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
                    updtDmg = -dmgSpeed;
                }
            }
        }
        if (overlay.color.a > 0)
        {
            if (currentHealth < 30)
            {
                return;
            }
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                // fade image setelah durasi tertentu
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.SetHealth(currentHealth);
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.55f);
    }
    public void HealPlayer(int heal)
    {
        currentHealth += heal;

        if (currentHealth > 100)
        {
            currentHealth = 100;
        }

        healthBar.SetHealth(currentHealth);
    }
}