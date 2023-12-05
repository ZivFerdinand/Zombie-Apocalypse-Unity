using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SkillScript : MonoBehaviour
{
    private Color fireColor = new Color(1, 0.33f, 0.33f);
    private Color iceColor = new Color(0f, 0.54f, 0f);
    private Color idleColor = new Color(1, 1, 1, 0.35f);
    private const float initAimingTime = 5f;

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
    void Start()
    {
        iceCount = fireCount = 2;
        isAimingSkill = false;
    }

    void Update()
    {
        aimingStatus.SetActive(isAimingSkill);
        if (isAimingSkill)
        {
            sliderSkill.value = aimingTimeLeft / initAimingTime;
            aimingTimeLeft -= Time.deltaTime / 0.25f;
            if(aimingTimeLeft > 0)
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
    private void UpdateEffect()
    {
        if (fireCount > 0 && Input.GetKeyDown(KeyCode.E) && !ZombieApocalypse.GameStatus.isPaused)
        {
            fireCount--;
            updateSkillBar();
            currentPrefabIndex = 0;
            Prefabs[currentPrefabIndex].GetComponent<AudioSource>().volume = ZombieApocalypse.GameStatus.sfxValue;
            SetAiming();
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
            SetAiming();
        }
        else if (Input.GetKeyUp(KeyCode.Q) && isAimingSkill && !ZombieApocalypse.GameStatus.isPaused)
        {
            StartCurrent();

        }
        

    }
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

    public void SetAiming()
    {
        isAimingSkill = true;
        Time.timeScale = 0.25f;
        aimingTimeLeft = initAimingTime;
    }
    public void StartCurrent()
    {
        Time.timeScale = 1f;
        isAimingSkill = false;
        StopCurrent();
        BeginEffect();
    }

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
