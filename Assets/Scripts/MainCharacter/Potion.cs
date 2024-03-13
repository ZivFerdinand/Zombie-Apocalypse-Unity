using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private void Update()
    {
        transform.localRotation = Quaternion.Euler(-60f, Time.time * 100f, 0);
    }
    /// <summary>
    /// This function is triggered when an object with a Collider enters the trigger zone.
    /// It checks if the colliding object has a PlayerInventory component and if its tag is "Player.".
    /// If both conditions are met, it informs the PlayerInventory about the collected potion and destroys the potion object.
    /// </summary>
    /// <param name="other">The Collider of the object entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null && other.gameObject.tag == "Player")
        {
            playerInventory.PotionCollected(gameObject.name);

            Destroy(gameObject);
        }
    }

}
