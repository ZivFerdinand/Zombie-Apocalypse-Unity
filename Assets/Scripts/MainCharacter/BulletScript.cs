using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    [Header("Bullet Logic")]
    [Tooltip("Furthest distance bullet will look for target")]
	public float maxDistance = 1000000;
	RaycastHit hit;
	[Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
	public GameObject decalHitWall;
	[Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
	public float floatInfrontOfWall;
	[Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
	public GameObject bloodEffect;
	[Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
	public LayerMask ignoreLayer;

	private float[] bulletDamagePerLevel = { 1f, 1.5f, 2f, 2.5f, 3f, 4f };
    public float[] bulletDamagePerLevelPrice = { 100f, 200f, 300f, 400f, 500f };

    void Update () {
		if (Physics.Raycast(transform.position, transform.forward,out hit, maxDistance, ~ignoreLayer))
		{
			if (decalHitWall)
			{
				if (hit.transform.tag == "LevelPart")
				{
					Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
					Destroy(gameObject);
				}
				if (hit.transform.tag == "Zombie")
				{
					if (ZombieApocalypse.GameData.currentWeapon == 1)
					{
						Debug.Log(bulletDamagePerLevel[ZombieApocalypse.GameShopInfo.weapon_2_dmg_level] + " berkurang Auto");
                        Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
						hit.transform.GetComponentInParent<ZombieMovement>().decreaseHealth(bulletDamagePerLevel[ZombieApocalypse.GameShopInfo.weapon_2_dmg_level]);
                        Destroy(gameObject);
                    } 
					else
					{
                        Debug.Log(bulletDamagePerLevel[ZombieApocalypse.GameShopInfo.weapon_1_dmg_level] + " berkurang Sniper");
                        Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                        hit.transform.GetComponentInParent<ZombieMovement>().decreaseHealth(bulletDamagePerLevel[ZombieApocalypse.GameShopInfo.weapon_1_dmg_level]);
                        Destroy(gameObject);
                    }
				}
			}		
			Destroy(gameObject);
		}
		Destroy(gameObject, 0.1f);
	}
}
