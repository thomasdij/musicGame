using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

public class NoteModel
{
    public bool NewNote;
    public float Pitch;
    public Dictionary<string, float[]> TensionCoefficients {get; private set;}
    private bool _tensionCoefficientsInitialized = false;
       
    public void InitializeTensionCoefficients()
    {
        TensionCoefficients = new Dictionary<string, float[]>
        {
            { "PitchHeight" ,        new float[] {1f  , 1.6f, 2.2f, 2.8f, 3.4f, 4f  , 4.6f, 5.2f, 5.8f, 6.4f, 7f  , 7.6f, 8.2f } },
            { "ChordalDissonance" ,  new float[] {6f  , -1.5f, -3f  , 0f } },
            { "HarmonicDissonance" , new float[] {0f  , 8.6f, 4.8f, 4.2f, 3.4f, 1.1f, 4.8f, 0.2f, 4.8f, 2.8f, 4.8f, 5.8f, 0f } },
            { "TonalDissonance" ,    new float[] {0.1f, 6.4f, 1.3f, 3.1f, 0.8f, 2.5f, 5f , 0.4f, 3.7f, 1.9f, 4.3f, 5.7f, 0f } },
            { "IntervalDissonance" , new float[] {0f  , 0.3f, 0.3f, 0.8f, 0.8f, 1f  , 4.7f, 1.2f, 1.9f, 2.1f, 2.5f, 3f  , 2.8f } },
            { "16thNotePlace" ,      new float[] {1f  , 2f  , 3f  , 4f  , 5f  , 6f  , 7f  , 8f  , 9f  , 10f , 11f , 12f , 13f } },
            { "RythmicDissonance" ,  new float[] {0.5f, 3.6f, 2.4f, 3.6f, 1.3f, 3.6f, 2.4f, 3.6f, 0.5f, 3.6f, 2.4f, 3.6f, 1.3f } },
            { "OnsetFrequency" ,     new float[] {2f  , 0f } },
        };
        _tensionCoefficientsInitialized = true;
    }

    public NoteModel()
    {
        NewNote = false;
        Pitch = 0f;
        if (_tensionCoefficientsInitialized == false)
        {
            InitializeTensionCoefficients();
        }
    }

    public NoteModel(bool NewNote, float Pitch)
    {
        this.NewNote = NewNote;
        this.Pitch = Pitch;
        if (_tensionCoefficientsInitialized == false)
        {
            InitializeTensionCoefficients();
        }
    }

    public float CalcDamage(float targetResistance, NoteModel emitterNoteModel)
    {
        float damage = 0f;
        float pitchDamage = Mathf.Pow(PitchTension(emitterNoteModel.Pitch) - targetResistance, 1.5f);
        if (pitchDamage > 0)
        {
            damage += pitchDamage;
        }
        return damage;
    }

    public float PitchTension(float pitch)
    {
        int row = (int)Math.Round(pitch - 1f);
        float pitchHeight = (float)(TensionCoefficients["PitchHeight"][row + 1]);
        return pitchHeight;
    }
    //  ChordalDissonance HarmonicDissonance  TonalDissonance IntervalDissonance	16thNotePlace RythmicDissonance   OnsetFrequency
    public float ChordalTension(float pitch, ChordModel chordModel)
    {
        float ChordalDissonance;
        int row = 3; // no tension change
        if (pitch == chordModel.ChordPitches["root"] || pitch == chordModel.ChordPitches["fifth"])
        {
            row = 1;
            ChordalDissonance = (TensionCoefficients["ChordalDissonance"][row]);
            return ChordalDissonance;
        }
        foreach (KeyValuePair<string, float> chordPitch in chordModel.ChordPitches)
        {
            if (pitch == chordPitch.Value) // pitch matches chord color tone including 3rd
            {
                row = 2;
                ChordalDissonance = (TensionCoefficients["ChordalDissonance"][row]);
                return ChordalDissonance;
            }
        }
        foreach (KeyValuePair<string, float> chordPitch in chordModel.ChordPitches)
        {
            if (pitch > chordPitch.Value - 1.1 && pitch < chordPitch.Value + 1.1) // pitch clashes with chord
            {
                row = 0;
                ChordalDissonance = (TensionCoefficients["ChordalDissonance"][row]);
                return ChordalDissonance;
            }
        }
        ChordalDissonance = (TensionCoefficients["ChordalDissonance"][row]);
        return ChordalDissonance;
    }

    public float HarmonicTension(float pitch)
    {
        int row = (int)Math.Round(pitch - 1);
        float HarmonicDissonance = (float)(TensionCoefficients["HarmonicDissonance"][row + 1]);
        return HarmonicDissonance;
    }
}

//string _tensionTableFilePath = @"C:\Users\tddij\Unity Projects\MusicGame\Main\Assets\Resources\Other\Melody Analysis Engine - Melody Model for Export (1).csv";

//// Reads a CSV file with all the values for each descriptive variable in each circumstance
//Dictionary<string, float[]> ReadCSV(string filePath)
//{
//    // Read the CSV file and parse the contents into a 2D array of strings
//    string[][] csvData = File.ReadAllLines(filePath)
//        .Select(line => line.Split(','))
//        .ToArray();

//    // Check if the csvData array is null or empty
//    if (csvData == null || csvData.Length == 0)
//    {
//        Debug.LogError("Error reading CSV file: file is empty or does not exist");
//    }

//    // Extract the column names from the first row of the CSV data
//    string[] columnNames = csvData.First().ToArray();

//    // Create a dictionary to store the arrays for each column
//    Dictionary<string, float[]> columnsDict = new Dictionary<string, float[]>();

//    // Iterate over the column names, and create an array for each column
//    foreach (string columnName in columnNames)
//    {
//        try
//        {
//            float[] columnData = Enumerable.Range(1, csvData.Length - 1)
//                .Select(row => float.Parse(csvData[row][Array.IndexOf(columnNames, columnName)]))
//                .ToArray();
//            columnsDict.Add(columnName, columnData);
//        }
//        catch (FormatException e)
//        {
//            // Log the column that caused the error
//            Debug.LogError($"Error parsing CSV data: {e.Message} (column: {columnName}");
//        }
//    }
//    return columnsDict;
//}