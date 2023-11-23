using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Bar")]
    public float currentHealth;
    [HideInInspector] public int maxHealth = 100;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;
    private float dmgSpeed = 1f;
    private float updtDmg;

    public TextMeshProUGUI healthStatus;
    private float durationTimer;

    void Start()
    {
        currentHealth = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        updtDmg = fadeSpeed;
    }

    void Update()
    {
        healthStatus.text = ((int) currentHealth).ToString();
        healPlayer(Time.deltaTime * 0.5f);

        if(currentHealth < 30)
        {
            float tempAlpha = overlay.color.a;
            tempAlpha += Time.deltaTime * updtDmg;
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);

            if (overlay.color.a < 0)
            {
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
                updtDmg = dmgSpeed;
            }
            else if (overlay.color.a > 1)
            {
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
                updtDmg = -dmgSpeed;
            }
        }
        if (overlay.color.a > 0 && currentHealth >= 30)
        {
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

    public void damagePlayer(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);


        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.55f);
    }
    public void healPlayer(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Min(100, currentHealth);
    }
}