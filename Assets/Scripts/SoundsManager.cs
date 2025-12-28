using System;
using System.Collections.Generic;
using UnityEngine;

public enum SFXTypeEnum
{
    MovementSound,
    GameOverSoundLose,
    GameOverSoundWin,
    ButtonClickSound,
    ObstcaleToObstacleCollisionSound,
}

public enum MusicTypeEnum
{
    MainMenuMusic,
    GameMusic,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public Sound[] sfxSounds;
    public Sound[] musicSounds;
    //TODO: USE Dict instead of arrays
    //public Dictionary<SFXTypeEnum, Sound> sfxSounds;
    //public Dictionary<MusicTypeEnum, Sound> musicSounds;
    public AudioSource musicSource, sfxSource;

    public void OnEnable()
    {
        PlayerController.BadGameOver += PlayGameOverSound;
        FallManager.GoodGameOver += PlayWinSound;
    }

    public void OnDisable()
    {
        PlayerController.BadGameOver -= PlayGameOverSound;
        FallManager.GoodGameOver -= PlayWinSound;
    }

    private void PlayGameOverSound()
    {
        PlaySFX(SFXTypeEnum.GameOverSoundLose);
    }

    private void PlayWinSound()
    {
       PlaySFX(SFXTypeEnum.GameOverSoundWin);
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        print("SoundManager: Playing Main Menu Music");
        PlayMusic(MusicTypeEnum.MainMenuMusic);
    }

    //TODO
    public void PlaySFX(SFXTypeEnum sfxType)
    {
        var sound = Array.Find(sfxSounds, s => s.name == sfxType.ToString());
        if (sound == null)
        {
            Debug.LogWarning("SoundManager: SFX type " + sfxType + " not found!");
            return;
        }

        sfxSource.clip = sound.clip;
        sfxSource.PlayOneShot(sound.clip);


        //if (!sfxSounds.ContainsKey(sfxType))
        //{
        //    Debug.Log("SoundManager: SFX type " + sfxType + " not found!");
        //    return;
        //}

        //Sound sfxSound = sfxSounds[sfxType];
        //sfxSource.PlayOneShot(sfxSound.clip);
    }

    public void PlayMusic(MusicTypeEnum musicType)
    {
        var sound = Array.Find(musicSounds, s => s.name == musicType.ToString());
        if (sound == null)
        {
            Debug.LogWarning("SoundManager: Music type " + musicType + " not found!");
            return;
        }

        musicSource.clip = sound.clip;
        musicSource.resource = sound.clip;
        musicSource.Play();
        Debug.Log("SoundManager: Playing music " + musicType.ToString());
        //if (!musicSounds.ContainsKey(musicType))
        //{
        //    Debug.Log("SoundManager: Music type " + musicType + " not found!");
        //    return;
        //}

        //Sound musicSound = musicSounds[musicType];
        //musicSource.Play();
    }
}