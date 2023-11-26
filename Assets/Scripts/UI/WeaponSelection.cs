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

    public float scaleSpeed = 5f; // Adjust the speed of scaling
    public float weaponNameDisplayDuration = 1.0f; // Duration to display the weapon name

    private bool currentA = true;
    void Start()
    {
        currentA = false;
    }

    private void Update()
    {

    }
    public void SelectWeaponChange()
    {
        if (currentA)
        {
            LeanTween.scale(autoImage, new Vector3(1.2f, 1.2f, 1.2f), 0.25f).setEaseInBack();
            StartCoroutine(Fade(0.5f, 1, automaticRifleIcon, 0.5f));
            StartCoroutine(Fade(1, 0.5f, sniperIcon, 0.5f));
            LeanTween.scale(sniperImage, new Vector3(1f, 1f, 1f), 0.25f).setEaseInBack();
            LeanTween.moveX(automaticRifleIcon, automaticRifleIcon.transform.position.x + 10, 0.1f).setEaseInBounce().setOnComplete(() => LeanTween.moveX(automaticRifleIcon, automaticRifleIcon.transform.position.x - 10, 0.2f).setEaseOutBounce());
            
            StartCoroutine(Fade(1, 0, sniperText, 2));
            StartCoroutine(Fade(0, 1, automaticText, 2));
            StartCoroutine(Fade(0.5f, 1, autoImage, 0.5f));
            StartCoroutine(Fade(1, 0.5f, sniperImage, 0.5f));
            currentA = !currentA;
        }
        else
        {
            LeanTween.scale(sniperImage, new Vector3(1.2f, 1.2f, 1.2f), 0.25f).setEaseInBack();
            StartCoroutine(Fade(0.5f, 1, sniperIcon, 0.5f));
            StartCoroutine(Fade(1, 0.5f, automaticRifleIcon, 0.5f));
            LeanTween.scale(autoImage, new Vector3(1f, 1f, 1f), 0.25f).setEaseInBack();
            LeanTween.moveX(sniperIcon, sniperIcon.transform.position.x + 10, 0.1f).setEaseInBounce().setOnComplete(() => LeanTween.moveX(sniperIcon, sniperIcon.transform.position.x - 10, 0.2f).setEaseOutBounce());

            StartCoroutine(Fade(1, 0, automaticText, 2));
            StartCoroutine(Fade(0, 1, sniperText, 2));
            StartCoroutine(Fade(1, 0.5f, autoImage, 0.5f));
            StartCoroutine(Fade(0.5f, 1, sniperImage, 0.5f));
            currentA = !currentA;
        }
    }
    private float m_timerCurrent;
    public AnimationCurve m_smoothCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
    private readonly WaitForSeconds m_skipFrame = new WaitForSeconds(0.01f);

    private IEnumerator Fade(float start, float end, TextMeshProUGUI text, float fadeDuration)
    {
        m_timerCurrent = 0f;

        while (m_timerCurrent <= fadeDuration)
        {
            if (!currentA)
                sniperText.color = new Color(255, 255, 255, 0);
            else
                automaticText.color = new Color(255, 255, 255, 0);  
            m_timerCurrent += Time.deltaTime;
            Color c = text.color;
            text.color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, m_smoothCurve.Evaluate(m_timerCurrent / fadeDuration)));
            yield return m_skipFrame;
        }
    }
    private IEnumerator Fade(float start, float end, GameObject icon, float fadeDuration)
    {
        m_timerCurrent = 0f;

        while (m_timerCurrent <= fadeDuration)
        {
            if (!currentA)
                sniperText.color = new Color(255, 255, 255, 0);
            else
                automaticText.color = new Color(255, 255, 255, 0);
            m_timerCurrent += Time.deltaTime;
            Color c = icon.GetComponent<Image>().color;
            icon.GetComponent<Image>().color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, m_smoothCurve.Evaluate(m_timerCurrent / fadeDuration)));
            yield return m_skipFrame;
        }
    }
}
