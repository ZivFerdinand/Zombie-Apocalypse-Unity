using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider comboBar;
    public static float comboCurrentValue = 0;
    public static float divider = 1f;
    private float overBarValue = 0f;

    private void Update()
    {
        if (comboBar.value + comboCurrentValue/divider > 100)
            overBarValue = (comboBar.value + comboCurrentValue/divider) - 100;

        comboBar.value += comboCurrentValue / divider;
        comboCurrentValue = 0;

        if(comboBar.value >= 100 && divider < 2f)
        {
            comboBar.value = overBarValue;
            overBarValue = 0;
            divider += 0.5f;
        } 
        else if(comboBar.value <= 1 && divider > 1f)
        {
            comboBar.value = 99;
            divider -= 0.5f;
        }

        comboBar.value -= Time.deltaTime * (1.25f * divider);
    }
}
