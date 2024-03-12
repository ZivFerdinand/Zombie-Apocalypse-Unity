using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    public TextMeshProUGUI multiplierText;

    void Update()
    {
        UpdateMultiplier();
    }

    private void UpdateMultiplier()
    {
        if(ComboBar.divider == 1f)
        {
            multiplierText.text = "x 1";
            ZombieApocalypse.GameData.currentMultiplier = 1;
        }
        else if(ComboBar.divider == 1.5f)
        {
            multiplierText.text = "x 1.5";
            ZombieApocalypse.GameData.currentMultiplier = 1.5f;
        }
        else if(ComboBar.divider == 2f)
        {
            multiplierText.text = "x 2";
            ZombieApocalypse.GameData.currentMultiplier = 2;
        }
    }
}
