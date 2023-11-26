using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static int score = 0;

    void Update()
    {
        UpdateScoreText();
    }

    // Call this method when the player kills a zombie to increase the score
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString("D10");
    }
}
