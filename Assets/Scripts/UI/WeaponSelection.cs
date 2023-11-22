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
    public float weaponNameDisplayDuration = 1.0f; // Duration to display the weapon name

    private Vector3 originalScale;
    private Coroutine weaponNameCoroutine;

    void Start()
    {
        weaponNameText.gameObject.SetActive(false);
        // Store the original scale of the icons
        originalScale = automaticRifleIcon.localScale;
    }

    public void SelectAutomaticRifle()
    {
        Debug.Log("Selecting Automatic Rifle");
        ScaleUpIconSmoothly(automaticRifleIcon);
        SetWeaponName("Automatic Rifle");
    }

    public void SelectSniper()
    {
        Debug.Log("Selecting Sniper");
        ScaleUpIconSmoothly(sniperIcon);
        SetWeaponName("Single-Shot Rifle");
    }

    void ScaleUpIconSmoothly(Transform icon)
    {
        // Reset the scale of both icons
        ResetIconScale(automaticRifleIcon);
        ResetIconScale(sniperIcon);

        // Scale up the selected icon smoothly using Lerp
        StartCoroutine(ScaleOverTime(icon, originalScale, new Vector3(1.2f, 1.2f, 1.2f), scaleSpeed));

        // Show the weapon name temporarily
        ShowWeaponNameTemporarily();
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

    void ShowWeaponNameTemporarily()
    {
        if (weaponNameCoroutine != null)
            StopCoroutine(weaponNameCoroutine);

        weaponNameCoroutine = StartCoroutine(ShowWeaponName());
    }

    IEnumerator ShowWeaponName()
    {
        weaponNameText.gameObject.SetActive(true);
        yield return new WaitForSeconds(weaponNameDisplayDuration);
        weaponNameText.gameObject.SetActive(false);
    }
}
