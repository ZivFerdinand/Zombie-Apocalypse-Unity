using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coin;

    void Update()
    {
        coin.text = ZombieApocalypse.GameData.coinCounter.ToString();
    }
}
