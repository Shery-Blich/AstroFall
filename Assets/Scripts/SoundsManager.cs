using System;
using UnityEngine;

public enum SFXTypeEnum
{
    GameOverSoundLose,
    GameOverSoundWin,
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
            print("SoundManager: SFX type " + sfxType + " not found!");
            return;
        }

        if (sfxSource.isPlaying && sfxSource.clip == sound.clip)
        {
            print("SoundManager: SFX " + sfxType.ToString() + " is already playing.");
            return;
        }

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
            print("SoundManager: Music type " + musicType + " not found!");
            return;
        }

        if (musicSource.isPlaying && musicSource.clip == sound.clip)
        {
            print("SoundManager: Music " + musicType.ToString() + " is already playing.");

            return;
        }

        musicSource.clip = sound.clip;
        musicSource.loop = true;
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