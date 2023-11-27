using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieApocalypse
{
    public static class GameData
    {
        public static int gameScore
        {
            set
            {
                PlayerPrefs.SetInt("gameScore", value);

            }
            get
            {
                int temp = PlayerPrefs.GetInt("gameScore", 0);
                return temp;
            }
        }
    }
    public static class GameStatus
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
        public static int gameMode
        {
            set
            {
                PlayerPrefs.SetInt("gameMode", value);

            }
            get
            {
                int temp = PlayerPrefs.GetInt("gameMode", 0);
                return temp;
            }
        }
    }
}