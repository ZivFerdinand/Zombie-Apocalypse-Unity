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
        Debug.Log("1");
        int n = Random.Range(0,footstepSounds.Length);
        soundSource.clip= footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void LeftRunFoot()
    {
        Debug.Log("3");
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void RightRunFoot()
    {
        Debug.Log("4");
        int n = Random.Range(0, footstepSounds.Length);
        soundSource.clip = footstepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);
        footstepSounds[n]= footstepSounds[0];
        footstepSounds[0] = soundSource.clip;
    }
    public void RightFoot()
    {
        Debug.Log("2");
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
        Debug.Log("6");
        soundSource.clip = deadSound;
        soundSource.PlayOneShot(deadSound);
    }
    // Update is called once per frame
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
