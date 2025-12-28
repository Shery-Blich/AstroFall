 using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource tiltSource;
    public float maxVolume = 0.4f;
    public float minVolume = 0.1f;
    public float fadeSpeed = 5f;

    private float targetVolume = 0f;

    void Awake()
    {
        tiltSource = GetComponent<AudioSource>();
        tiltSource.clip = SoundManager.Instance.playerfall;
    }

    private void Start()
    {
        tiltSource.Play();
    }

    // Call this from your PlayerController inside TiltPlayer
    public void UpdateTiltSound(MovementDirection state)
    {
        // If moving Left or Right, we want sound. Otherwise, silence.
        if (state == MovementDirection.Left || state == MovementDirection.Right)
        {
            targetVolume = maxVolume;
        }
        else
        {
            targetVolume = minVolume;
        }
    }

    void Update()
    {
        // Smoothly interpolate the volume to avoid "popping" sounds
        tiltSource.volume = Mathf.MoveTowards(tiltSource.volume, targetVolume, Time.deltaTime * fadeSpeed);
    }
}
