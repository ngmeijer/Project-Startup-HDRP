using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Footsteps are trigger by Animation Events and playrd random with the array of sounds
/// </summary>
public class PlayFootSteps : MonoBehaviour
{
    [Range(-3f,3f)]
    public float minPitch;
    [Range(-3f,3f)]
    public float maxPitch;

    [SerializeField] private AudioSource[] _audioSources;

    /// <summary>
    /// Event with this name in animations
    /// </summary>
    public void RightStep()
    {
        if (!(_audioSources[0].isPlaying || _audioSources[1].isPlaying))
        {
            SetRandomPitchBetweenVariation(1);
            _audioSources[1].Play();
        }
    }

    /// <summary>
    /// Event with this name in animations
    /// </summary>
    public void LeftStep()
    {
        if (!(_audioSources[0].isPlaying || _audioSources[1].isPlaying))
        {
            SetRandomPitchBetweenVariation(0);
            _audioSources[0].Play();
        }
    }

    void SetRandomPitchBetweenVariation(int audioSourceIndex)
    {
        float pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSources[audioSourceIndex].pitch = pitch;

    }
}