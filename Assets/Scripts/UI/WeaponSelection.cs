using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelection : MonoBehaviour
{
    public GameObject automaticRifleIcon;
    public GameObject sniperIcon;

    public GameObject autoImage;
    public GameObject sniperImage;

    public TextMeshProUGUI automaticText;
    public TextMeshProUGUI sniperText;

    public float scaleSpeed = 5f;
    public float weaponNameDisplayDuration = 1.0f;

    private bool currentA = false;

    public void SelectWeaponChange()
    {
        if (currentA)
        {
            LeanTween.scale(autoImage, new Vector3(1.2f, 1.2f, 1.2f), 0.25f).setEaseInBack();
            StartCoroutine(CustomFadeAnimator.Fade(automaticRifleIcon.GetComponent<Image>(), 0.5f, 1, 0.5f));
            StartCoroutine(CustomFadeAnimator.Fade(sniperIcon.GetComponent<Image>(), 1, 0.5f, 0.5f));
            LeanTween.scale(sniperImage, new Vector3(1f, 1f, 1f), 0.25f).setEaseInBack();
            LeanTween.moveX(automaticRifleIcon, automaticRifleIcon.transform.position.x + 10, 0.1f).setEaseInBounce().setOnComplete(() => LeanTween.moveX(automaticRifleIcon, automaticRifleIcon.transform.position.x - 10, 0.2f).setEaseOutBounce());
            
            StartCoroutine(CustomFadeAnimator.Fade(sniperText, 1, 0, 2));
            StartCoroutine(CustomFadeAnimator.Fade(automaticText, 0, 1, 2));
            StartCoroutine(CustomFadeAnimator.Fade(autoImage.GetComponent<Image>(), 0.5f, 1, 0.5f));
            StartCoroutine(CustomFadeAnimator.Fade(sniperImage.GetComponent<Image>(), 1f, 0.5f, 0.5f));
            currentA = !currentA;
        }
        else
        {
            LeanTween.scale(sniperImage, new Vector3(1.2f, 1.2f, 1.2f), 0.25f).setEaseInBack();
            StartCoroutine(CustomFadeAnimator.Fade(sniperIcon.GetComponent<Image>(), 0.5f, 1, 0.5f));
            StartCoroutine(CustomFadeAnimator.Fade(automaticRifleIcon.GetComponent<Image>(), 1, 0.5f, 0.5f));
            LeanTween.scale(autoImage, new Vector3(1f, 1f, 1f), 0.25f).setEaseInBack();
            LeanTween.moveX(sniperIcon, sniperIcon.transform.position.x + 10, 0.1f).setEaseInBounce().setOnComplete(() => LeanTween.moveX(sniperIcon, sniperIcon.transform.position.x - 10, 0.2f).setEaseOutBounce());

            StartCoroutine(CustomFadeAnimator.Fade(automaticText, 1, 0, 2));
            StartCoroutine(CustomFadeAnimator.Fade(sniperText, 0, 1, 2));
            StartCoroutine(CustomFadeAnimator.Fade(sniperImage.GetComponent<Image>(), 0.5f, 1, 0.5f));
            StartCoroutine(CustomFadeAnimator.Fade(autoImage.GetComponent<Image>(), 1f, 0.5f, 0.5f));
            currentA = !currentA;
        }
    }
}
