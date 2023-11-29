using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public UnityEvent<PlayerInventory> OnPotionCollected;
    public SkillScript playerSkill;
    public PlayerHealth playerHealth;

    public void PotionCollected(string name)
    {
        switch (name)
        {
            case "FirePotion":
                playerSkill.getSkill(0);
                break;
            case "HealPotion":
                playerHealth.startHeal();
                break;
            case "IcePotion":
                playerSkill.getSkill(1);
                break;
        }
        OnPotionCollected.Invoke(this);
    }
}
