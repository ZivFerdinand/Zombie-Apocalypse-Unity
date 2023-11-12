using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Model;

public class WeaponController : MonoBehaviour
{
    private InputManager inputManager;
    
    [Header("Settings")]
    public WeaponSettingsModel settings;

    bool isInitialized;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    // Start is called before the first frame update
    void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }

    public void Initialize(InputManager InputManager)
    {
        inputManager = InputManager;
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        targetWeaponRotation.y += settings.SwayAmount * (settings.SwayXInverted ? -inputManager.input_View.x : inputManager.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += settings.SwayAmount * (settings.SwayYInverted ? inputManager.input_View.y : -inputManager.input_View.y) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(newWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(newWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
        //newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothing);

        transform.localRotation = Quaternion.Euler(newWeaponRotation);
    }
}
