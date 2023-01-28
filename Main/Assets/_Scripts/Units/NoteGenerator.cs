using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoteGenerator : MonoBehaviour
{
    
    public bool MelodyPlaying { get; private set; }
    private float frequency; // pitch of the note in Hz
    private float duration; // duration of the note in seconds
    public int SixteenthNotesBetweenAllowableMelodies { get; private set; } // time in 16th notes between opportunities to fire the melody
    public MelodyModel melodyModel1 { get; private set; }
    public MelodyModel melodyModel2 { get; private set; }
    public AudioSource audioSource { get; private set; }
    public NoteModel CurrentNoteModel { get; private set; }
    private float[] frequencyTable =
        {
        10f, 261.63f, 277.18f, 293.66f, 311.13f, 329.63f, 349.23f, 369.99f, 392f, 415.3f, 440f, 466.16f, 493.88f, 523.25f,
        554.37f, 587.33f, 622.25f, 659.25f, 698.46f, 739.99f, 783.99f, 830.61f, 880f, 932.33f, 987.77f, 1046.5f
        }; // frequency of each musical note chromaticly from C4 to C6, 1st value acts as rest

    void Start()
    {
        MelodyPlaying = false;
        audioSource = GetComponent<AudioSource>();
        SixteenthNotesBetweenAllowableMelodies = 2;
        // Populate melodyModel1
        melodyModel1 = new MelodyModel(16);
        float[] pitchArray1 = { 3f, 3f, 3f, 3f, 0f, 7f, 5f, 3f, 7f, 7f, 7f, 7f, 0f, 10f, 8f, 7f };
        bool[] boolArray1 = { true, false, true, false, true, true, true, true, true, false, true, false, true, true, true, true};
        melodyModel1.Populate(melodyModel1, pitchArray1, boolArray1);
        // Populate melodyModel2
        melodyModel2 = new MelodyModel(16);
        float[] pitchArray2 = { 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f };
        bool[] boolArray2 = { true, false, true, false, true, true, true, true, true, false, true, false, true, true, true, true };
        melodyModel2.Populate(melodyModel2, pitchArray2, boolArray2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateMelody(melodyModel1);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GenerateMelody(melodyModel2);
        }
    }

    public void GenerateMelody(MelodyModel melodyModel)
    {
        // check if note is on allowable beat
        if ((ManageMeasureProgress.SixteenthNoteProgress) % SixteenthNotesBetweenAllowableMelodies == 0)
        {
            if (MelodyPlaying)
            {
                // Melody is already playing, another melody cannot be played until this melody finishes
                return;
            }
            // if the entry to play melody is late but not too late, no delay is given to PlayMelody
            else if (ManageMeasureProgress.MeasureProgress - (float)ManageMeasureProgress.SixteenthNoteProgress * .25 < .01)
            {
                Debug.Log($"Start late generate MELODY time: {Time.time}, measureProgress: {ManageMeasureProgress.MeasureProgress}, 16th note: {ManageMeasureProgress.SixteenthNoteProgress}");
                StartCoroutine(PlayMelody(0f, melodyModel));
            }
            // check if the melody is late within the timespan of a 16th note
            else
            {

                // if the melody is late enough within the timespan of a 16th note, it is actually not too early for the next 16th note
                if (ManageMeasureProgress.MeasureProgress - (float)ManageMeasureProgress.SixteenthNoteProgress * .25f < .20f)
                {
                    // durationOfOne16thNote =  60 sec per min * 1 beat (quarter note) per 4 16th notes * min per beat
                    float durationOfOne16thNote = 60f / 4f / ManageMeasureProgress.BPM;
                    // timeElapsedInMeasure = MeasureProgress * min per beat (quarter note) * 60 sec per min
                    float timeElapsedInMeasure = ManageMeasureProgress.MeasureProgress / ManageMeasureProgress.BPM * 60f;
                    // timeElapsedIn16thNotes = SixteenthNoteProgress * 1 beat (quarter note) per 4 16thNotes * min per beat * 60 sec per min
                    float timeElapsedIn16thNotes = ManageMeasureProgress.SixteenthNoteProgress / 4f / ManageMeasureProgress.BPM * 60f;
                    // timeUntilNextSixteenthNote = time of 1 full 16th note - time elapsed within current 16th note so far
                    float timeUntilNextSixteenthNote = durationOfOne16thNote + timeElapsedInMeasure - timeElapsedIn16thNotes;
                    Debug.Log($"Start early generate MELODY wait time: {timeUntilNextSixteenthNote}, time: {Time.time}, measureProgress: {ManageMeasureProgress.MeasureProgress}, 16th note: {ManageMeasureProgress.SixteenthNoteProgress}");
                    StartCoroutine(PlayMelody(timeUntilNextSixteenthNote, melodyModel));
                }
                // if the entry to play the melody is given at the wrong time, it does not execute
                else
                {
                    //Debug.Log($"ENTRY LATE IN 16TH NOTE: measureProg- {ManageMeasureProgress.MeasureProgress}, 16thProg- {ManageMeasureProgress.SixteenthNoteProgress}, " +
                    //    $"dist- {ManageMeasureProgress.MeasureProgress * 100 - (float)ManageMeasureProgress.SixteenthNoteProgress * 25}");
                }
            }
        }
        else
        {
           // Debug.Log($"ENTRY OFF BEAT: 16thProg- {ManageMeasureProgress.SixteenthNoteProgress}");
        }
    }

    IEnumerator PlayMelody(float timeUntilNextSixteenthNote, MelodyModel melodyModel)
    {
        yield return new WaitForSecondsRealtime(timeUntilNextSixteenthNote);
        int currentSixteenthNote;
        int sixteenthNoteLength;
        MelodyPlaying = true;
        for (int sixteenthNoteIndex = 0; sixteenthNoteIndex < melodyModel.MelodyArray.Length; sixteenthNoteIndex++)
        {
            currentSixteenthNote = ManageMeasureProgress.SixteenthNoteProgress;
            sixteenthNoteLength = 1;
            float pitch;
            pitch = melodyModel.MelodyArray[sixteenthNoteIndex].Pitch;
            if (sixteenthNoteIndex + 1 < melodyModel.MelodyArray.Length)
            {
                while (melodyModel.MelodyArray[sixteenthNoteIndex + 1].NewNote == false)
                {
                    sixteenthNoteLength++;
                    sixteenthNoteIndex++;
                    if (sixteenthNoteIndex > melodyModel.MelodyArray.Length - 1)
                    {
                        break;
                    }
                }
            }
            CurrentNoteModel = melodyModel.MelodyArray[sixteenthNoteIndex];
            Debug.Log($"    Start generate note time: {Time.time}, measureProgress: {ManageMeasureProgress.MeasureProgress}, 16th note: {ManageMeasureProgress.SixteenthNoteProgress}");
            GenerateNote(pitch, sixteenthNoteLength);
            Debug.Log($"    End generate note time: {Time.time}, measureProgress: {ManageMeasureProgress.MeasureProgress}, 16th note: {ManageMeasureProgress.SixteenthNoteProgress}");
            // Wait until the sixteenth note after the end of the current note
            yield return new WaitUntil(() => ManageMeasureProgress.SixteenthNoteProgress == (currentSixteenthNote + sixteenthNoteLength) % 16);
            sixteenthNoteLength = 1;
        }
        MelodyPlaying = false;
    }

    public void GenerateNote(float pitch, int sixteenthNoteLength)
    {
        frequency = ConvertToFrequency(pitch);
        duration = ConvertToDuration(sixteenthNoteLength);
        // create a new audio clip with one channel and a length of 44100 samples (1 second at 44.1kHz)
        int sampleRate = 44100;
        int length = (int)Mathf.Round(sampleRate * duration);
        AudioClip audioClip = AudioClip.Create("MyAudioClip", length, 1, sampleRate, false);

        // create an array to hold the audio samples
        float[] samples = new float[length];

        // generate a sine wave for the audio data
        for (int i = 0; i < length; i++)
        {
            samples[i] = Mathf.Sin(2.0f * Mathf.PI * frequency * ((float)i / sampleRate));
        }

        // set the audio data for the audio clip
        audioClip.SetData(samples, 0);

        // play the audio clip
        audioSource.PlayOneShot(audioClip);
    }

    public float ConvertToFrequency(float pitch)
    {
        int noteInt = (int)Math.Round(pitch);
        float frequency = frequencyTable[noteInt];
        return frequency;
    }
    public float ConvertToDuration(int sixteenthNoteLength)
    {
        // duration in seconds 4 beats per measure, seconds per minute, beats per measure, 16th notes per measure, note shortener
        // notes are shortened slightly to prevent note overlap that occurrs from note generation delays
        float duration = (float)sixteenthNoteLength * 4f * 60f / ManageMeasureProgress.BPM / 16f * 0.96f;
        return duration;
    }
}
