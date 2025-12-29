using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    [SerializeField]
    public AudioSource musicSource, sfxSource;

    [SerializeField]
    public float fadeDuration = 1.0f;

    [SerializeField]
    public float OriginalVolume = 1f;

    private Dictionary<SFXTypeEnum, AudioClip> sfxSounds;
    private Dictionary<MusicTypeEnum, AudioClip> musicSounds;
    private CancellationTokenSource musicCancelletionToken;

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

    private bool IsMusicTransitionPossible(MusicTypeEnum musicType)
    {
        if(!musicSounds.ContainsKey(musicType))
        {
            //TODO: Add exception?
            print("SoundManager: Music type " + musicType + " not found!");
            return false;
        }

        return true;
    }

    public async void PlayMusic(MusicTypeEnum musicType)
    {
        if(!IsMusicTransitionPossible(musicType))
        {
            return;
        }

        if(musicSource.clip == null)
        {
            musicSource.clip = musicSounds[musicType];
            musicSource.volume = OriginalVolume;
            musicSource.Play();
            print($"SoundManager: Starting up with Music {musicType}");

            return;
        }

        var sound = musicSounds[musicType];

        try
        {
            print($"Transitioning from {musicSource.clip?.name ?? "No music"} to music {musicType}");
            musicCancelletionToken?.Cancel();
            musicCancelletionToken?.Dispose();
            musicCancelletionToken = new CancellationTokenSource();
            await FadeTransitionAsync(sound, musicCancelletionToken.Token);
            print($"Transition Succesful to {musicType}");
        }
        catch (OperationCanceledException)
        {
            print("Music transition cancelled");
        }
        catch (Exception ex)
        {
            print($"Error during music transition: {ex.Message}");
        }
    }

    private async Task FadeTransitionAsync(AudioClip newClip, CancellationToken token)
    {
        if (musicSource.isPlaying)
        {
            while (musicSource.volume > 0)
            {
                token.ThrowIfCancellationRequested();
                musicSource.volume -= OriginalVolume * Time.deltaTime / fadeDuration;
                await Task.Yield();
            }
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        while (musicSource.volume < OriginalVolume)
        {
            token.ThrowIfCancellationRequested();
            musicSource.volume += OriginalVolume * Time.deltaTime / fadeDuration;
            await Task.Yield();
        }

        musicSource.volume = OriginalVolume;
    }
}