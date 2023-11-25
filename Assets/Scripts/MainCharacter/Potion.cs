using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public AudioSource source;

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(-60f, Time.time * 100f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.PotionCollected();
            source.Play();
            StartCoroutine(SelfDestruct());
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
        Destroy(source);
    }
}
