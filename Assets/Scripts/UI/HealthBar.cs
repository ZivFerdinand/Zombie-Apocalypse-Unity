using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    public PlayerHealth playerHealth;
    public float currHp;

    private void Start()
    {
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = currHp = playerHealth.maxHealth;
    }
    public void Update()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, currHp, 5f * Time.deltaTime);
    }
    public void SetHealth(float hp)
    {
        currHp = hp;
    }
}