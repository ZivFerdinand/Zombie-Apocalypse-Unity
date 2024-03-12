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
    private void UpdateScoreText()
    {
        scoreText.text = ZombieApocalypse.GameData.gameScore.ToString();
    }
}
