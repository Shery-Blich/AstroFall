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

    [Header("Audio Clips")]
    [SerializeField] public AudioClip gameOver;
    [SerializeField] public AudioClip gameWin;
    [SerializeField] public AudioClip mainMenuMusic;
    [SerializeField] public AudioClip gameMusic;
    [SerializeField] public AudioClip obstcaleCollision;
    [SerializeField] public AudioClip playerfall;
    [Header("Audio Sources")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    [Header("Music Settings")]
    [SerializeField] public float fadeDuration = 1.0f;
    [SerializeField] public float OriginalVolume = 1f;
    private Dictionary<SFXTypeEnum, AudioClip> sfxSounds;
    private Dictionary<MusicTypeEnum, AudioClip> musicSounds;
    private CancellationTokenSource musicCancelletionToken;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerController.BadGameOver += PlayGameOverSound;
        FallManager.GoodGameOver += PlayWinSound;
    }

    private void OnDisable()
    {
        PlayerController.BadGameOver -= PlayGameOverSound;
        FallManager.GoodGameOver -= PlayWinSound;
    }

    // GAME EVENTS

    private void PlayGameOverSound()
    {
        PlaySFX(SFXTypeEnum.GameOverSoundLose);
    }

    private void PlayWinSound()
    {
        PlaySFX(SFXTypeEnum.GameOverSoundWin);
    }

    // SFX

    public void PlaySFX(SFXTypeEnum sfxType)
    {
        if (!sfxSounds.ContainsKey(sfxType))
        {
            Debug.LogWarning($"SoundManager: SFX type {sfxType} not found!");
            return;
        }

        sfxSource.PlayOneShot(sfxSounds[sfxType]);
        Debug.Log($"SoundManager: Playing SFX {sfxType}");
    }

    /// Plays a sound effect ONLY if the given transform is visible on screen.
    public void PlaySFXIfVisible(SFXTypeEnum sfxType, Transform sourceTransform)
    {
        if (!IsTransformVisible(sourceTransform))
            return;

        PlaySFX(sfxType);
    }

    private bool IsTransformVisible(Transform t)
    {
        if (t == null || Camera.main == null)
            return false;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(t.position);

        return viewportPos.z > 0 &&
               viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }

    // MUSIC

    private bool IsMusicTransitionPossible(MusicTypeEnum musicType)
    {
        if (!musicSounds.ContainsKey(musicType))
        {
            Debug.LogWarning($"SoundManager: Music type {musicType} not found!");
            return false;
        }

        return true;
    }

    public async void PlayMusic(MusicTypeEnum musicType)
    {
        if (!IsMusicTransitionPossible(musicType))
            return;
        if (musicSource.clip == null)
        {
            musicSource.clip = musicSounds[musicType];
            musicSource.volume = OriginalVolume;
            musicSource.Play();
            Debug.Log($"SoundManager: Starting music {musicType}");
            return;
        }
        try
        {
            musicCancelletionToken?.Cancel();
            musicCancelletionToken?.Dispose();
            musicCancelletionToken = new CancellationTokenSource();

            await FadeTransitionAsync(musicSounds[musicType], musicCancelletionToken.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("SoundManager: Music transition cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SoundManager: Music transition error - {ex.Message}");
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