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

    [Header("Scripts")]
    public ZombieMovement zombieMovement;
    public SkillScript skillScript;
    public GunScript gunScriptWeapon1;
    public GunScript gunScriptWeapon2;

    [Header("Shop Buttons")]
    public TextMeshProUGUI increaseDropChancePrice;
    public TextMeshProUGUI aimTime1Price;
    public TextMeshProUGUI aimTime2Price;

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

    public BannerAds bannerAds;
    public InterstitialAds interstitialAds;

    private List<Vector2> pauseUIInitPos;
    private List<Vector2> shopUIInitPos;
    private bool isAnimating = false;
    private const int maxUpgradeLevel = 5;


    
    private void Start()
    {
        pauseUIInitPos = new List<Vector2>();
        shopUIInitPos = new List<Vector2>();

        ZombieApocalypse.GameStatus.isPaused = (Time.timeScale == 0);

        int pauseUICount = pauseUI.transform.childCount;
        int shopUICount = shopUI.transform.childCount;
        int count = Mathf.Max(pauseUICount, shopUICount);

        for (int i = 0; i < count; i++)
        {
            if (i < pauseUICount)
            {
                pauseUIInitPos.Add(pauseUI.transform.GetChild(i).transform.localPosition);
                pauseUI.transform.GetChild(i).transform.localPosition = new Vector2(0, -1000);
            }

            if (i < shopUICount)
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

        if (!isAnimating && !isShopMenuOpen && !isOptionsOpen)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenShop();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseUI.SetActive(true);
                bannerAds.HideBannerAd();
                pauseCheck();
            }
        }
        updateButtonText();
    }

    private void pauseCheck()
    {
        if (!ZombieApocalypse.GameStatus.isPaused && !isShopMenuOpen && !isOptionsOpen)
        {
            shopUI.SetActive(false);
            bannerAds.ShowBannerAd();
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
                bannerAds.HideBannerAd();
                break;
            case "ResumeButton":
                pauseCheck();
                bannerAds.HideBannerAd();
                break;
            case "ShopButton":
                StartCoroutine(shopFromPause());
                bannerAds.HideBannerAd();
                break;
            case "MainMenuButton":
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                bannerAds.HideBannerAd();
                break;
            case "OptionsButton":
                isOptionsOpen = true;
                pauseUI.SetActive(false);
                optionsUI.SetActive(true);
                bannerAds.HideBannerAd();
                break;
            case "LeaderBoardButton":
                gameoverUI.SetActive(false);
                LeaderboardUI.SetActive(true);
                bannerAds.HideBannerAd();
                break;
            case "BackButton":
                gameoverUI.SetActive(true);
                LeaderboardUI.SetActive(false);
                bannerAds.ShowBannerAd();
                break;
            case "BackOptionsButton":
                isOptionsOpen = false;
                pauseUI.SetActive(true);
                optionsUI.SetActive(false);
                bannerAds.ShowBannerAd();
                break;
            case "ShopResumeButton":
                if (isShopMenuOpen)
                    CloseShop();
                else
                    Debug.Log("Ignoring ShopResumeButton click while shop is closed.");
                break;
            case "BuyAmmo1Button":
                buyWeapon1Ammo();
                break;
            case "BuyAmmo2Button":
                buyWeapon2Ammo();
                break;
            case "IncreaseDmgButton":
                
                break;
            case "IncreaseDropButton":
                upgradeDropChance();
                break;
            case "AimTime1Button":
                upgradeAimTime1();
                break;
            case "AimTime2Button":
                upgradeAimTime2();
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

        interstitialAds.LoadAd();
        interstitialAds.ShowAd();
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
        pauseCheck();

        yield return new WaitForSeconds(0.5f);
        
        shopCheck();
    }
    private void OpenShop()
    {
        isShopMenuOpen = true;
        shopUI.SetActive(true);
        shopCheck();
    }
    private void CloseShop()
    {
        isShopMenuOpen = false;
        shopCheck();
    }
    private void upgradeDropChance()
    {
        // Debug.Log("Before Coins: " + ZombieApocalypse.GameData.coinCounter);
        // Debug.Log("Before Level: " + ZombieApocalypse.GameShopInfo.item_drop_chance_level);
        if (ZombieApocalypse.GameShopInfo.item_drop_chance_level < maxUpgradeLevel)
        {
            if (ZombieApocalypse.GameData.coinCounter - (int)zombieMovement.dropChancePerLevelPrice[ZombieApocalypse.GameShopInfo.item_drop_chance_level] >= 0)
            {
                ZombieApocalypse.GameData.coinCounter -= (int)zombieMovement.dropChancePerLevelPrice[ZombieApocalypse.GameShopInfo.item_drop_chance_level];
                ZombieApocalypse.GameShopInfo.item_drop_chance_level += 1;
            }
        }
        // Debug.Log("After Coins: " + ZombieApocalypse.GameData.coinCounter);
        // Debug.Log("After Level: " + ZombieApocalypse.GameShopInfo.item_drop_chance_level);
    }
    private void buyWeapon1Ammo()
    {
        if (ZombieApocalypse.GameData.coinCounter - 200 >= 0)
        {
            ZombieApocalypse.GameData.coinCounter -= 200;
            ZombieApocalypse.GameShopInfo.weapon_1_1 += 300;
            gunScriptWeapon1.bulletsIHave += 300;
        }
    }
    private void buyWeapon2Ammo()
    {
        if (ZombieApocalypse.GameData.coinCounter - 200 >= 0)
        {
            ZombieApocalypse.GameData.coinCounter -= 200;
            ZombieApocalypse.GameShopInfo.weapon_2_1 += 300;
        }
    }
    private void upgradeAimTime1()
    {
        if (ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level < maxUpgradeLevel)
        {
            if (ZombieApocalypse.GameData.coinCounter - (int)skillScript.aimingTimePerLevelPrice[ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level] >= 0)
            {
                ZombieApocalypse.GameData.coinCounter -= (int)skillScript.aimingTimePerLevelPrice[ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level];
                ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level += 1;
            }
        }
    }
    private void upgradeAimTime2()
    {
        if (ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level < maxUpgradeLevel)
        {
            if (ZombieApocalypse.GameData.coinCounter - (int)skillScript.aimingTimePerLevelPrice[ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level] >= 0)
            {
                ZombieApocalypse.GameData.coinCounter -= (int)skillScript.aimingTimePerLevelPrice[ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level];
                ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level += 1;
            }
        }
    }
    private void updateButtonText()
    {
        if (ZombieApocalypse.GameShopInfo.item_drop_chance_level < maxUpgradeLevel)
            increaseDropChancePrice.text = zombieMovement.dropChancePerLevelPrice[ZombieApocalypse.GameShopInfo.item_drop_chance_level].ToString();
        else
            increaseDropChancePrice.text = "MAX";

        if (ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level < maxUpgradeLevel)
            aimTime1Price.text = skillScript.aimingTimePerLevelPrice[ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level].ToString();
        else
            aimTime1Price.text = "MAX";

        if (ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level < maxUpgradeLevel)
            aimTime2Price.text = skillScript.aimingTimePerLevelPrice[ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level].ToString();
        else
            aimTime2Price.text = "MAX";
    }
}
