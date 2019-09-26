using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for playing the catch audio.
/// Requires two audio sources to play one shots of the catch sounds.
/// </summary>
public class CatchAudio : MonoBehaviour
{
    [Range(0, 1)]
    public float catchSoundVolume = 0.4f;
    [Range(0, 1)]
    public float monsterSoundVolume = 0.04f;
    [Range(0, 1)]
    public float missedSoundVolume = 1f;

    public AudioClip missedAudioClip;
    public AudioClip catchAudioClip;
    public AudioClip monsterAudioClip;

    public AudioSource audioSource01;
    public AudioSource audioSource02;

    private AudioSource audioSource;
    
    /// <summary>
    /// Plays the appropriate catch sound
    /// </summary>
    /// <param name="success">Catch hit/missed</param>
    public void PlaySound(bool success)
    {
        if (success)
        {
            audioSource01.volume = catchSoundVolume;
            audioSource01.PlayOneShot(catchAudioClip);
            audioSource02.volume = monsterSoundVolume;
            audioSource02.PlayOneShot(monsterAudioClip);
        }
        else
        {
            audioSource01.volume = missedSoundVolume;
            audioSource01.PlayOneShot(missedAudioClip);
        }
    }
}
