using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]
    public AudioClip buttonClickSound;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioClip levelCompleteSound;
    public AudioClip gameOverSound;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Create audio sources
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }

    public void PlayCorrectAnswer()
    {
        PlaySFX(correctAnswerSound);
    }

    public void PlayWrongAnswer()
    {
        PlaySFX(wrongAnswerSound);
    }

    public void PlayLevelComplete()
    {
        PlaySFX(levelCompleteSound);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOverSound);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    [Header("Volume Settings")]
    [Range(0, 1)] public float sfxVolume = 0.8f;
    [Range(0, 1)] public float musicVolume = 0.6f;

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.volume = sfxVolume;
            sfxSource.PlayOneShot(clip);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        
        // Auto-unmute when volume is adjusted
        if (volume > 0 && musicSource.mute)
        {
            musicSource.mute = false;
            sfxSource.mute = false;
            PlayerPrefs.SetInt("Muted", 0);
        }
    }

    public void ToggleMute(bool mute)
    {
        musicSource.mute = mute;
        sfxSource.mute = mute;
        PlayerPrefs.SetInt("Muted", mute ? 1 : 0);
    }

    void Start()
    {
        // Load saved volume settings
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip != null && musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
}