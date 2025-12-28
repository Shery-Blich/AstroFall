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

    public void UpdateTiltSound(MovementDirection state)
    {
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
        tiltSource.volume = Mathf.MoveTowards(tiltSource.volume, targetVolume, Time.deltaTime * fadeSpeed);
    }
}
