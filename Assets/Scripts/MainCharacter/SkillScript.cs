using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SkillScript : MonoBehaviour
{
    private const float initAimingTime = 5f;

    public GameObject[] Prefabs;
    public GameObject customCrosshair;
    public TextMeshProUGUI skillTimeText;

    private GameObject currentPrefabObject;
    private FireBaseScript currentPrefabScript;
    private int currentPrefabIndex;
    
    private float aimingTimeLeft;
    private bool isAimingSkill;
    void Start()
    {
        isAimingSkill = false;
    }

    void Update()
    {
        customCrosshair.SetActive(isAimingSkill);
        if (isAimingSkill)
        {
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

    private void UpdateEffect()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPrefabIndex = 0;
            SetAiming();
        }
        else if(Input.GetKeyUp(KeyCode.E) && isAimingSkill)
        {
            StartCurrent();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPrefabIndex = 1;
            SetAiming();
        }
        else if (Input.GetKeyUp(KeyCode.Q) && isAimingSkill)
        {
            StartCurrent();

        }
        else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            NextPrefab();
        }
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            PreviousPrefab();
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

    public void NextPrefab()
    {
        currentPrefabIndex++;
        if (currentPrefabIndex == Prefabs.Length)
        {
            currentPrefabIndex = 0;
        }
    }

    public void PreviousPrefab()
    {
        currentPrefabIndex--;
        if (currentPrefabIndex == -1)
        {
            currentPrefabIndex = Prefabs.Length - 1;
        }
    }
}
