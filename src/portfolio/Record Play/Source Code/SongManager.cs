using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[Serializable]
public class TempoChangeEvent : UnityEvent<float> { }
[Serializable]
public class BeatEvent : UnityEvent<Beat> { }

public class SongManager : MonoBehaviour
{
    public List<Song> songs;
    public AudioSource audioSource;
    public Song currentSong;
    public float timePlaying = 0;
    public bool loop = false;
    public BeatEvent beatEvent = new BeatEvent();
    public TempoChangeEvent tempoChangeEvent = new TempoChangeEvent();

    private int beatCount = 0;

    void Start()
    {
        if (!songs.Any()) return;
        currentSong = songs.First();
        audioSource.clip = currentSong.clip;
        audioSource.loop = loop;
        tempoChangeEvent.Invoke(currentSong.tempo);
        audioSource.Play();

    }

    void Update()
    {
        if (audioSource.isPlaying)
            StartCoroutine(BeatIt(currentSong));
    }

    IEnumerator BeatIt(Song song)
    {
        if (timePlaying <= currentSong.songLength)
        {
            if (beatCount < song.beats.Count)
                beatEvent.Invoke(song.beats[beatCount]);
            timePlaying += Time.deltaTime;
            beatCount++;
            yield return new WaitForSeconds(currentSong.beatTimeStepInMilliseconds / 1000);
        }
        else
            yield return null;
    }
}
