using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuStyle
{
    horizontal, vertical
}

public class GunInventory : MonoBehaviour
{
    [Tooltip("Current weapon gameObject.")]
    public GameObject currentGun;
    public Animator currentHandsAnimator;
    private int currentGunCounter = 0;

    [Tooltip("Put Strings of weapon objects from Resources Folder.")]
    public List<string> gunsIHave = new List<string>();
    [Tooltip("Icons from weapons. (Fetched when you run the game) *MUST HAVE ICONS WITH CORRESPONDING NAMES IN RESOURCES FOLDER*")]
    public Texture[] icons;

    [HideInInspector]
    public float switchWeaponCooldown;

    public WeaponSelection weaponSelection;

    public List<GameObject> instantiatedGuns = new List<GameObject>();

    [Header("Sounds")]
    [Tooltip("Sound of weapon changing.")]
    public AudioSource weaponChanging;

    void Awake()
    {
        SpawnAllWeapons();
        EquipWeapon(0); // Equip the first weapon at start
        if (gunsIHave.Count == 0) print("No guns in the inventory");
    }
    void Update()
    {
        switchWeaponCooldown += 1 * Time.deltaTime;

        if (!ZombieApocalypse.GameStatus.isPaused && switchWeaponCooldown > 1.2f && !Input.GetKey(KeyCode.LeftShift))
        {
            ChangeWeapon();
        }
    }

    /// <summary>
    /// This function creates the player's weapons at the start of the game.
    /// </summary>
    void SpawnAllWeapons()
    {
        foreach (string gunName in gunsIHave)
        {
            GameObject resource = (GameObject)Resources.Load(gunName);
            GameObject gun = Instantiate(resource, transform.position, Quaternion.identity);
            gun.SetActive(false); // Deactivate all weapons initially
            instantiatedGuns.Add(gun);
        }
    }
    
    /// <summary>
    /// This function sets the weapon that is equipped by the player.
    /// </summary>
    /// <param name="index">The weapon's index</param>
    void EquipWeapon(int index)
    {
        if (index >= 0 && index < instantiatedGuns.Count)
        {
            if (currentGun != null)
            {
                currentGun.SetActive(false); // Deactivate the currently equipped gun
            }

            currentGun = instantiatedGuns[index];
            currentGun.SetActive(true); // Activate the newly equipped gun

            AssignHandsAnimator(currentGun);

            currentGunCounter = index;
        }
    }

    
    /// <summary>
    /// This function switches the weapon that is going to be equipped by the player according to the keyboard/mouse input.
    /// </summary>
    void ChangeWeapon()
    {
        if (!ZombieApocalypse.GameStatus.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                switchWeaponCooldown = 0;
                currentGunCounter++;
                if (currentGunCounter > gunsIHave.Count - 1)
                {
                    currentGunCounter = 0;
                }

                StartCoroutine(ChangeWeaponWithDelay(0.8f, currentGunCounter));
                weaponSelection.SelectWeaponChange();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                switchWeaponCooldown = 0;
                currentGunCounter--;
                if (currentGunCounter < 0)
                {
                    currentGunCounter = gunsIHave.Count - 1;
                }

                StartCoroutine(ChangeWeaponWithDelay(0.8f, currentGunCounter));
                weaponSelection.SelectWeaponChange();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && currentGunCounter != 0)
            {
                switchWeaponCooldown = 0;
                currentGunCounter = 0;
                StartCoroutine(ChangeWeaponWithDelay(0.8f, currentGunCounter));

                weaponSelection.SelectWeaponChange();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && currentGunCounter != 1)
            {
                switchWeaponCooldown = 0;
                currentGunCounter = 1;
                StartCoroutine(ChangeWeaponWithDelay(0.8f, currentGunCounter));

                weaponSelection.SelectWeaponChange();
            }
        }
    }


    /// <summary>
    /// This function will play the change weapon animation.
    /// </summary>
    /// <param name="delay">Amount of time delay from changing weapons.</param>
    /// <param name="currentGunCounter">Index of weapons being used.</param>
    /// <returns></returns>
    IEnumerator ChangeWeaponWithDelay(float delay, int currentGunCounter)
    {
        if (weaponChanging)
            weaponChanging.Play();
        else
            print("Missing Weapon Changing music clip.");

        currentHandsAnimator.SetBool("changingWeapon", true);
        yield return new WaitForSeconds(delay);
        AssignHandsAnimator(currentGun);
        EquipWeapon(currentGunCounter);
    }
    /// <summary>
    /// This function manages the animation of the hands when switching or equipping weapon.
    /// and manages how hands will move.
    /// </summary>
    /// <param name="_currentGun">The weapon that will be equippied</param>
    void AssignHandsAnimator(GameObject _currentGun)
    {
        if (_currentGun.name.Contains("Gun"))
        {
            currentHandsAnimator = currentGun.GetComponent<GunScript>().handsAnimator;
        }
    }

    /// <summary>
    /// This function destroys gun equipped when the player is dead.
    /// </summary>
    public void DeadMethod()
    {
        Destroy(currentGun);
        Destroy(this);
    }

}
