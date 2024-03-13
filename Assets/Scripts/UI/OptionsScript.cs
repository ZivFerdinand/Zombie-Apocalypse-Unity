using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider mouseSlider;
    public Toggle muteToggle;
    public AudioClip selectSound;
    public AudioSource menuMusicSource;
    public AudioSource selectSoundSource;

    private void Start()
    {
        musicSlider.value = ZombieApocalypse.GameStatus.musicValue;
        sfxSlider.value = ZombieApocalypse.GameStatus.sfxValue;
        muteToggle.isOn = ZombieApocalypse.GameStatus.isMuted;
        mouseSlider.value = ZombieApocalypse.GameStatus.mouseValue;

        sfxChange();
        musicChange();
        muteChange();
        mouseChange();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ZombieApocalypse.GameStatus.isPaused || SceneManager.GetActiveScene().name == "MainMenu")
            {
                selectSoundSource.volume = ZombieApocalypse.GameStatus.sfxValue;
                selectSoundSource.PlayOneShot(selectSound);
            }
        }
    }
    /// <summary>
    /// Update sfx volume value on slider change..
    /// </summary>
    public void sfxChange()
    {
        ZombieApocalypse.GameStatus.sfxValue = sfxSlider.value;
        selectSoundSource.volume = sfxSlider.value;
        if(sfxSlider.value > 0)
        {
            ZombieApocalypse.GameStatus.isMuted = muteToggle.isOn = false;
        }
    }

    /// <summary>
    /// Update music volumeusic value on slider change..
    /// </summary>
    public void musicChange()
    {
        ZombieApocalypse.GameStatus.musicValue = musicSlider.value;
        menuMusicSource.volume = musicSlider.value;
        if (musicSlider.value > 0)
        {
            ZombieApocalypse.GameStatus.isMuted = muteToggle.isOn = false;
        }
    }
    /// <summary>
    /// Update the mouse sensitivity on slider change.
    /// </summary>
    /// <summary>
    /// Update mouse value on slider change.
    /// </summary>
    public void mouseChange()
    {
        ZombieApocalypse.GameStatus.mouseValue = mouseSlider.value;
    }

    /// <summary>
    /// Update the mute volume status of the game.
    /// </summary>
    /// <summary>
    /// Update mute status on button click.
    /// </summary>
    public void muteChange()
    {
        ZombieApocalypse.GameStatus.isMuted = muteToggle.isOn;

        if(muteToggle.isOn)
        {
            ZombieApocalypse.GameStatus.musicValue = musicSlider.value = 0;
            ZombieApocalypse.GameStatus.sfxValue = sfxSlider.value = 0;

        }
    }
    
}
