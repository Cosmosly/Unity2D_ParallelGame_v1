using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager audioManager;

    [Header("Ambience Sound")]
    public AudioClip ambienceClip;
    public AudioClip backgroundMusicClip;

    [Header("FXsound")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;

    [Header("Robbie Sound")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip jumpSpeakClip;
    public AudioClip deathClip;
    public AudioClip deathVoiceClip;
    public AudioClip orbVoiceClip;

    private AudioSource ambienceSource;
    private AudioSource backgroundMusicSource;
    private AudioSource fxSource;
    private AudioSource playerSource;
    private AudioSource voiceSource;

    private void Awake()
    {
        audioManager = this;
        DontDestroyOnLoad(gameObject); // when changing scene, this would not be destroy

        ambienceSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        // [ambience sound]
        audioManager.ambienceSource.clip = audioManager.ambienceClip;
        audioManager.ambienceSource.loop = true;
        audioManager.ambienceSource.volume = 0.3f;
        audioManager.ambienceSource.Play();

        // [background music]
        audioManager.backgroundMusicSource.clip = audioManager.backgroundMusicClip;
        audioManager.backgroundMusicSource.loop = true;
        audioManager.backgroundMusicSource.volume = 0.5f;
        audioManager.backgroundMusicSource.Play();


    }

    public static void PlayFootstepAudio()
    {
        int index = Random.Range(0, audioManager.walkStepClips.Length);

        audioManager.playerSource.clip = audioManager.walkStepClips[index];
        audioManager.playerSource.Play();
    }

    public static void PlayCrouchFootstepAudio()
    {
        int index = Random.Range(0, audioManager.crouchStepClips.Length);

        audioManager.playerSource.clip = audioManager.crouchStepClips[index];
        audioManager.playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        audioManager.playerSource.clip = audioManager.jumpClip;
        audioManager.playerSource.Play();

        audioManager.voiceSource.clip = audioManager.jumpSpeakClip;
        audioManager.voiceSource.Play();
    }

    public static void PlayDeathAudio()
    {
        audioManager.playerSource.clip = audioManager.deathClip;
        audioManager.playerSource.Play();
        
        audioManager.voiceSource.clip = audioManager.deathVoiceClip;
        audioManager.voiceSource.Play();
        
        audioManager.fxSource.clip = audioManager.deathFXClip;
        audioManager.fxSource.Play();

    }

    public static void PlayOrbAudio()
    {
        audioManager.fxSource.clip = audioManager.orbFXClip;
        audioManager.fxSource.Play();

        audioManager.voiceSource.clip = audioManager.orbVoiceClip;
        audioManager.voiceSource.Play();
    }
}
