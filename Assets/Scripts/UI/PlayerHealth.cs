using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Bar")]
    public float currentHealth;
    private int maxHealth = 100;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed = 1f;
    private float healCoolDown;
    private float updtDmg;

    public TextMeshProUGUI healthStatus;
    public TextMeshProUGUI healingTime;
    public GameObject healingStatus;
    private float durationTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        resetOverlay();
        updtDmg = fadeSpeed;
    }

    private void Update()
    {
        healthStatus.text = ((int)currentHealth).ToString();
        if (currentHealth < 20)
        {
            healPlayer(Time.deltaTime * 0.5f);
        }
        if(healCoolDown > 0)
        {
            healingTime.text = healCoolDown.ToString("F1") + "s";
            healCoolDown -= Time.deltaTime;
            healPlayer(Time.deltaTime);
        }
        else
        {
            healingStatus.SetActive(false);
        }
        if (currentHealth < 30)
        {
            updateOverlayAlpha();
        }
        else if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                fadeOverlay();
            }
        }
    }
    public void startHeal()
    {
        healingStatus.SetActive(true);
        healCoolDown = 7f;
    }
    public void damagePlayer(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        durationTimer = 0;
        setOverlayColor(0.55f);
    }

    private void healPlayer(float heal)
    {
        // SHAM, SELAMA PLAYER SEDANG HEAL, TOLONG BUAT IJO IJO NYUT NYUT DI LAYAR YE
        currentHealth = Mathf.Min(maxHealth, currentHealth + heal);
    }

    private void updateOverlayAlpha()
    {
        float tempAlpha = overlay.color.a + Time.deltaTime * updtDmg;
        setOverlayColor(Mathf.Clamp01(tempAlpha));

        if (tempAlpha < 0 || tempAlpha > 1)
        {
            updtDmg = -updtDmg;
        }
    }

    private void fadeOverlay()
    {
        float tempAlpha = overlay.color.a - Time.deltaTime * fadeSpeed;
        setOverlayColor(Mathf.Clamp01(tempAlpha));
    }

    private void setOverlayColor(float alpha)
    {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, alpha);
    }

    private void resetOverlay()
    {
        setOverlayColor(0);
    }
}
