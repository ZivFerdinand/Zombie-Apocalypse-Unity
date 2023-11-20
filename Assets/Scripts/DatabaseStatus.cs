using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieApocalypse
{
    public static class DatabaseStatus
    {
        public static bool isPaused
        {
            set
            {
                PlayerPrefs.SetInt("isPaused", (value == true) ? 1 : 0);
            }
            get
            {
                int temp = PlayerPrefs.GetInt("isPaused", 0);
                return (temp == 1) ? true : false;
            }
        }
        
    }
}