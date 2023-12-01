using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieApocalypse
{
    public static class GameShopInfo
    {
        public static float weapon_1_1
        {
            set
            {
                PlayerPrefs.SetFloat("weapon_1_1", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("weapon_1_1", 1800f);
                return temp;
            }
        }
        public static float weapon_1_2
        {
            set
            {
                PlayerPrefs.SetFloat("weapon_1_2", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("weapon_1_2", 60f);
                return temp;
            }
        }
        public static float weapon_2_1
        {
            set
            {
                PlayerPrefs.SetFloat("weapon_2_1", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("weapon_2_1", 900f);
                return temp;
            }
        }
        public static float weapon_2_2
        {
            set
            {
                PlayerPrefs.SetFloat("weapon_2_2", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("weapon_2_2", 30f);
                return temp;
            }
        }
    }
    public static class GameData
    {
        public static string playerName
        {
            set
            {
                PlayerPrefs.SetString("playerName", value);

            }
            get
            {
                return PlayerPrefs.GetString("playerName", null);
            }
        }
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
        public static float currentMultiplier
        {
            set
            {
                PlayerPrefs.SetFloat("currentMultiplier", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("currentMultiplier", 1);
                return temp;
            }
        }
        public static int coinCounter
        {
            set
            {
                PlayerPrefs.SetInt("coinCounter", value);

            }
            get
            {
                int temp = PlayerPrefs.GetInt("coinCounter", 0);
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
        public static float sfxValue
        {
            set
            {
                PlayerPrefs.SetFloat("sfxValue", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("sfxValue", 1);
                return temp;
            }
        }
        public static float mouseValue
        {
            set
            {
                PlayerPrefs.SetFloat("mouseValue", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("mouseValue", 0.1f);
                return temp;
            }
        }
        public static float musicValue
        {
            set
            {
                PlayerPrefs.SetFloat("musicValue", value);

            }
            get
            {
                float temp = PlayerPrefs.GetFloat("musicValue", 1);
                return temp;
            }
        }
        public static bool isMuted
        {
            set
            {
                PlayerPrefs.SetInt("isMuted", (value == true) ? 1 : 0);
            }
            get
            {
                int temp = PlayerPrefs.GetInt("isMuted", 0);
                return (temp == 1) ? true : false;
            }
        }
    }
}