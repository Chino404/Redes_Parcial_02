using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : NetworkBehaviour
{ 
    public static AudioManager instance;

    [Header("-------- Audio Source --------")]
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _SFXSource;

    [Space(25), Header("-------- Audio Clip --------")]
    public AudioClip musicLobby;
    public AudioClip shoot;
    public AudioClip takeDamage;

    private void Awake()
    {
        instance = this;
    }

    public override void Spawned()
    {
        _musicSource.clip = musicLobby;
        _musicSource.Play();
    }
    public void StopAll()
    {
        _SFXSource?.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        _SFXSource?.PlayOneShot(clip);
    }
    public void StopSFX()
    {
        _SFXSource?.Stop();
    }
}
