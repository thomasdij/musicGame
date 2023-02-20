using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManageMeasureProgress : MonoBehaviour
{
    public delegate void New16thNote();
    public static event New16thNote OnNew16thNote;
    public static float BPM { get; private set; } // beats per minute
    public static float MeasureProgress { get; private set; } // quantity of beats that have occurrred in a given measure
    public static int SixteenthNoteProgress { get; private set; } // number of 16th note time intervals that have passed in a given measure
    public static int MeasureCount { get; private set; } // the number of measures that have passed
    public static float TimeStep { get; private set; } // quantity in beats that time advances each fixedDeltaTime
    int _current16thNote;
    [SerializeField] AudioSource _audioSource;
    void Start()
    {
        BPM = 60f;
        TimeStep = BPM / 60 * Time.fixedDeltaTime; 
        SixteenthNoteProgress = 0;
        MeasureProgress = 0;
        MeasureCount= 0;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }

    void FixedUpdate()
    {
        // seconds progressed in audio file * 60 seconds per minute * minutes per beat mod length of one measure 
        MeasureProgress = _audioSource.time * 60 / BPM % 4;
        SixteenthNoteProgress = (int)Mathf.Floor(MeasureProgress * 4f);
        if (_current16thNote != SixteenthNoteProgress)
        {
            OnNew16thNote?.Invoke();
            _current16thNote= SixteenthNoteProgress;
        }
        MeasureCount = (int)Mathf.Floor(_audioSource.time * BPM / 4 / 60);
    }
}