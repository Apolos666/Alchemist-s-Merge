using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffectManager : GenericSingleton<SoundEffectManager>
{
    [SerializeField] private SoundEffect[] _soundEffects;
    [SerializeField] private int _poolSize = 5;

    private AudioSource[] _audioSources;
    private readonly Dictionary<string, SoundEffect> _soundEffectDictionary = new Dictionary<string, SoundEffect>();

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        _audioSources = new AudioSource[_poolSize];
        for (var i = 0; i < _poolSize; i++)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
        }

        foreach (var effect in _soundEffects)
        {
            _soundEffectDictionary[effect.name] = effect;
        }
    }

    public void PlaySound(string soundName, Vector3 position)
    {
        if (_soundEffectDictionary.TryGetValue(soundName, out var effect))
        {
            var source = GetAvailableAudioSource();
            if (source != null)
            {
                source.volume = effect.volume;
                source.pitch = effect.basePitch + Random.Range(0.1f, effect.pitchVariance);
                source.transform.position = position;
                source.PlayOneShot(effect.clips[Random.Range(0, effect.clips.Length)]);
            }
            else
            {
                Debug.LogWarning("No available audio sources.");
            }
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        return _audioSources.FirstOrDefault(audioSource => !audioSource.isPlaying);
    }
}
