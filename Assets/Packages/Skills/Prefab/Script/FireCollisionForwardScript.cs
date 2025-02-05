﻿using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    public interface ICollisionHandler
    {
        void HandleCollision(GameObject obj, Collision c);
    }

    /// <summary>
    /// This script simply allows forwarding collision events for the objects that collide with something. This
    /// allows you to have a generic collision handler and attach a collision forwarder to your child objects.
    /// In addition, you also get access to the game object that is colliding, along with the object being
    /// collided into, which is helpful.
    /// </summary>
    public class FireCollisionForwardScript : MonoBehaviour
    {
        private SkillScript skillScript;
        public ICollisionHandler CollisionHandler;
        private void Start()
        {
            skillScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SkillScript>();   
        }
        public void OnCollisionEnter(Collision col)
        {
            CollisionHandler.HandleCollision(gameObject, col);
            
                if (col.gameObject.tag == "Zombie")
                {
                if (col.gameObject.GetComponent<ZombieMovement>() != null)
                {
                    if (skillScript.currentPrefabIndex==0)
                    {

                        col.gameObject.GetComponent<ZombieMovement>().decreaseHealth(50);
                    }
                    else
                    {
                        col.gameObject.GetComponent<ZombieMovement>().slowSpeed();
                    }
                }   
                }
        }
    }
}
