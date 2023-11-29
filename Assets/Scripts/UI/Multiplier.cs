using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    public TextMeshProUGUI multiplierText;

    // Update is called once per frame
    void Update()
    {
        UpdateMultiplier();
    }

    private void UpdateMultiplier()
    {
        if(ComboBar.divider == 1f)
        {
            multiplierText.text = "x 1";
        }
        else if(ComboBar.divider == 1.5f)
        {
            multiplierText.text = "x 2";
        }
        else if(ComboBar.divider == 2f)
        {
            multiplierText.text = "x 4";
        }
    }
}
