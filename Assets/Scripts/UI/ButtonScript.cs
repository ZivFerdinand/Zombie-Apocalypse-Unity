using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject player;
    public GameObject player2;

    public GameObject askNameUI;
    public GameObject pauseUI;
    public GameObject LeaderboardUI;
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

        if ((!isAnimating && Input.GetKeyDown(KeyCode.Backspace)) || (playerHealth.currentHealth < 0.1f && !isAnimating))
        {
            isAnimating = true;
            StartCoroutine(playerDeathAnimation());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isAnimating)
        {
            pauseCheck();
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
            case "LeaderBoardButton":
                gameoverUI.SetActive(false);
                LeaderboardUI.SetActive(true);
                break;
            case "BackButton":
                gameoverUI.SetActive(true);
                LeaderboardUI.SetActive(false);
                break;

        }

    }
    public void onPlayerNameSubmit()
    {
        askNameUI.SetActive(false);
        LeaderboardUI.SetActive(true);
    }
    private IEnumerator playerDeathAnimation()
    {
        // Get the player's forward direction
        Vector3 playerForward = player2.transform.forward;

        // Calculate the target rotation based on the player's forward direction
        Quaternion targetRotation = Quaternion.LookRotation(playerForward, Vector3.up);

        // Convert the quaternion to Euler angles
        Vector3 targetEulerAngles = targetRotation.eulerAngles;

        //// Initiate the rotation animation using LeanTween
        //LeanTween.rotateLocal(player, targetEulerAngles - new Vector3(90, 0, 0), 1.5f).setEaseOutBounce();
        //LeanTween.rotateLocal(player2, targetEulerAngles - new Vector3(90, 0, 0), 1.5f).setEaseOutBounce();

        player2.GetComponent<GunInventory>().currentHandsAnimator.SetBool("changingWeapon", true);


        //LeanTween.rotate(player, new Vector3(-90, initEuler.y, initEuler.z), 1.5f).setEaseOutBounce();
        //LeanTween.moveX(player, player.transform.position.x -10, 2);
        yield return new WaitForSeconds(0.75f);
        player.SetActive(false);
        LeanTween.rotateLocal(player2, targetEulerAngles - new Vector3(90, 0, 0), 2.5f).setEaseOutBounce();
        yield return new WaitForSeconds(1.25f);
        //yield return new WaitForSeconds(0.5f);

        gameoverCheck();
    }
    private void gameoverCheck()
    {
        gameoverUI.SetActive(true);
        if (ZombieApocalypse.GameData.playerName == "")
        {
            gameoverUI.SetActive(false);
            askNameUI.SetActive(true);
        }


        StartCoroutine(CustomFadeAnimator.Fade(gameoverOverlay.GetComponent<Image>(), 0, 1, 1f));
        StartCoroutine(CustomFadeAnimator.Fade(gameoverOverlay.transform.GetChild(0).GetComponent<Image>(), 0, 1, 0.75f));
        
        StartCoroutine(setGameOver());
        updateScoreText();
    }
    private IEnumerator setGameOver()
    {
        yield return new WaitForSeconds(1f);

        pauseUpdate();
    }
    private void updateScoreText()
    {
        scoreText.text = "Score : " + ZombieApocalypse.GameData.gameScore.ToString();
    }
}
