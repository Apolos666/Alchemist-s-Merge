using UnityEngine;

public class BackgroundMusicManager : GenericSingleton<BackgroundMusicManager>
{
    [SerializeField] private MusicTrack[] musicTracks;
    private AudioSource audioSource;
    private int currentTrackIndex = -1;

    protected override void Awake()
    {
        base.Awake();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    private void Start()
    {
        PlayRandomTrack();
    }

    private void PlayRandomTrack()
    {
        var randomIndex = UnityEngine.Random.Range(0, musicTracks.Length);
        if (musicTracks.Length > 0)
        {
            PlayMusic(randomIndex);
        }
    }

    private void PlayMusic(int index)
    {
        if (index >= 0 && index < musicTracks.Length)
        {
            currentTrackIndex = index;
            audioSource.clip = musicTracks[currentTrackIndex].clip;
            audioSource.Play();
            EventBus.Publish(new TrackChangedEvent(GetCurrentTrackName()));
            EventBus.Publish(new PlayPauseChangedEvent(true));
        }
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            EventBus.Publish(new PlayPauseChangedEvent(false));
        }
        else
        {
            audioSource.UnPause();
            EventBus.Publish(new PlayPauseChangedEvent(true));
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
        EventBus.Publish(new PlayPauseChangedEvent(false));
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioSource.volume = volume;
        EventBus.Publish(new VolumeChangedEvent(volume));
    }

    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        PlayMusic(currentTrackIndex);
    }

    public void PlayPreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + musicTracks.Length) % musicTracks.Length;
        PlayMusic(currentTrackIndex);
    }

    public string GetCurrentTrackName()
    {
        return currentTrackIndex >= 0 && currentTrackIndex < musicTracks.Length
            ? musicTracks[currentTrackIndex].name
            : "No track playing";
    }

    public float GetCurrentTrackProgress()
    {
        if (audioSource.clip != null)
        {
            return audioSource.time / audioSource.clip.length;
        }
        return 0f;
    }
}