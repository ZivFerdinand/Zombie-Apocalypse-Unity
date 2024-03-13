using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        UpdateScoreText();
    }

    /// <summary>
    /// Update score based on current combo multiplier.
    /// </summary>
    private void UpdateScoreText()
    {
        scoreText.text = ZombieApocalypse.GameData.gameScore.ToString();
    }
}
