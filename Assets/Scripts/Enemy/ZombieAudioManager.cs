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
    public AudioSource Growl;
    
    public void LeftFoot()
    {
        int n = Random.Range(0,footstepSounds.Length);
        soundSource.clip= footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void LeftRunFoot()
    {
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void RightRunFoot()
    {
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void RightFoot()
    {
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void Attack()
    {
        Debug.Log("5");
        int n = Random.Range(0, attackSounds.Length);
        soundSource.clip = attackSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        attackSounds[n] = attackSounds[0];
        attackSounds[0] = soundSource.clip;
    }
    public void DeadZombie()
    {
        Growl.Stop();
        soundSource.clip = deadSound;
        soundSource.PlayOneShot(deadSound);
    }
    private void Update()
    {
        if (Growl.isPlaying)
        {
            if(ZombieApocalypse.GameStatus.isPaused)
            Growl.Stop();
        }
        else
        {
            Growl.Play();
        }
    }
}
