using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    public Toggle muteToggle;
    public AudioClip selectSound;
    public AudioSource menuMusicSource;
    public AudioSource selectSoundSource;

    private void Start()
    {
        sfxSlider.value = ZombieApocalypse.GameStatus.sfxValue;
        musicSlider.value = ZombieApocalypse.GameStatus.musicValue;
        muteToggle.isOn = ZombieApocalypse.GameStatus.isMuted;

        sfxChange();
        musicChange();
        muteChange();
    }
    public void sfxChange()
    {
        ZombieApocalypse.GameStatus.sfxValue = sfxSlider.value;
        selectSoundSource.volume = sfxSlider.value;
        if(sfxSlider.value > 0)
        {
            ZombieApocalypse.GameStatus.isMuted = muteToggle.isOn = false;
        }
    }
    public void musicChange()
    {
        ZombieApocalypse.GameStatus.musicValue = musicSlider.value;
        menuMusicSource.volume = musicSlider.value;
        if (musicSlider.value > 0)
        {
            ZombieApocalypse.GameStatus.isMuted = muteToggle.isOn = false;
        }
    }
    public void muteChange()
    {
        ZombieApocalypse.GameStatus.isMuted = muteToggle.isOn;

        if(muteToggle.isOn)
        {
            ZombieApocalypse.GameStatus.musicValue = musicSlider.value = 0;
            ZombieApocalypse.GameStatus.sfxValue = sfxSlider.value = 0;

        }
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectSoundSource.PlayOneShot(selectSound);
        }
    }
}
