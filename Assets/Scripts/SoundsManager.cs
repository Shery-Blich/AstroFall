using System;
using System.Collections.Generic;
using UnityEngine;

public enum SFXTypeEnum
{
    GameOverSoundWin,
    GameOverSoundLose,
    ObstacleCollision,
    PlayerFall,
}

public enum MusicTypeEnum
{
    MainMenuMusic,
    GameMusic,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    public AudioClip gameOver,gameWin, mainMenuMusic, gameMusic, obstcaleCollision, playerfall;
    public AudioSource musicSource, sfxSource;

    private Dictionary<SFXTypeEnum, AudioClip> sfxSounds;
    private Dictionary<MusicTypeEnum, AudioClip> musicSounds;

    public void Awake()
    {
        if (Instance == null)
        {
            sfxSounds = new Dictionary<SFXTypeEnum, AudioClip>
            {
                { SFXTypeEnum.GameOverSoundWin, gameWin },
                { SFXTypeEnum.GameOverSoundLose, gameOver },
                { SFXTypeEnum.ObstacleCollision, obstcaleCollision },
                { SFXTypeEnum.PlayerFall, playerfall },
            };

            musicSounds = new Dictionary<MusicTypeEnum, AudioClip>
            {
                { MusicTypeEnum.MainMenuMusic, mainMenuMusic },
                { MusicTypeEnum.GameMusic, gameMusic },
            };

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // We start menu music on default
    public void Start()
    {
        print("SoundManager: Starting up");
        PlayMusic(MusicTypeEnum.MainMenuMusic);
    }

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


    //TODO
    public void PlaySFX(SFXTypeEnum sfxType)
    {
        if(!sfxSounds.ContainsKey(sfxType))
        {
            print("SoundManager: SFX type " + sfxType + " not found!");
            return;
        }

        sfxSource.PlayOneShot(sfxSounds[sfxType]);
        print("SoundManager: Playing SFX " + sfxType.ToString());
    }

    public void PlayMusic(MusicTypeEnum musicType)
    {
        if(!musicSounds.ContainsKey(musicType))
        {
            print("SoundManager: Music type " + musicType + " not found!");
            return;
        }

        var sound = musicSounds[musicType];

        if (musicSource.isPlaying && musicSource.clip == sound)
        {
            print("SoundManager: Music " + musicType.ToString() + " is already playing.");

            return;
        }

        musicSource.clip = sound;
        musicSource.loop = true;
        musicSource.Play();
        print("SoundManager: Playing music " + musicType.ToString());
    }
}