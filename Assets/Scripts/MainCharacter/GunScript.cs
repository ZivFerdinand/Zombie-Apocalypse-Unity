﻿using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public enum GunStyles{
	nonautomatic,automatic
}
public class GunScript : MonoBehaviour {
	[Tooltip("Selects type of waepon to shoot rapidly or one bullet per click.")]
	public GunStyles currentStyle;
	[HideInInspector]
	public MouseLookScript mls;

	[Header("Player movement properties")]
	[Tooltip("Speed is determined via gun because not every gun has same properties or weights so you MUST set up your speeds here")]
	public int walkingSpeed = 3;
	[Tooltip("Speed is determined via gun because not every gun has same properties or weights so you MUST set up your speeds here")]
	public int runningSpeed = 5;

	[Header("Bullet properties")]
	[Tooltip("Preset value to tell with how many bullets will our waepon spawn aside.")]
	public float bulletsIHave = 20;
	[Tooltip("Preset value to tell with how much bullets will our waepon spawn inside rifle.")]
	public float bulletsInTheGun = 5;
	private float bulletsI;
	[Tooltip("Preset value to tell how much bullets can one magazine carry.")]
	public float amountOfBulletsPerLoad = 5;

	private Transform player;
	private Camera cameraComponent;
	private Transform gunPlaceHolder;

	private PlayerMovementScript pmS;


    private void Start()
    {
		crosshair = GameObject.Find("Crosshair").GetComponent<Image>();

		if (gameObject.name == "NewGun_semi(Clone)")
		{
			bulletsI = bulletsInTheGun = ZombieApocalypse.GameShopInfo.weapon_1_2;
			bulletsIHave = ZombieApocalypse.GameShopInfo.weapon_1_1;
			bulletsI = 60f;
		}
		else
		{
			bulletsI = bulletsInTheGun = ZombieApocalypse.GameShopInfo.weapon_2_2;
			bulletsIHave = ZombieApocalypse.GameShopInfo.weapon_2_1;
			bulletsI = 30f;
		}
	}
    void Awake(){
		mls = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLookScript>();
		player = mls.transform;
		mainCamera = mls.myCamera;
		secondCamera = GameObject.FindGameObjectWithTag("SecondCamera").GetComponent<Camera>();
		cameraComponent = mainCamera.GetComponent<Camera>();
		pmS = player.GetComponent<PlayerMovementScript>();

		bulletSpawnPlace = GameObject.FindGameObjectWithTag("BulletSpawn");
		hitMarker = transform.Find ("hitMarkerSound").GetComponent<AudioSource> ();

		startLook = mouseSensitvity_notAiming;
		startAim = mouseSensitvity_aiming;
		startRun = mouseSensitvity_running;

		rotationLastY = mls.currentYRotation;
		rotationLastX= mls.currentCameraXRotation;

	}


	[HideInInspector]
	public Vector3 currentGunPosition;
	[Header("Gun Positioning")]
	[Tooltip("Vector 3 position from player SETUP for NON AIMING values")]
	public Vector3 restPlacePosition;
	[Tooltip("Vector 3 position from player SETUP for AIMING values")]
	public Vector3 aimPlacePosition;
	[Tooltip("Time that takes for gun to get into aiming stance.")]
	public float gunAimTime = 0.1f;

	[HideInInspector]
	public bool reloading;

	private Vector3 gunPosVelocity;
	private float cameraZoomVelocity;
	private float secondCameraZoomVelocity;

	private Vector2 gunFollowTimeVelocity;

	void Update(){

		Animations();

		GiveCameraScriptMySensitvity();

		PositionGun();

		Shooting();
		MeeleAttack();
		LockCameraWhileMelee ();

		Sprint(); //iff we have the gun you sprint from here, if we are gunless then its called from movement script

		CrossHairFadeout();

		if (gameObject.name == "NewGun_semi(Clone)")
		{
			ZombieApocalypse.GameShopInfo.weapon_1_2 = bulletsInTheGun;
			ZombieApocalypse.GameShopInfo.weapon_1_1 = bulletsIHave;
		}
		else
		{
			ZombieApocalypse.GameShopInfo.weapon_2_2 = bulletsInTheGun;
			ZombieApocalypse.GameShopInfo.weapon_2_1 = bulletsIHave;
		}
	}

	void FixedUpdate(){
		RotationGun ();

		MeeleAnimationsStates ();

		/*
		 * Changing some values if we are aiming, like sensitity, zoom racion and position of the waepon.
		 */
		//if aiming
		if(!ZombieApocalypse.GameStatus.isPaused && Input.GetAxis("Fire2") != 0 && !reloading && !meeleAttack){
			gunPrecision = gunPrecision_aiming;
			recoilAmount_x = recoilAmount_x_;
			recoilAmount_y = recoilAmount_y_;
			recoilAmount_z = recoilAmount_z_;
			currentGunPosition = Vector3.SmoothDamp(currentGunPosition, aimPlacePosition, ref gunPosVelocity, gunAimTime);
			cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_aiming, ref cameraZoomVelocity, gunAimTime);
			secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_aiming, ref secondCameraZoomVelocity, gunAimTime);
		}
		//if not aiming
		else{
			gunPrecision = gunPrecision_notAiming;
			recoilAmount_x = recoilAmount_x_non;
			recoilAmount_y = recoilAmount_y_non;
			recoilAmount_z = recoilAmount_z_non;
			currentGunPosition = Vector3.SmoothDamp(currentGunPosition, restPlacePosition, ref gunPosVelocity, gunAimTime);
			cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_notAiming, ref cameraZoomVelocity, gunAimTime);
			secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_notAiming, ref secondCameraZoomVelocity, gunAimTime);
		}

	}

	[Header("Sensitvity of the gun")]
	[Tooltip("Sensitvity of this gun while not aiming.")]
	public float mouseSensitvity_notAiming = 10;
	//[HideInInspector]
	[Tooltip("Sensitvity of this gun while aiming.")]
	public float mouseSensitvity_aiming = 5;
	//[HideInInspector]
	[Tooltip("Sensitvity of this gun while running.")]
	public float mouseSensitvity_running = 4;



	/// <summary>
	/// Used to give our main camera different sensivity options for each gun.
	/// </summary>
	void GiveCameraScriptMySensitvity(){
		mls.mouseSensitvity_notAiming = mouseSensitvity_notAiming;
		mls.mouseSensitvity_aiming = mouseSensitvity_aiming;
	}

	/// <summary>
	/// Used to expand position of the crosshair or make it dissapear when running.
	/// </summary>
	void CrossHairFadeout(){
        if (!ZombieApocalypse.GameStatus.isPaused && player.GetComponent<Rigidbody>().velocity.magnitude > 1 && Input.GetAxis("Fire1") == 0)
        {
			//if not shooting

			if (player.GetComponent<PlayerMovementScript>().maxSpeed < runningSpeed)
            { 
				//not running
				if (crosshair.color.a == 0)
				{
					StartCoroutine(CustomFadeAnimator.Fade(crosshair, 0, 1, 0.25f));
				}
            }
            else
            {
				//running
                if (crosshair.color.a == 1)
				{
					StartCoroutine(CustomFadeAnimator.Fade(crosshair, 1, 0, 0.25f));
				}
            }
        }
    }

	/// <summary>
	/// Changes the max speed that player is allowed to go.
	/// Also max speed is connected to the animator which will trigger the run animation.
	/// </summary>
	void Sprint()
	{
		// Running();  so developer can find it with CTRL+F
		if (Input.GetAxis ("Vertical") > 0 && Input.GetAxisRaw ("Fire2") == 0 && meeleAttack == false && Input.GetAxisRaw ("Fire1") == 0) {
			if (!ZombieApocalypse.GameStatus.isPaused && Input.GetKey (KeyCode.LeftShift)) {
					pmS.maxSpeed = runningSpeed;
			}
			else
            {

				pmS.maxSpeed = walkingSpeed;
			}
		
		} else {
			pmS.maxSpeed = walkingSpeed;
		}

	}

	[HideInInspector]
	public bool meeleAttack;
	[HideInInspector]
	public bool aiming;

	/// <summary>
	/// Checking if meeleAttack is already running.
	/// If we are not reloading we can trigger the MeeleAttack animation from the IENumerator.
	/// </summary>
	void MeeleAnimationsStates(){
		if (handsAnimator) {
			meeleAttack = handsAnimator.GetCurrentAnimatorStateInfo (0).IsName (meeleAnimationName);
			aiming = handsAnimator.GetCurrentAnimatorStateInfo (0).IsName (aimingAnimationName);	
		}
	}
	
	/// <summary>
	/// User inputs meele attack with Q in keyboard start the coroutine for animation and damage attack.
	/// </summary>
	void MeeleAttack(){	

		if(!ZombieApocalypse.GameStatus.isPaused && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) && !meeleAttack){
            StartCoroutine("AnimationMeeleAttack");
        }
    }

	/// <summary>
	/// Sets meele animation to play.
	/// </summary>
	/// <returns></returns>
	IEnumerator AnimationMeeleAttack(){
		handsAnimator.SetBool("meeleAttack",true);
		//yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.1f);
		handsAnimator.SetBool("meeleAttack",false);
	}

	private float startLook, startAim, startRun;

	/// <summary>
	/// Setting the mouse sensitvity lower when meele attack and waits till it ends.
	/// </summary>
	void LockCameraWhileMelee(){
		if (meeleAttack) {
			mouseSensitvity_notAiming = 2;
			mouseSensitvity_aiming = 1.6f;
			mouseSensitvity_running = 1;
		} else {
			mouseSensitvity_notAiming = startLook;
			mouseSensitvity_aiming = startAim;
			mouseSensitvity_running = startRun;
		}
	}


	private Vector3 velV;
	[HideInInspector]
	public Transform mainCamera;
	private Camera secondCamera;

	/// <summary>
	/// Calculating the weapon position accordingly to the player position and rotation.
	/// After calculation the recoil amount are decreased to 0.
	/// </summary>
	void PositionGun(){
		transform.position = Vector3.SmoothDamp(transform.position,
			mainCamera.transform.position  - 
			(mainCamera.transform.right * (currentGunPosition.x + currentRecoilXPos)) + 
			(mainCamera.transform.up * (currentGunPosition.y+ currentRecoilYPos)) + 
			(mainCamera.transform.forward * (currentGunPosition.z + currentRecoilZPos)),ref velV, 0);



		pmS.cameraPosition = new Vector3(currentRecoilXPos,currentRecoilYPos, 0);

		currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref velocity_z_recoil, recoilOverTime_z);
		currentRecoilXPos = Mathf.SmoothDamp(currentRecoilXPos, 0, ref velocity_x_recoil, recoilOverTime_x);
		currentRecoilYPos = Mathf.SmoothDamp(currentRecoilYPos, 0, ref velocity_y_recoil, recoilOverTime_y);

	}


	[Header("Rotation")]
	private Vector2 velocityGunRotate;
	private float gunWeightX,gunWeightY;
	[Tooltip("The time waepon will lag behind the camera view best set to '0'.")]
	public float rotationLagTime = 0f;
	private float rotationLastY;
	private float rotationDeltaY;
	private float angularVelocityY;
	private float rotationLastX;
	private float rotationDeltaX;
	private float angularVelocityX;
	[Tooltip("Value of forward rotation multiplier.")]
	public Vector2 forwardRotationAmount = Vector2.one;

	/// <summary>
	/// Rotating the weapon according to mouse look rotation.
	/// Calculating the forawrd rotation like in Call Of Duty weapon weight.
	/// </summary>
	void RotationGun(){

		rotationDeltaY = mls.currentYRotation - rotationLastY;
		rotationDeltaX = mls.currentCameraXRotation - rotationLastX;

		rotationLastY= mls.currentYRotation;
		rotationLastX= mls.currentCameraXRotation;

		angularVelocityY = Mathf.Lerp (angularVelocityY, rotationDeltaY, Time.deltaTime * 5);
		angularVelocityX = Mathf.Lerp (angularVelocityX, rotationDeltaX, Time.deltaTime * 5);

		gunWeightX = Mathf.SmoothDamp (gunWeightX, mls.currentCameraXRotation, ref velocityGunRotate.x, rotationLagTime);
		gunWeightY = Mathf.SmoothDamp (gunWeightY, mls.currentYRotation, ref velocityGunRotate.y, rotationLagTime);

		transform.rotation = Quaternion.Euler (gunWeightX + (angularVelocityX*forwardRotationAmount.x), gunWeightY + (angularVelocityY*forwardRotationAmount.y), 0);
	}

	private float currentRecoilZPos;
	private float currentRecoilXPos;
	private float currentRecoilYPos;

	/// <summary>
	/// Called from ShootMethod();, upon shooting the recoil amount will increase.
	/// </summary>
	public void RecoilMath(){
		currentRecoilZPos -= recoilAmount_z;
		currentRecoilXPos -= (UnityEngine.Random.value - 0.5f) * recoilAmount_x;
		currentRecoilYPos -= (UnityEngine.Random.value - 0.5f) * recoilAmount_y;
		mls.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision);
		mls.wantedYRotation -= (currentRecoilXPos * gunPrecision);		 
	}

	[Header("Shooting setup - MUSTDO")]
	[HideInInspector] public GameObject bulletSpawnPlace;
	[Tooltip("Bullet prefab that this waepon will shoot.")]
	public GameObject bullet;
	[Tooltip("Rounds per second if weapon is set to automatic rafal.")]
	public float roundsPerSecond;
	private float waitTillNextFire;

	/// <summary>
	/// Checking if the gun is automatic or nonautomatic and accordingly runs the ShootMethod();.
	/// </summary>
	void Shooting(){

		if (!ZombieApocalypse.GameStatus.isPaused && !meeleAttack) {
			if (currentStyle == GunStyles.nonautomatic) {
				if (Input.GetButtonDown ("Fire1")) {
					ShootMethod ();
				}
			}
			if (currentStyle == GunStyles.automatic) {
				if (Input.GetButton ("Fire1")) {
					ShootMethod ();
				}
			}
		}
		waitTillNextFire -= roundsPerSecond * Time.deltaTime;
	}


	[HideInInspector]	public float recoilAmount_z = 0.5f;
	[HideInInspector]	public float recoilAmount_x = 0.5f;
	[HideInInspector]	public float recoilAmount_y = 0.5f;
	[Header("Recoil Not Aiming")]
	[Tooltip("Recoil amount on that AXIS while NOT aiming")]
	public float recoilAmount_z_non = 0.5f;
	[Tooltip("Recoil amount on that AXIS while NOT aiming")]
	public float recoilAmount_x_non = 0.5f;
	[Tooltip("Recoil amount on that AXIS while NOT aiming")]
	public float recoilAmount_y_non = 0.5f;
	[Header("Recoil Aiming")]
	[Tooltip("Recoil amount on that AXIS while aiming")]
	public float recoilAmount_z_ = 0.5f;
	[Tooltip("Recoil amount on that AXIS while aiming")]
	public float recoilAmount_x_ = 0.5f;
	[Tooltip("Recoil amount on that AXIS while aiming")]
	public float recoilAmount_y_ = 0.5f;
	[HideInInspector]public float velocity_z_recoil,velocity_x_recoil,velocity_y_recoil;
	[Header("")]
	[Tooltip("The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
	public float recoilOverTime_z = 0.5f;
	[Tooltip("The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
	public float recoilOverTime_x = 0.5f;
	[Tooltip("The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
	public float recoilOverTime_y = 0.5f;

	[Header("Gun Precision")]
	[Tooltip("Gun rate precision when player is not aiming. THis is calculated with recoil.")]
	public float gunPrecision_notAiming = 200.0f;
	[Tooltip("Gun rate precision when player is aiming. THis is calculated with recoil.")]
	public float gunPrecision_aiming = 100.0f;
	[Tooltip("FOV of first camera when NOT aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
	public float cameraZoomRatio_notAiming = 60;
	[Tooltip("FOV of first camera when aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
	public float cameraZoomRatio_aiming = 40;
	[Tooltip("FOV of second camera when NOT aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
	public float secondCameraZoomRatio_notAiming = 60;
	[Tooltip("FOV of second camera when aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
	public float secondCameraZoomRatio_aiming = 40;
	[HideInInspector]
	public float gunPrecision;

	[Tooltip("Audios for shootingSound, and reloading.")]
	public AudioSource shoot_sound_source, reloadSound_source;
	[Tooltip("Sound that plays after successful attack bullet hit.")]
	public static AudioSource hitMarker;

	/// <summary>
	/// Sounds that is called upon hitting the target.
	/// </summary>
	public static void HitMarkerSound(){
		hitMarker.volume = ZombieApocalypse.GameStatus.sfxValue;
    
		hitMarker.Play();
	}

    [Tooltip("Array of muzzel flashes, randmly one will appear after each bullet.")]
	public GameObject[] muzzelFlash;
	[Tooltip("Place on the gun where muzzel flash will appear.")]
	public GameObject muzzelSpawn;
	private GameObject holdFlash;
	private GameObject holdSmoke;

	/// <summary>
	/// Called from Shooting();
	/// Creates bullets and muzzle flashes and calls for Recoil.
	/// </summary>
	private void ShootMethod(){
		if(waitTillNextFire <= 0 && !reloading && pmS.maxSpeed < 5){

			if(bulletsInTheGun > 0){

				int randomNumberForMuzzelFlash = UnityEngine.Random.Range(0,5);
				if (bullet)
					Instantiate (bullet, bulletSpawnPlace.transform.position, bulletSpawnPlace.transform.rotation);
				else
					print ("Missing the bullet prefab");
				holdFlash = Instantiate(muzzelFlash[randomNumberForMuzzelFlash], muzzelSpawn.transform.position /*- muzzelPosition*/, muzzelSpawn.transform.rotation * Quaternion.Euler(0,0,90) ) as GameObject;
				holdFlash.transform.parent = muzzelSpawn.transform;
				if (shoot_sound_source)
				{
					shoot_sound_source.volume = ZombieApocalypse.GameStatus.sfxValue;
					shoot_sound_source.Play();
				}
				else
					print("Missing 'Shoot Sound Source'.");

				RecoilMath();

				waitTillNextFire = 1;
				bulletsInTheGun -= 1;
            }
				
			else{
				StartCoroutine("Reload_Animation");
			}

		}

	}


/// <summary>/// /// /// </summary>
	/// <summary>
	/// Reloading, setting the reloading to animator,
	/// Waiting for 2 seconds and then seeting the reloaded clip.
	/// </summary>
	
	[Header("reload time after anima")]
	[Tooltip("Time that passes after reloading. Depends on your reload animation length, because reloading can be interrupted via meele attack or running. So any action before this finishes will interrupt reloading.")]
	public float reloadChangeBulletsTime;
	IEnumerator Reload_Animation(){
		if(bulletsIHave > 0 && bulletsInTheGun < amountOfBulletsPerLoad && !reloading/* && !aiming*/){

			if (reloadSound_source.isPlaying == false && reloadSound_source != null) {
				if (reloadSound_source)
				{reloadSound_source.volume = ZombieApocalypse.GameStatus.sfxValue;
             
					reloadSound_source.Play();
				}
				else
					print("'Reload Sound Source' missing.");
			}
		

			handsAnimator.SetBool("reloading",true);
			yield return new WaitForSeconds(0.5f);
			handsAnimator.SetBool("reloading",false);



			yield return new WaitForSeconds (reloadChangeBulletsTime - 0.5f);//minus ovo vrijeme cekanja na yield
			if (meeleAttack == false && pmS.maxSpeed != runningSpeed) {
				//print ("tu sam");
				if (player.GetComponent<PlayerMovementScript>()._freakingZombiesSound)
				{
					player.GetComponent<PlayerMovementScript>()._freakingZombiesSound.volume = ZombieApocalypse.GameStatus.sfxValue;
					player.GetComponent<PlayerMovementScript>()._freakingZombiesSound.Play();
				}
				else
					print("Missing Freaking Zombies Sound");
				
				if (bulletsIHave - amountOfBulletsPerLoad >= 0) {
					bulletsIHave -= amountOfBulletsPerLoad - bulletsInTheGun;
					bulletsInTheGun = amountOfBulletsPerLoad;
				} else if (bulletsIHave - amountOfBulletsPerLoad < 0) {
					float valueForBoth = amountOfBulletsPerLoad - bulletsInTheGun;
					if (bulletsIHave - valueForBoth < 0) {
						bulletsInTheGun += bulletsIHave;
						bulletsIHave = 0;
					} else {
						bulletsIHave -= valueForBoth;
						bulletsInTheGun += valueForBoth;
					}
				}
			} else {
				reloadSound_source.Stop ();

				print ("Reload interrupted via meele attack");
			}

		}
	}


	/// <summary>
	/// Setting the number of bullets to the hud UI gameobject if there is one.
	/// And drawing CrossHair from here.
	/// </summary>
	[Tooltip("HUD bullets to display bullet count on screen. Will be find under name 'HUD_bullets' in scene.")]
	public TextMeshProUGUI HUD_bullets_left;
    public TextMeshProUGUI HUD_bullets_right;
    void OnGUI(){
		if(!HUD_bullets_left){
			try{
				HUD_bullets_left = GameObject.Find("HUD_bullets_left").GetComponent<TextMeshProUGUI>();
			}
			catch(System.Exception ex){
				print("Couldnt find the HUD_Bullets ->" + ex.StackTrace.ToString());
			}
		}
		if(mls && HUD_bullets_left)
			HUD_bullets_left.text = bulletsInTheGun.ToString();

        if (!HUD_bullets_right)
        {
            try
            {
                HUD_bullets_right = GameObject.Find("HUD_bullets_right").GetComponent<TextMeshProUGUI>();
            }
            catch (System.Exception ex)
            {
                print("Couldnt find the HUD_Bullets ->" + ex.StackTrace.ToString());
            }
        }
        if (mls && HUD_bullets_right)
            HUD_bullets_right.text = ((int) (bulletsIHave / bulletsI)).ToString();

        DrawCrosshair();
	}


	[Header("Crosshair properties")]
	private Image crosshair;

	/// <summary>
	/// Draws Crosshair.
	/// </summary>
    void DrawCrosshair(){
		if(!ZombieApocalypse.GameStatus.isPaused && Input.GetAxis("Fire2") == 0){//if not aiming draw
            crosshair.gameObject.SetActive(true);
            //GUI.DrawTexture(new Rect(Screen.width/2 - vec2(size_crosshair).x/2, Screen.height/2 - vec2(size_crosshair).y/2, vec2(size_crosshair).x, vec2(size_crosshair).y), dotCrosshair);
		}
		else
		{
			crosshair.gameObject.SetActive(false);
		}
	}

	public Animator handsAnimator;

	/// <summary>
	/// Fetching if any current animation is running.
	/// Setting the reload animation upon pressing R.
	/// </summary>
	void Animations(){

		if(handsAnimator){

			reloading = handsAnimator.GetCurrentAnimatorStateInfo(0).IsName(reloadAnimationName);

			handsAnimator.SetFloat("walkSpeed",pmS.currentSpeed);
			handsAnimator.SetBool("aiming", Input.GetButton("Fire2"));
			handsAnimator.SetInteger("maxSpeed", pmS.maxSpeed);
			if(!ZombieApocalypse.GameStatus.isPaused && Input.GetKeyDown(KeyCode.R) && pmS.maxSpeed < 5 && !reloading && !meeleAttack/* && !aiming*/){
				StartCoroutine("Reload_Animation");
			}
		}

	}

	[Header("Animation names")]
	public string reloadAnimationName = "Player_Reload";
	public string aimingAnimationName = "Player_AImpose";
	public string meeleAnimationName = "Character_Malee";
}
