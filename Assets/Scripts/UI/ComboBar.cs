using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider comboBar;
    public static float comboCurrentValue = 0;
    private void Update()
    {
        comboBar.value += comboCurrentValue;
        comboCurrentValue = 0;

        comboBar.value -= Time.deltaTime * 20f;
    }
}
