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
    public Image damageOverlay;
    public float duration;
    public float fadeSpeed = 1f;
    private float healCoolDown;
    private float updtDmg;

    public TextMeshProUGUI healthStatus;
    public TextMeshProUGUI healingTime;
    public GameObject healingStatus;
    private float durationTimer;

    public Image healOverlay;
    public float pulseSpeed = 5f;

    private void Start()
    {
        currentHealth = maxHealth;
        resetOverlay(damageOverlay);
        resetOverlay(healOverlay);
        updtDmg = fadeSpeed;
    }

    private void Update()
    {
        healthStatus.text = ((int)currentHealth).ToString();

        if (currentHealth < 20)
        {
            healPlayer(Time.deltaTime * 0.5f);
        }

        if (healCoolDown > 0)
        {
            healingTime.text = healCoolDown.ToString("F1") + "s";
            healCoolDown -= Time.deltaTime;
            healPlayer(Time.deltaTime);
        }
        else
        {
            healingStatus.SetActive(false);
        }

        if (currentHealth < 20 || healCoolDown > 0)
        {
            PulsateOverlay(healOverlay);
            resetOverlay(damageOverlay);
        }
        else if (currentHealth < 30)
        {
            resetOverlay(healOverlay);
            updateOverlayAlpha(damageOverlay);
        }
        else if (damageOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                fadeOverlay(damageOverlay);
            }
        }
        else
        {
            resetOverlay(healOverlay);
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
        setOverlayColor(damageOverlay, 0.55f);
    }

    private void healPlayer(float heal)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + heal);
        setOverlayColor(healOverlay, 0.55f);
    }

    private void updateOverlayAlpha(Image overlay)
    {
        float tempAlpha = overlay.color.a + Time.deltaTime * updtDmg;
        setOverlayColor(overlay, Mathf.Clamp01(tempAlpha));

        if (tempAlpha < 0 || tempAlpha > 1)
        {
            updtDmg = -updtDmg;
        }
    }

    private void fadeOverlay(Image overlay)
    {
        float tempAlpha = overlay.color.a - Time.deltaTime * fadeSpeed;
        setOverlayColor(damageOverlay, Mathf.Clamp01(tempAlpha));
    }

    private void setOverlayColor(Image overlay, float alpha)
    {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, alpha);
    }

    private void resetOverlay(Image overlay)
    {
        setOverlayColor(overlay, 0);
    }

    void PulsateOverlay(Image overlay)
    {
        overlay.gameObject.SetActive(true);
        float alpha = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f;
        setOverlayColor(overlay, alpha);
    }
}