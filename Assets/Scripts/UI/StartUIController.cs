using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartUIController : MonoBehaviour
{
    public GameObject mainSc, creditSc;
    public GameObject overlay;
    private Vector2 creditScInitPos;
    private bool creditScActive;
    private List<Vector2> mainScChildInitPos;
    public void Start()
    {
        creditScInitPos = creditSc.transform.position;
        creditScActive = false;
        mainScChildInitPos = new List<Vector2>();
        for (int i = 0; i < mainSc.transform.childCount; i++)
        {
            mainScChildInitPos.Add(mainSc.transform.GetChild(i).transform.localPosition);
        }
    }
    public void Update()
    {
        if(creditScActive)
        {
            creditScActive = false;
            LeanTween.moveLocal(creditSc, new Vector2(0, 1870), 15f).setOnComplete(() => 
            {
                creditSc.transform.position = creditScInitPos;
                for (int i = 0; i < mainSc.transform.childCount; i++)
                {
                    LeanTween.moveLocal(mainSc.transform.GetChild(i).gameObject, mainScChildInitPos[i], 1).setEaseOutBack();
                }
                creditSc.SetActive(false);
                StartCoroutine(Fade(0.5f, 0));
            });
        }
    }
    public void onButtonClick(string name)
    {
        if (name == "PlayButton")
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        if (name == "CreditsButton")
        {
            StartCoroutine(Fade(0, 0.5f));
            creditSc.SetActive(true);
            for (int i = 0; i < mainSc.transform.childCount; i++)
            {
                LeanTween.moveLocal(mainSc.transform.GetChild(i).gameObject, new Vector2(0, -1500), 1).setEaseInBack().setOnComplete(() => { creditScActive = true; });
            }
        }
        if (name == "OptionsButton")
        {

        }
    }
    private IEnumerator FadeInOut()
    {
        float start = 0f;
        float end = 1f;

        yield return StartCoroutine(Fade(start, end)); // Fade in.
        yield return new WaitForSeconds(1); // Stay
        yield return StartCoroutine(Fade(end, start)); // Fade out.
    }

    private float m_timerCurrent;
    private float m_fadeDuration = 1f;
    public AnimationCurve m_smoothCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
    private readonly WaitForSeconds m_skipFrame = new WaitForSeconds(0.01f);

    private IEnumerator Fade(float start, float end)
    {
        m_timerCurrent = 0f;

        while (m_timerCurrent <= m_fadeDuration)
        {
            m_timerCurrent += Time.deltaTime;
            Color c = overlay.GetComponent<Image>().color;
            overlay.GetComponent<Image>().color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, m_smoothCurve.Evaluate(m_timerCurrent / m_fadeDuration)));
            yield return m_skipFrame;
        }
    }

}
