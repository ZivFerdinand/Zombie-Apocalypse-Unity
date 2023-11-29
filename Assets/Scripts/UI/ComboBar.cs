using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider comboBar;
    //private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        //playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        comboBar = GetComponent<Slider>();
        //comboBar.maxValue = playerHealth.maxHealth;
        //comboBar.value = playerHealth.maxHealth;
        comboBar.value = 20f;
    }

    public void SetBar(float score)
    {
        comboBar.value += score;
    }

    // Update is called once per frame
    void Update()
    {
        comboBar.value -= Time.deltaTime * 20f;
    }
}
