using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public AudioSource clip;
    public LeaderBoardScript leaderBoardScript;
    public TextMeshProUGUI scoreA;
    public GameObject player;
    public GameObject player2;

    [Header("Scripts")]
    public ZombieMovement zombieMovement;
    public SkillScript skillScript;
    public GunInventory gunInventory;
    public BulletScript bulletScript;

    [Header("Shop Buttons")]
    public TextMeshProUGUI increaseDropChancePrice;
    public TextMeshProUGUI aimTime1Price;
    public TextMeshProUGUI aimTime2Price;
    public TextMeshProUGUI weapon1DmgPrice;
    public TextMeshProUGUI weapon2DmgPrice;

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
        interstitialAds.LoadAd();
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
        if (playerHealth.currentHealth < 0.1f && !isAnimating)
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

    /// <summary>
    /// This function creates/removes the pause screen.
    /// </summary>
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

    /// <summary>
    /// This function pauses the game and sets the player's cursor status.
    /// </summary>
    private void pauseUpdate()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        ZombieApocalypse.GameStatus.isPaused = (Time.timeScale == 0);
        Cursor.visible = ZombieApocalypse.GameStatus.isPaused;
    }

    /// <summary>
    /// This function detects if the player have entered the zombie spawn point.
    /// </summary>
    /// <param name="name">The button's name</param>
    public void onButtonClick(string name)
    {
        switch (name)
        {
            case "RestartButton":
                ZombieApocalypse.GameData.gameScore = 0;
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
                bannerAds.ShowBannerAd();
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
                break;
            case "BackButton":
                gameoverUI.SetActive(true);
                LeaderboardUI.SetActive(false);
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
                clip.Play();
                buyWeapon1Ammo();
                break;
            case "BuyAmmo2Button":
                clip.Play();
                buyWeapon2Ammo();
                break;
            case "IncreaseDmg1Button":
                clip.Play();
                upgradeWeapon1Dmg();
                break;
            case "IncreaseDmg2Button":
                clip.Play();
                upgradeWeapon2Dmg();
                break;
            case "IncreaseDropButton":
                clip.Play();
                upgradeDropChance();
                break;
            case "AimTime1Button":
                clip.Play();
                upgradeAimTime1();
                break;
            case "AimTime2Button":
                clip.Play();
                upgradeAimTime2();
                break;
        }
    }

    /// <summary>
    /// This function removes the ask name screen and creates the leaderboard screen.
    /// </summary>
    public void onPlayerNameSubmit()
    {
        askNameUI.SetActive(false);
        LeaderboardUI.SetActive(true);
    }

    /// <summary>
    /// This function plays the player death animation, sets the game over status and plays the ad after the game is over.
    /// </summary>
    private IEnumerator playerDeathAnimation()
    {
        // Get the player's forward direction
        Vector3 playerForward = player2.transform.forward;

        // Calculate the target rotation based on the player's forward direction
        Quaternion targetRotation = Quaternion.LookRotation(playerForward, Vector3.up);

        // Convert the quaternion to Euler angles
        Vector3 targetEulerAngles = targetRotation.eulerAngles;

        player2.GetComponent<GunInventory>().currentHandsAnimator.SetBool("changingWeapon", true);

        yield return new WaitForSeconds(0.75f);
        player.SetActive(false);
        LeanTween.rotateLocal(player2, targetEulerAngles - new Vector3(90, 0, 0), 2.5f).setEaseOutBounce();
        yield return new WaitForSeconds(1.25f);
        //yield return new WaitForSeconds(0.5f);

        interstitialAds.ShowAd();
        gameoverCheck();
    }

    /// <summary>
    /// This function activates the game over screen.
    /// </summary>
    private void gameoverCheck()
    {
        gameoverUI.SetActive(true);
        if (ZombieApocalypse.GameData.playerName == "")
        {
            gameoverUI.SetActive(false);
            askNameUI.SetActive(true);
        }
        else
            leaderBoardScript.SetLeaderboardEntry("", int.Parse(scoreA.text));


        StartCoroutine(CustomFadeAnimator.Fade(gameoverOverlay.GetComponent<Image>(), 0, 1, 1f));
        StartCoroutine(CustomFadeAnimator.Fade(gameoverOverlay.transform.GetChild(0).GetComponent<Image>(), 0, 1, 0.75f));
        
        StartCoroutine(setGameOver());
        updateScoreText();
    }

    /// <summary>
    /// This function plays the pause update function- with a delay.
    /// </summary>
    private IEnumerator setGameOver()
    {
        yield return new WaitForSeconds(1f);

        pauseUpdate();
    }

    /// <summary>
    /// This function updates the player's final score on the screen.
    /// </summary>
    private void updateScoreText()
    {
        scoreText.text = "Score : " + ZombieApocalypse.GameData.gameScore.ToString();
    }

    /// <summary>
    /// This function creates/removes the shop screen.
    /// </summary>
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

    /// <summary>
    /// This function removes the pause screen and activates the shop screen with a delay.
    /// </summary>
    private IEnumerator shopFromPause()
    {
        pauseCheck();

        yield return new WaitForSeconds(0.5f);
        
        shopCheck();
    }

    /// <summary>
    /// This function creates the shop screen.
    /// </summary>
    private void OpenShop()
    {
        isShopMenuOpen = true;
        shopUI.SetActive(true);
        shopCheck();
    }

    /// <summary>
    /// This function removes the shop screen.
    /// </summary>
    private void CloseShop()
    {
        isShopMenuOpen = false;
        shopCheck();
    }

    /// <summary>
    /// This function upgrades the player's chance to increase the drop rate from zombies.
    /// </summary>
    private void upgradeDropChance()
    {
        if (ZombieApocalypse.GameShopInfo.item_drop_chance_level < maxUpgradeLevel)
        {
            if (ZombieApocalypse.GameData.coinCounter - (int)zombieMovement.dropChancePerLevelPrice[ZombieApocalypse.GameShopInfo.item_drop_chance_level] >= 0)
            {
                ZombieApocalypse.GameData.coinCounter -= (int)zombieMovement.dropChancePerLevelPrice[ZombieApocalypse.GameShopInfo.item_drop_chance_level];
                ZombieApocalypse.GameShopInfo.item_drop_chance_level += 1;
            }
        }
    }

    /// <summary>
    /// This function increases the ammo count of weapon 1.
    /// </summary>
    private void buyWeapon1Ammo()
    {
        if (ZombieApocalypse.GameData.coinCounter - 200 >= 0)
        {
            ZombieApocalypse.GameData.coinCounter -= 200;
            gunInventory.instantiatedGuns[0].GetComponent<GunScript>().bulletsIHave += 60;
        }
    }

    /// <summary>
    /// This function increases the ammo count of weapon 2.
    /// </summary>
    private void buyWeapon2Ammo()
    {
        if (ZombieApocalypse.GameData.coinCounter - 200 >= 0)
        {
            ZombieApocalypse.GameData.coinCounter -= 200;
            gunInventory.instantiatedGuns[1].GetComponent<GunScript>().bulletsIHave += 30;
        }
    }

    /// <summary>
    /// This function increases the aiming time of skill 1.
    /// </summary>
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
    
    /// <summary>
    /// This function increases the aiming time of skill 2.
    /// </summary>
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

    /// <summary>
    /// This function increases the damage of weapon 1.
    /// </summary>
    private void upgradeWeapon1Dmg()
    {
        if (ZombieApocalypse.GameShopInfo.weapon_1_dmg_level < maxUpgradeLevel)
        {
            if (ZombieApocalypse.GameData.coinCounter - (int)bulletScript.bulletDamagePerLevelPrice[ZombieApocalypse.GameShopInfo.weapon_1_dmg_level] >= 0)
            {
                ZombieApocalypse.GameData.coinCounter -= (int)bulletScript.bulletDamagePerLevelPrice[ZombieApocalypse.GameShopInfo.weapon_1_dmg_level];
                ZombieApocalypse.GameShopInfo.weapon_1_dmg_level += 1;
            }
        }
    }

    /// <summary>
    /// This function increases the damage of weapon 2.
    /// </summary>
    private void upgradeWeapon2Dmg()
    {
        if (ZombieApocalypse.GameShopInfo.weapon_2_dmg_level < maxUpgradeLevel)
        {
            if (ZombieApocalypse.GameData.coinCounter - (int)bulletScript.bulletDamagePerLevelPrice[ZombieApocalypse.GameShopInfo.weapon_2_dmg_level] >= 0)
            {
                ZombieApocalypse.GameData.coinCounter -= (int)bulletScript.bulletDamagePerLevelPrice[ZombieApocalypse.GameShopInfo.weapon_2_dmg_level];
                ZombieApocalypse.GameShopInfo.weapon_2_dmg_level += 1;
            }
        }
    }

    /// <summary>
    /// This function updates the button text according to the player's shop item level.
    /// </summary>
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

        if (ZombieApocalypse.GameShopInfo.weapon_1_dmg_level < maxUpgradeLevel)
            weapon1DmgPrice.text = bulletScript.bulletDamagePerLevelPrice[ZombieApocalypse.GameShopInfo.weapon_1_dmg_level].ToString();
        else
            weapon1DmgPrice.text = "MAX";

        if (ZombieApocalypse.GameShopInfo.weapon_2_dmg_level < maxUpgradeLevel)
            weapon2DmgPrice.text = bulletScript.bulletDamagePerLevelPrice[ZombieApocalypse.GameShopInfo.weapon_2_dmg_level].ToString();
        else
            weapon2DmgPrice.text = "MAX";
    }
}
