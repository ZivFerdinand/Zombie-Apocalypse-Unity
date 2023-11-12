using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerinput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;

    [HideInInspector]
    public Vector2 input_View;

    [Header("Weapon")]
    public WeaponController currentWeapon;

    // Start is called before the first frame update
    void Awake()
    {
        playerinput = new PlayerInput();
        onFoot = playerinput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        onFoot.Jump.performed += ctx
             => motor.jump();
        onFoot.Crouch.performed += ctx
            => motor.Crouch();
        onFoot.Sprint.performed += ctx
            => motor.Sprint();

        if (currentWeapon)
        {
            currentWeapon.Initialize(this);
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();            
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
