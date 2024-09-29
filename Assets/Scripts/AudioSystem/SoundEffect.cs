using System;
using UnityEngine;

[Serializable]
public class SoundEffect
{
    public string name;
    public AudioClip[] clips;
    public float volume = 1f;
    public float pitchVariance = 0.1f;
    public float basePitch = 1f;
}