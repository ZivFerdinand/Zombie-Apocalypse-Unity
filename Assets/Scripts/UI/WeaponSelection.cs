using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    public Transform automaticRifleIcon;
    public Transform sniperIcon;
    public TextMeshProUGUI weaponNameText;

    public float scaleSpeed = 5f; // Adjust the speed of scaling

    private Vector3 originalScale;

    void Start()
    {
        // Store the original scale of the icons
        originalScale = automaticRifleIcon.localScale;
    }

    public void SelectAutomaticRifle()
    {
        ScaleUpIconSmoothly(automaticRifleIcon);
        SetWeaponName("Automatic Rifle");
    }

    public void SelectSniper()
    {
        ScaleUpIconSmoothly(sniperIcon);
        SetWeaponName("Sniper");
    }

    void ScaleUpIconSmoothly(Transform icon)
    {
        // Reset the scale of both icons
        ResetIconScale(automaticRifleIcon);
        ResetIconScale(sniperIcon);

        // Scale up the selected icon smoothly using Lerp
        StartCoroutine(ScaleOverTime(icon, originalScale, new Vector3(1.2f, 1.2f, 1.2f), scaleSpeed));
    }

    void ResetIconScale(Transform icon)
    {
        // Reset the scale of the icons
        icon.localScale = originalScale;
    }

    IEnumerator ScaleOverTime(Transform objectToScale, Vector3 startScale, Vector3 endScale, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            objectToScale.localScale = Vector3.Lerp(startScale, endScale, (Time.time - startTime) / duration);
            yield return null;
        }
        objectToScale.localScale = endScale;
    }

    void SetWeaponName(string weaponName)
    {
        weaponNameText.text = weaponName;
    }
}
