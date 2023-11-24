using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class StartUIController : MonoBehaviour
{
    public GameObject mainSc, creditSc;
    public GameObject playButt, normButt, hardButt;
    public GameObject overlay;
    public GameObject loadingCircle;
    private Vector2 creditScInitPos;
    private bool creditScActive;
    private List<Vector2> mainScChildInitPos;
    private bool isLoading = false;
    public void Start()
    {
        creditScInitPos = creditSc.transform.position;
        creditScActive = false;
        mainScChildInitPos = new List<Vector2>();
        for (int i = 0; i < mainSc.transform.childCount; i++)
        {
            mainScChildInitPos.Add(mainSc.transform.GetChild(i).transform.localPosition);
        }
        normButt.SetActive(false);
        hardButt.SetActive(false);
        loadingCircle.SetActive(false);
    }
    public void Update()
    {
        if(isLoading)
        {
            loadingCircle.transform.localEulerAngles = new Vector3(0, 0, loadingCircle.transform.localEulerAngles.z+50f*Time.deltaTime);
        }
        if(creditScActive)
        {
            creditScActive = false;
            LeanTween.moveLocal(creditSc, new Vector2(0, 1870), 15f).setOnComplete(() => 
            {
                creditSc.transform.position = creditScInitPos;
                for (int i = 0; i < mainSc.transform.childCount; i++)
                {
                    LeanTween.moveLocal(mainSc.transform.GetChild(i).gameObject, mainScChildInitPos[i], 0.5f).setEaseOutBack();
                }
                creditSc.SetActive(false);
                StartCoroutine(Fade(0.5f, 0));
            });
        }
    }
    public void onButtonClick(string name)
    {
        if (!isLoading)
        {
            if (name == "PlayButton")
            {
                playButt.SetActive(false);
                normButt.SetActive(true);
                hardButt.SetActive(true);
                //SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }
            if (name == "NormalButton")
            {
                StartCoroutine(OnLoadScene(0));

            }
            if (name == "HardButton")
            {
                StartCoroutine(OnLoadScene(1));
            }
            if (name == "CreditsButton")
            {
                StartCoroutine(Fade(0, 0.5f));
                creditSc.SetActive(true);
                for (int i = 0; i < mainSc.transform.childCount; i++)
                {
                    LeanTween.moveLocal(mainSc.transform.GetChild(i).gameObject, new Vector2(0, -1500), 0.5f).setEaseInBack().setOnComplete(() => { creditScActive = true; });
                }
            }
            if (name == "OptionsButton")
            {

            }
        }
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
    private IEnumerator OnLoadScene(int diff)
    {
        isLoading = true;
        loadingCircle.SetActive(true);
        StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(1);
        ZombieApocalypse.DatabaseStatus.gameMode = diff;
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }
}
