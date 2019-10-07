using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Song : MonoBehaviour
{
    public AudioClip clip;
    public List<Beat> beats = new List<Beat>();
    public float beatTimeStepInMilliseconds;
    public float songLength;
    public float tempo;
    public int randomNoteLimitPerBeat = 3;
    public int randomSkipBeat = 4;

    void Start()
    {
        if (songLength <= 0)
            songLength = clip.length;
        if (beats.Count <= 0)
            GenerateRandomBeats();
    }

    private void GenerateRandomBeats()
    {
        var beatCount = songLength / (beatTimeStepInMilliseconds / 1000);
        for (int i = 0; i < beatCount; i++)
        {
            var beat = new Beat();
            beat.notes = new List<bool>();
            var noteCount = 0;
            for (int j = 0; j < 7; j++)
            {
                var noted = Random.value > 0.5f;
                if (noted) noteCount++;
                if (noteCount < randomNoteLimitPerBeat)
                    beat.notes.Add(noted);
                else
                    beat.notes.Add(false);
            }

            if (noteCount > 1)
            {
                i += randomSkipBeat;
                for (int j = 0; j < randomSkipBeat; j++)
                {
                    beats.Add(new Beat()
                    {
                        notes = new List<bool>()
                        {
                            false, false, false, false, false, false, false
                        }
                    });
                }

            }

            beats.Add(beat);
        }
    }
}

[Serializable]
public class Beat
{
    public List<bool> notes = new List<bool>();
}

