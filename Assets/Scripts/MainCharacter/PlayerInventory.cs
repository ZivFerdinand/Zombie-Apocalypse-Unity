using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public UnityEvent<PlayerInventory> OnPotionCollected;
    public SkillScript playerSkill;
    public PlayerHealth playerHealth;
    /// <summary>
    /// This function will tell which potion are taken and carry out the effect of the potion.
    /// </summary>
    /// <param name="name">String of potion name</param>

    public void PotionCollected(string name)
    {
        switch (name)
        {
            case "FirePotion(Clone)":
                playerSkill.getSkill(0);
                break;
            case "HealPotion(Clone)":
                playerHealth.startHeal();
                break;
            case "IcePotion(Clone)":
                playerSkill.getSkill(1);
                break;
        }
        OnPotionCollected.Invoke(this);
    }
}
