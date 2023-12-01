using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class ZombieAudioManager : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    public AudioClip[] attackSounds;
    public AudioClip deadSound;
    public AudioSource soundSource;
    private float initSoundValue;
    public AudioSource growlSoundSource;
    private float initGrowlSoundValue;
    private void Start()
    {
        initSoundValue = soundSource.volume;
        initGrowlSoundValue = growlSoundSource.volume;
    }
    public void LeftFoot()
    {
        int n = Random.Range(0,footstepSounds.Length);
        soundSource.volume = initSoundValue * ZombieApocalypse.GameStatus.sfxValue;
        soundSource.clip= footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void LeftRunFoot()
    {
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.volume = initSoundValue * ZombieApocalypse.GameStatus.sfxValue;
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void RightRunFoot()
    {
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.volume = initSoundValue * ZombieApocalypse.GameStatus.sfxValue;
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void RightFoot()
    {
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.volume = initSoundValue * ZombieApocalypse.GameStatus.sfxValue;
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void Attack()
    {
        int n = Random.Range(0, attackSounds.Length);
        soundSource.volume = initSoundValue * ZombieApocalypse.GameStatus.sfxValue;
        soundSource.clip = attackSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        attackSounds[n] = attackSounds[0];
        attackSounds[0] = soundSource.clip;
    }
    public void DeadZombie()
    {
        growlSoundSource.Stop();
        soundSource.clip = deadSound;
        soundSource.PlayOneShot(deadSound);
    }
    private void Update()
    {
        if (growlSoundSource.isPlaying)
        {
            growlSoundSource.volume = initGrowlSoundValue * ZombieApocalypse.GameStatus.sfxValue;
            if (ZombieApocalypse.GameStatus.isPaused)
            growlSoundSource.Stop();
        }
        else
        {
            growlSoundSource.Play();
        }
    }
}
