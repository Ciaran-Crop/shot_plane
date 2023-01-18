using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] float minPitch = 0.9f;
    [SerializeField] float maxPitch = 1.1f;
    [SerializeField] AudioSource sFXPlayer;

    [Header("--- Audio Data ---")]

    [SerializeField] public AudioData EnemyExplosion1Data;
    [SerializeField] public AudioData EnemyExplosion2Data;
    [SerializeField] public AudioData EnemyProjectileHit1Data;
    [SerializeField] public AudioData EnemyProjectileHit2Data;
    [SerializeField] public AudioData EnemyProjectileLaunch1Data;
    [SerializeField] public AudioData EnemyProjectileLaunch2Data;
    [SerializeField] public AudioData EnemyProjectileLaunch3Data;
    [SerializeField] public AudioData PlayerDodgeData;
    [SerializeField] public AudioData PlayerExplosionData;
    [SerializeField] public AudioData PlayerProjectileHit1Data;
    [SerializeField] public AudioData PlayerProjectileHit2Data;
    [SerializeField] public AudioData PlayerProjectileHit3Data;
    [SerializeField] public AudioData PlayerProjectileLaunchData;


    public void PlayAudio(AudioData audioData)
    {
        PlayOneShot(audioData);
    }

    public void PlayRandomPitch(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(minPitch, maxPitch);
        PlayOneShot(audioData);
    }

    public void PlayRandomAudios(AudioData[] audioDatas)
    {
        PlayOneShot(audioDatas[Random.Range(0, audioDatas.Length)]);
    }

    public void PlayRandomEnemyLaunch()
    {
        AudioData[] audioDatas = new AudioData[3] { EnemyProjectileLaunch1Data, EnemyProjectileLaunch2Data, EnemyProjectileLaunch3Data };
        PlayRandomAudios(audioDatas);
    }

    public void PlayRandomEnemyExplosion()
    {
        AudioData[] audioDatas = new AudioData[2] { EnemyExplosion1Data, EnemyExplosion2Data };
        PlayRandomAudios(audioDatas);
    }

    public void PlayRandomPlayerProjectileHit()
    {
        AudioData[] audioDatas = new AudioData[3] { PlayerProjectileHit1Data, PlayerProjectileHit2Data, PlayerProjectileHit3Data };
        PlayRandomAudios(audioDatas);
    }

    public void PlayOneShot(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.AudioClip, audioData.Volume);
    }

    public void PlayEnemyExplosion1()
    {
        PlayOneShot(EnemyExplosion1Data);
    }

    public void PlayEnemyExplosion2()
    {
        PlayOneShot(EnemyExplosion2Data);
    }
    public void PlayEnemyProjectileHit1()
    {
        PlayOneShot(EnemyProjectileHit1Data);
    }
    public void PlayEnemyProjectileHit2()
    {
        PlayOneShot(EnemyProjectileHit2Data);
    }
    public void PlayEnemyProjectileLaunch1()
    {
        PlayOneShot(EnemyProjectileLaunch1Data);
    }
    public void PlayEnemyProjectileLaunch2()
    {
        PlayOneShot(EnemyProjectileLaunch2Data);
    }
    public void PlayEnemyProjectileLaunch3()
    {
        PlayOneShot(EnemyProjectileLaunch3Data);
    }

    public void PlayPlayerDodge()
    {
        PlayOneShot(PlayerDodgeData);
    }
    public void PlayPlayerExplosion()
    {
        PlayOneShot(PlayerExplosionData);
    }
    public void PlayPlayerProjectileLaunch(bool isRandomPitch)
    {
        if (isRandomPitch)
        {
            PlayRandomPitch(PlayerProjectileLaunchData);
        }
        else
        {
            PlayOneShot(PlayerProjectileLaunchData);
        }
    }
    public void PlayPlayerProjectileHit1()
    {
        PlayOneShot(PlayerProjectileHit1Data);
    }
    public void PlayPlayerProjectileHit2()
    {
        PlayOneShot(PlayerProjectileHit2Data);
    }
    public void PlayPlayerProjectileHit3()
    {
        PlayOneShot(PlayerProjectileHit3Data);
    }
}
