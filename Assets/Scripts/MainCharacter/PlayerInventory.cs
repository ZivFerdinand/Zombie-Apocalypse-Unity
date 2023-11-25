using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public UnityEvent<PlayerInventory> OnPotionCollected;

    public void PotionCollected()
    {
        OnPotionCollected.Invoke(this);
    }
}
