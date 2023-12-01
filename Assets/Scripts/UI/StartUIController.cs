using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class StartUIController : MonoBehaviour
{
    private const float loadingRotateSpeed = 50f;
    public GameObject mainSc, creditSc;
    public GameObject playButt, normButt, hardButt;
    public GameObject overlay;
    public GameObject loadingCircle;
    public GameObject leaderboardUI;
    public GameObject optionsUI;

    private List<Vector2> mainScChildInitPos;
    private Vector2 creditScInitPos;

    private bool creditScActive;
    private bool isLoading;

    private void Start()
    {
        creditScInitPos = creditSc.transform.position;
        isLoading = false;
        creditScActive = false;
        mainScChildInitPos = new List<Vector2>();

        foreach (Transform child in mainSc.transform)
        {
            mainScChildInitPos.Add(child.localPosition);
        }

        normButt.SetActive(false);
        hardButt.SetActive(false);
        loadingCircle.SetActive(false);
    }
    private void Update()
    {
        if(isLoading)
            loadingCircle.transform.localEulerAngles += new Vector3(0, 0, loadingRotateSpeed * Time.deltaTime);

        if(creditScActive)
        {
            creditScActive = false;
            LeanTween.moveLocal(creditSc, new Vector2(0, 1870), 15f)
            .setOnComplete(() =>
            {
                creditSc.transform.position = creditScInitPos;

                foreach (Transform child in mainSc.transform)
                {
                    LeanTween.moveLocal(child.gameObject, mainScChildInitPos[child.GetSiblingIndex()], 0.5f).setEaseOutBack();
                }

                creditSc.SetActive(false);
                StartCoroutine(CustomFadeAnimator.Fade(overlay.GetComponent<Image>(), 0.25f, 0, 1));
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
                StartCoroutine(CustomFadeAnimator.Fade(overlay.GetComponent<Image>(), 0f, 0.25f, 1));
                creditSc.SetActive(true);

                foreach (Transform child in mainSc.transform)
                {
                    LeanTween.moveLocal(child.gameObject, new Vector2(0, -1500), 0.5f).setEaseInBack().setOnComplete(() => creditScActive = true);
                }

            }
            if (name == "LeaderboardButton")
            {
                StartCoroutine(CustomFadeAnimator.Fade(overlay.GetComponent<Image>(), 0, 1, 0.25f));
                leaderboardUI.SetActive(true);
            }
            if (name == "OptionsButton")
            {
                StartCoroutine(CustomFadeAnimator.Fade(overlay.GetComponent<Image>(), 0, 1, 0.25f));
                optionsUI.SetActive(true);
            }
            if (name == "BackButton")
            {
                StartCoroutine(CustomFadeAnimator.Fade(overlay.GetComponent<Image>(), 1, 0, 0.25f));
                leaderboardUI.SetActive(false);
            }
            if (name == "BackOptionsButton")
            {
                StartCoroutine(CustomFadeAnimator.Fade(overlay.GetComponent<Image>(), 1, 0, 0.25f));
                optionsUI.SetActive(false);
            }
        }
    }

    private IEnumerator OnLoadScene(int diff)
    {
        ZombieApocalypse.GameData.gameScore = 0;
        ZombieApocalypse.GameStatus.gameMode = diff;
        StartCoroutine(CustomFadeAnimator.Fade(overlay.GetComponent<Image>(), 0, 1, 1));

        isLoading = true;
        loadingCircle.SetActive(isLoading);

        yield return new WaitForSeconds(1);


        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }
}
