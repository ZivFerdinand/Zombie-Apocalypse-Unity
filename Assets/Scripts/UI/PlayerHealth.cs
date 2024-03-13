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

    [Header("Healing Overlay")]
    public Image healOverlay;
    public float pulseSpeed = 5f;
    public TextMeshProUGUI healthStatus;
    public TextMeshProUGUI healingTime;
    public GameObject healingStatus;
    public Slider healingSlider;
    private float durationTimer;

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
            healingSlider.value = healCoolDown / 7f;
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

    /// <summary>
    /// Starting heal process.
    /// </summary>
    public void startHeal()
    {
        healingStatus.SetActive(true);
        healCoolDown = 7f;
    }
    /// <summary>
    /// Add damage to player.
    /// </summary>
    /// <param name="damage">Damage value</param>
    public void damagePlayer(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        durationTimer = 0;
        setOverlayColor(damageOverlay, 0.55f);
    }

    /// <summary>
    /// Heal player with value
    /// </summary>
    /// <param name="heal">Heal value</param>
    private void healPlayer(float heal)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + heal);
        setOverlayColor(healOverlay, 0.55f);
    }

    /// <summary>
    /// Overlay effect on heal/damage.
    /// </summary>
    /// <param name="overlay">Image</param>
    private void updateOverlayAlpha(Image overlay)
    {
        float tempAlpha = overlay.color.a + Time.deltaTime * updtDmg;
        setOverlayColor(overlay, Mathf.Clamp01(tempAlpha));

        if (tempAlpha < 0 || tempAlpha > 1)
        {
            updtDmg = -updtDmg;
        }
    }
    /// <summary>
    /// Fading effect overlay.
    /// </summary>
    /// <param name="overlay">Image</param>
    private void fadeOverlay(Image overlay)
    {
        float tempAlpha = overlay.color.a - Time.deltaTime * fadeSpeed;
        setOverlayColor(damageOverlay, Mathf.Clamp01(tempAlpha));
    }

    /// <summary>
    /// Overlay coloring settings.
    /// </summary>
    /// <param name="overlay">Image</param>
    /// <param name="alpha">Alpha value</param>
    private void setOverlayColor(Image overlay, float alpha)
    {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, alpha);
    }

    /// <summary>
    /// Resetting overlay effect.
    /// </summary>
    /// <param name="overlay">Image</param>
    private void resetOverlay(Image overlay)
    {
        setOverlayColor(overlay, 0);
    }

    /// <summary>
    /// Pulsing effect.
    /// </summary>
    /// <param name="overlay">Image</param>
    void PulsateOverlay(Image overlay)
    {
        overlay.gameObject.SetActive(true);
        float alpha = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f;
        setOverlayColor(overlay, alpha);
    }
}