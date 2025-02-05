using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SkillScript : MonoBehaviour
{
    private Color fireColor = new Color(1, 0.33f, 0.33f);
    private Color iceColor = new Color(0.35f, 0.64f, 1f);
    private Color idleColor = new Color(1, 1, 1, 0.35f);
    
    private float[] aimingTimePerLevel = { 1f, 1.5f, 2f, 2.5f, 3f, 4f };
    public float[] aimingTimePerLevelPrice = { 100f, 200f, 300f, 400f, 500f };

    public GameObject[] Prefabs;
    public GameObject aimingStatus;
    public TextMeshProUGUI skillTimeText;
    public Slider sliderSkill;
    public int currentPrefabIndex;
    public Image[] iceBars, fireBars;

    private GameObject currentPrefabObject;
    private FireBaseScript currentPrefabScript;
    
    private float aimingTimeLeft;
    private bool isAimingSkill;
    private int iceCount;
    private int fireCount;
    private bool isFireSkill;
    void Start()
    {
        iceCount = fireCount = 2;
        isAimingSkill = isFireSkill = false;
    }

    void Update()
    {
        aimingStatus.SetActive(isAimingSkill);
        if (isAimingSkill)
        {
            if (!isFireSkill)
                sliderSkill.value = aimingTimeLeft / aimingTimePerLevel[ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level];
            else
                sliderSkill.value = aimingTimeLeft / aimingTimePerLevel[ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level];

            aimingTimeLeft -= Time.deltaTime / 0.25f;
            if (aimingTimeLeft > 0)
            {
                skillTimeText.text = aimingTimeLeft.ToString("F2") + "s";
            }
            else
            {
                skillTimeText.text = "";
            }
        }
        else
        {
            skillTimeText.text = "";
        }
        if(aimingTimeLeft < 0 && isAimingSkill)
        {
            StartCurrent();
        }
        if(Input.GetKeyDown(KeyCode.Escape) && !ZombieApocalypse.GameStatus.isPaused && isAimingSkill)
        {
            StartCurrent();
        }
        UpdateEffect();
    }
    /// <summary>
    /// This function will update the skill bar when the skill potion is obtained.
    /// </summary>
    /// <param name="idx">Index that represents the skills that acquired.</param>
    public void getSkill(int idx)
    {
        if(idx == 0)
        {
            fireCount++;
            fireCount = Mathf.Min(fireCount, 2);
        }
        else if(idx == 1)
        {
            iceCount++;
            iceCount = Mathf.Min(iceCount, 2);
        }
        updateSkillBar();
    }
    /// <summary>
    /// This function will manages the usage of special skills (fire and ice) based on user input.
    /// Checks for key presses to activate and release skills, deducting skill counts and updating the skill bar accordingly.
    /// Adjusts audio volume based on the global sound effects value. Handles aiming and starting the current skill.
    /// </summary>
    private void UpdateEffect()
    {
        if (fireCount > 0 && Input.GetKeyDown(KeyCode.E) && !ZombieApocalypse.GameStatus.isPaused)
        {
            fireCount--;
            updateSkillBar();
            currentPrefabIndex = 0;
            Prefabs[currentPrefabIndex].GetComponent<AudioSource>().volume = ZombieApocalypse.GameStatus.sfxValue;
            SetAiming('E');
        }
        else if(Input.GetKeyUp(KeyCode.E) && isAimingSkill && !ZombieApocalypse.GameStatus.isPaused)
        {
            StartCurrent();
        }
        if (iceCount > 0 && Input.GetKeyDown(KeyCode.Q) && !ZombieApocalypse.GameStatus.isPaused)
        {
            iceCount--;
            updateSkillBar();
            currentPrefabIndex = 1;
            Prefabs[currentPrefabIndex].GetComponent<AudioSource>().volume = ZombieApocalypse.GameStatus.sfxValue;
            SetAiming('Q');
        }
        else if (Input.GetKeyUp(KeyCode.Q) && isAimingSkill && !ZombieApocalypse.GameStatus.isPaused)
        {
            StartCurrent();

        }
    }
    /// <summary>
    /// This function will update the skill bar after the player uses the skill.
    /// </summary>
    private void updateSkillBar()
    {
        if(iceCount <= 0)
        {
            iceBars[0].color = iceBars[1].color = idleColor;
        }
        else if(iceCount == 1)
        {
            iceBars[0].color = iceColor;
            iceBars[1].color = idleColor;
        }
        else
        {
            iceBars[0].color = iceBars[1].color = iceColor;
        }

        if (fireCount <= 0)
        {
            fireBars[0].color = fireBars[1].color = idleColor;
        }
        else if (fireCount == 1)
        {
            fireBars[0].color = fireColor;
            fireBars[1].color = idleColor;
        }
        else
        {
            fireBars[0].color = fireBars[1].color = fireColor;
        }
    }
    /// <summary>
    /// This function will issue the effect of the skill used.
    /// </summary>
    private void BeginEffect()
    {
        Vector3 pos;
        float yRot = Camera.main.transform.rotation.eulerAngles.y;
        Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
        Quaternion rotation = Quaternion.identity;
        currentPrefabObject = GameObject.Instantiate(Prefabs[currentPrefabIndex]);
        currentPrefabScript = currentPrefabObject.GetComponent<FireConstantBaseScript>();
        
        if (currentPrefabScript == null)
        {
            // temporary effect, like a fireball
            currentPrefabScript = currentPrefabObject.GetComponent<FireBaseScript>();
            if (currentPrefabScript.IsProjectile)
            {
                // Set the start point slightly below the camera
                pos = Camera.main.transform.position - (Camera.main.transform.up * 0.3f);
                rotation = Camera.main.transform.rotation;

                // Apply forward and right offsets (adjust these based on desired positioning)
                pos += Camera.main.transform.forward * 0.5f;
                pos += Camera.main.transform.right * 0.1f;
            }
            else
            {
                // set the start point in front of the player a ways
                pos = Camera.main.transform.position + (forwardY * 10.0f);
            }
        }
        else
        {
            // set the start point in front of the player a ways, rotated the same way as the player
            pos = Camera.main.transform.position + (forwardY * 5.0f);
            rotation = Camera.main.transform.rotation;
            pos.y = 0.0f;
        }

        FireProjectileScript projectileScript = currentPrefabObject.GetComponentInChildren<FireProjectileScript>();
        if (projectileScript != null)
        {
            // make sure we don't collide with other fire layers
            projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FireLayer"));
        }

        currentPrefabObject.transform.position = pos;
        currentPrefabObject.transform.rotation = rotation;
    }
    /// <summary>
    /// This function will provide an aiming effect when using the skill.
    /// </summary>
    /// <summary>
    /// This function controls the player's aiming state.
    /// </summary>
    public void SetAiming(char skill)
    {
        isAimingSkill = true;
        Time.timeScale = 0.25f;
        if (skill == 'Q')
        {
            isFireSkill = false;
            aimingTimeLeft = aimingTimePerLevel[ZombieApocalypse.GameShopInfo.skill_1_aim_duration_level];
        }
        else
        {
            isFireSkill = true;
            aimingTimeLeft = aimingTimePerLevel[ZombieApocalypse.GameShopInfo.skill_2_aim_duration_level];
        }
    }
    /// <summary>
    /// This function will start the current effect.
    /// </summary>

    /// <summary>
    /// This function will start the player's current skill.
    /// </summary>
    public void StartCurrent()
    {
        Time.timeScale = 1f;
        isAimingSkill = false;
        StopCurrent();
        BeginEffect();
    }
    /// <summary>
    /// This function will stop the current effect.
    /// </summary>
    /// <summary>
    /// This function will stop the player's current skill.
    /// </summary>
    private void StopCurrent()
    {
        // if we are running a constant effect like wall of fire, stop it now
        if (currentPrefabScript != null && currentPrefabScript.Duration > 10000)
        {
            currentPrefabScript.Stop();
        }
        currentPrefabObject = null;
        currentPrefabScript = null;
    }
    
}
