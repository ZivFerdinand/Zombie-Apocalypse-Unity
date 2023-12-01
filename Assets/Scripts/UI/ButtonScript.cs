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

    [Header("Pause UI")]
    public RectTransform pauseOverlay;
    public GameObject pauseUI;
    public GameObject optionsUI;

    [Header("Game Over UI")]
    public GameObject gameoverUI;
    public RectTransform gameoverOverlay;
    public GameObject LeaderboardUI;
    public GameObject askNameUI;
    public TextMeshProUGUI scoreText;
    public PlayerHealth playerHealth;

    [Header("Shop UI")]
    public GameObject shopUI;
    private bool isShopMenuOpen = false;
    private bool isOptionsOpen = false;

    private List<Vector2> pauseUIInitPos;
    private List<Vector2> shopUIInitPos;
    private bool isAnimating = false;

    
    private void Start()
    {
        pauseUIInitPos = new List<Vector2>();
        shopUIInitPos = new List<Vector2>();

        ZombieApocalypse.GameStatus.isPaused = (Time.timeScale == 0);

        int shopUIChildCount = shopUI.transform.childCount;
        for (int i = 0; i < pauseUI.transform.childCount; i++)
        {
            pauseUIInitPos.Add(pauseUI.transform.GetChild(i).transform.localPosition);
            pauseUI.transform.GetChild(i).transform.localPosition = new Vector2(0, -1000);

            if (i < shopUIChildCount)
            {
                shopUIInitPos.Add(shopUI.transform.GetChild(i).transform.localPosition);
                shopUI.transform.GetChild(i).transform.localPosition = new Vector2(0, -1000);
            }
        }
    }
    void Update()
    {

        if ((!isAnimating && Input.GetKeyDown(KeyCode.Backspace)) || (playerHealth.currentHealth < 0.1f && !isAnimating))
        {
            isAnimating = true;
            StartCoroutine(playerDeathAnimation());
        }
        if ((!isAnimating && Input.GetKeyDown(KeyCode.B)) && !isShopMenuOpen && !isOptionsOpen) // Check if the shop menu is not open
        {
            shopUI.SetActive(true);
            shopCheck();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isAnimating && !isShopMenuOpen && !isOptionsOpen) // Check if the shop menu is not open
        {
            pauseUI.SetActive(true);
            pauseCheck();
        }
    }

    private void pauseCheck()
    {
        if (!ZombieApocalypse.GameStatus.isPaused && !isShopMenuOpen && !isOptionsOpen)
        {
            shopUI.SetActive(false);
            isAnimating = true;
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
        else if (!isShopMenuOpen && !isOptionsOpen)
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
            case "ShopResumeButton":
                shopCheck();
                break;
            case "ShopButton":
                // pauseCheck();
                // StartCoroutine(shopFromPause());
                break;
            case "MainMenuButton":
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                break;
            case "OptionsButton":
                isOptionsOpen = true;
                pauseUI.SetActive(false);
                optionsUI.SetActive(true);
                break;
            case "LeaderBoardButton":
                gameoverUI.SetActive(false);
                LeaderboardUI.SetActive(true);
                break;
            case "BackButton":
                gameoverUI.SetActive(true);
                LeaderboardUI.SetActive(false);
                break;
            case "BackOptionsButton":
                isOptionsOpen = false;
                pauseUI.SetActive(true);
                optionsUI.SetActive(false);
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
    private void shopCheck()
    {
        if (!ZombieApocalypse.GameStatus.isPaused)
        {
            isAnimating = true;
            shopUI.SetActive(true);
            isShopMenuOpen = true;
            pauseUI.SetActive(false);
            StartCoroutine(CustomFadeAnimator.Fade(pauseOverlay.GetComponent<Image>(), 0, 1f, 0.25f));

            for (int i = 0; i < shopUI.transform.childCount; i++)
            {
                if (i == shopUI.transform.childCount - 1)
                    LeanTween.moveLocal(shopUI.transform.GetChild(i).gameObject, shopUIInitPos[i], 0.5f).setEaseOutBack().setOnComplete(() => {
                        isAnimating = false; pauseUpdate();

                    });
                else
                    LeanTween.moveLocal(shopUI.transform.GetChild(i).gameObject, shopUIInitPos[i], 0.5f).setEaseOutBack();
            }
        }
        else
        {
            isAnimating = true;
            isShopMenuOpen = false;
            pauseUpdate();
            StartCoroutine(CustomFadeAnimator.Fade(pauseOverlay.GetComponent<Image>(), 1, 0, 0.25f));

            for (int i = 0; i < shopUI.transform.childCount; i++)
            {
                if (i == shopUI.transform.childCount - 1)
                    LeanTween.moveLocal(shopUI.transform.GetChild(i).gameObject, new Vector2(0, -1000), 0.5f).setEaseInBack().setOnComplete(() => {

                        isAnimating = false;
                    });
                else
                    LeanTween.moveLocal(shopUI.transform.GetChild(i).gameObject, new Vector2(0, -1000), 0.5f).setEaseInBack();
            }

            pauseUI.SetActive(true); // Activate the pause menu when shop closes
        }
    }
    private IEnumerator shopFromPause()
    {
        yield return new WaitForSeconds(0.5f);

        shopUI.SetActive(true);
        pauseUI.SetActive(false);
        shopCheck();
    }
}
