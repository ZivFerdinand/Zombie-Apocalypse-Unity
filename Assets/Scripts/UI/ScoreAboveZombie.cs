using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    public TextMeshProUGUI aboveZombieText;
    void Update()
    {
        UpdateScore();
    }

    /// <summary>
    /// Score string updating.
    /// </summary>
    private void UpdateScore()
    {
        if (ComboBar.divider == 1f)
        {
            aboveZombieText.text = "+10";
        }
        else if (ComboBar.divider == 1.5f)
        {
            aboveZombieText.text = "+15";
        }
        else if (ComboBar.divider == 2f)
        {
            aboveZombieText.text = "+20";
        }
    }
}
