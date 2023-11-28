using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject pauseUI;
    public RectTransform pauseOverlay;
    public GameObject gameoverUI;
    public RectTransform gameoverOverlay;
    public TextMeshProUGUI scoreText;

    private List<Vector2> pauseUIInitPos;
    private bool isAnimating = false;

    public PlayerHealth playerHealth;
    private void Start()
    {
        pauseUIInitPos = new List<Vector2>();
        ZombieApocalypse.GameStatus.isPaused = (Time.timeScale == 0);

        for (int i = 0; i < pauseUI.transform.childCount; i++)
        {
            pauseUIInitPos.Add(pauseUI.transform.GetChild(i).transform.localPosition);
            pauseUI.transform.GetChild(i).transform.localPosition = new Vector2(0, -1000);
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            gameoverCheck();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isAnimating)
        {
            pauseCheck();
        }
        if (playerHealth.currentHealth < 0.1f && !isAnimating)
        {
            gameoverCheck();
        }
    }

    private void pauseCheck()
    {
        if (!ZombieApocalypse.GameStatus.isPaused)
        {
            isAnimating = true;
            pauseUI.SetActive(true);
            StartCoroutine(CustomFadeAnimator.Fade(pauseOverlay.GetComponent<Image>(), 0, 1f, 0.25f));

            for (int i = 0; i < pauseUI.transform.childCount; i++)
            {
                if (i == pauseUI.transform.childCount - 1)
                    LeanTween.moveLocal(pauseUI.transform.GetChild(i).gameObject, pauseUIInitPos[i], 0.5f).setEaseOutBack().setOnComplete(() => {
                        isAnimating = false; pauseUpdate();
                        
                    });
                else
                    LeanTween.moveLocal(pauseUI.transform.GetChild(i).gameObject, pauseUIInitPos[i], 0.5f).setEaseOutBack();
            }
        }
        else
        {
            isAnimating = true;
            pauseUpdate();
            StartCoroutine(CustomFadeAnimator.Fade(pauseOverlay.GetComponent<Image>(), 1, 0, 0.25f));

            for (int i = 0; i < pauseUI.transform.childCount; i++)
            {
                if (i == pauseUI.transform.childCount - 1)
                    LeanTween.moveLocal(pauseUI.transform.GetChild(i).gameObject, new Vector2(0, -1000), 0.5f).setEaseInBack().setOnComplete(() => {

                        isAnimating = false; });
                else
                    LeanTween.moveLocal(pauseUI.transform.GetChild(i).gameObject, new Vector2(0, -1000), 0.5f).setEaseInBack();
            }

        }
    }
    private void pauseUpdate()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        ZombieApocalypse.GameStatus.isPaused = (Time.timeScale == 0);
        Cursor.visible = ZombieApocalypse.GameStatus.isPaused;
    }
    public void onButtonClick(string name)
    {
        switch (name)
        {
            case "RestartButton":
                pauseCheck();
                SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
                break;
            case "ResumeButton":
                pauseCheck();
                break;
            case "MainMenuButton":
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                break;
        }

    }
    private void gameoverCheck()
    {
        gameoverUI.SetActive(true);

        StartCoroutine(CustomFadeAnimator.Fade(gameoverOverlay.GetComponent<Image>(), 0, 1, 0.4f));
        StartCoroutine(CustomFadeAnimator.Fade(gameoverOverlay.transform.GetChild(0).GetComponent<Image>(), 0, 1, 0.4f));
        
        StartCoroutine(setGameOver());
        updateScoreText();
    }
    private IEnumerator setGameOver()
    {
        yield return new WaitForSeconds(0.45f);

        pauseUpdate();
    }
    private void updateScoreText()
    {
        scoreText.text = "Score : " + ZombieApocalypse.GameData.gameScore.ToString();
    }
}
