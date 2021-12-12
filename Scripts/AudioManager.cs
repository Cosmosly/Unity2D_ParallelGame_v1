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
    public AudioClip doorFXClip;
    public AudioClip startLevelClip;
    public AudioClip winClip;

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


    [Header("AudioMixer Group")]
    public AudioMixerGroup ambientGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup FXGroup;
    public AudioMixerGroup playerGroup;
    public AudioMixerGroup voiceGroup;

    private void Awake()
    {
        if (audioManager != null)
        {
            Destroy(gameObject);
            return;
        }
        audioManager = this;
        DontDestroyOnLoad(gameObject); // when changing scene, this would not be destroy

        ambienceSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        // [AudioMixer Group combine]
        ambienceSource.outputAudioMixerGroup = ambientGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        backgroundMusicSource.outputAudioMixerGroup = musicGroup;
        fxSource.outputAudioMixerGroup = FXGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;


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

        // [start level]
        audioManager.fxSource.clip = audioManager.startLevelClip;
        audioManager.fxSource.Play();


    }

    public static void PlayerWonAudio()
    {
        audioManager.fxSource.clip = audioManager.winClip;
        audioManager.fxSource.Play();

        // cut off other music
        audioManager.playerSource.Stop();
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

    public static void PlayDoorOpenAudio()
    {
        audioManager.fxSource.clip = audioManager.doorFXClip;
        audioManager.fxSource.PlayDelayed(1f);
    }
}
