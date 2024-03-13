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

    /// <summary>
    /// This function will take the audioclip that has been entered in Unity.
    /// </summary>
    /// <param name="clips">Audioclip for sfx sound.</param>
    private void initSound(ref AudioClip[] clips)
    {
        int n = Random.Range(0, clips.Length);
        soundSource.volume = initSoundValue * ZombieApocalypse.GameStatus.sfxValue;
        soundSource.clip = clips[n];

        soundSource.PlayOneShot(soundSource.clip);

        clips[n] = footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }

    /// <summary>
    /// This function will produce the sound of the left foot touching the ground when walking.
    /// </summary>
    public void LeftFoot()
    {
        initSound(ref footstepSounds);
    }

    /// <summary>
    /// This function will produce the sound of the left foot touching the ground when running.
    /// </summary>
    public void LeftRunFoot()
    {
        initSound(ref footstepSounds);
    }

    /// <summary>
    /// This function will produce the sound of the right foot touching the ground when running.
    /// </summary>
    public void RightRunFoot()
    {
        initSound(ref footstepSounds);
    }

    /// <summary>
    /// This function will produce the sound of the right foot touching the ground when walking.
    /// </summary>
    public void RightFoot()
    {
        initSound(ref footstepSounds);
    }

    /// <summary>
    /// This function will produce the attack sound when the zombie attacks.
    /// </summary>
    public void Attack()
    {
        initSound(ref attackSounds);
    }

    /// <summary>
    /// This function will produce the sound of dead zombies.
    /// </summary>
    public void DeadZombie()
    {
        growlSoundSource.Stop();

        soundSource.clip = deadSound;
        soundSource.PlayOneShot(deadSound);
    }
    
}
