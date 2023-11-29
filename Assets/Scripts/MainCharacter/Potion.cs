using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private void Update()
    {
        transform.localRotation = Quaternion.Euler(-60f, Time.time * 100f, 0);
    }
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
