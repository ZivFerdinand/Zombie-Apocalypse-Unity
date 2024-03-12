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
    public AudioSource growlSoundSource;

    private float initSoundValue;
    private float initGrowlSoundValue;
    private void Start()
    {
        initSoundValue = soundSource.volume;
        initGrowlSoundValue = growlSoundSource.volume;
    }
    private void initSound(ref AudioClip[] clips)
    {
        int n = Random.Range(0, clips.Length);
        soundSource.volume = initSoundValue * ZombieApocalypse.GameStatus.sfxValue;
        soundSource.clip = clips[n];

        soundSource.PlayOneShot(soundSource.clip);

        clips[n] = footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void LeftFoot()
    {
        initSound(ref footstepSounds);
    }
    public void LeftRunFoot()
    {
        initSound(ref footstepSounds);
    }
    public void RightRunFoot()
    {
        initSound(ref footstepSounds);
    }
    public void RightFoot()
    {
        initSound(ref footstepSounds);
    }
    public void Attack()
    {
        initSound(ref attackSounds);
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
