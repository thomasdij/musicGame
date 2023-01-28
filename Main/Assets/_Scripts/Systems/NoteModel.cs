using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class NoteModel
{
    public bool NewNote;
    public float Pitch;
    Dictionary<string, float[]> tensionCoefficients;

    public NoteModel()
    {
        NewNote = false;
        Pitch = 0.0f;
        string filePath = @"C:\Users\tddij\Unity Projects\music-game\Main\Assets\Resources\Other\Melody Analysis Engine - Melody Model for Export (1).csv";
        tensionCoefficients = ReadCSV(filePath);
    }

    public NoteModel(bool NewNote, float Pitch)
    {
        this.NewNote = NewNote;
        this.Pitch = Pitch;
        string filePath = @"C:\Users\tddij\Unity Projects\music-game\Main\Assets\Resources\Other\Melody Analysis Engine - Melody Model for Export (1).csv";
        tensionCoefficients = ReadCSV(filePath);
    }
   
    void intializeTensionCoefficients(string filePath)
    {
        tensionCoefficients = ReadCSV(filePath);
    }

    public float CalcDamage(float targetResistance, NoteModel emitterNoteModel)
    {
        float damage = 0f;
        float pitchDamage = Mathf.Pow(PitchTension(emitterNoteModel.Pitch) - targetResistance, 1.5f);
        if (pitchDamage > 0)
        {
            damage += pitchDamage;
        }
        Debug.Log($"CalcDamage: {damage}");
        return damage;
    }

    public float PitchTension(float pitch)
    {
        int row = (int)Math.Round(pitch -1);
        float pitchHeight = (float)(tensionCoefficients["PitchHeight"][row + 1]);
        return pitchHeight;
    }

    // Reads a CSV file with all the values for each descriptive variable in each circumstance
    Dictionary<string, float[]> ReadCSV(string filePath) 
    {
        // Read the CSV file and parse the contents into a 2D array of strings
        string[][] csvData = File.ReadAllLines(filePath)
            .Select(line => line.Split(','))
            .ToArray();

        // Check if the csvData array is null or empty
        if (csvData == null || csvData.Length == 0)
        {
            Debug.LogError("Error reading CSV file: file is empty or does not exist");
        }

        // Extract the column names from the first row of the CSV data
        string[] columnNames = csvData.First().ToArray();

        // Create a dictionary to store the arrays for each column
        Dictionary<string, float[]> columnsDict = new Dictionary<string, float[]>();

        // Iterate over the column names, and create an array for each column
        foreach (string columnName in columnNames) 
        {
            try 
            {
                float[] columnData = Enumerable.Range(1, csvData.Length - 1)
                    .Select(row => float.Parse(csvData[row][Array.IndexOf(columnNames, columnName)]))
                    .ToArray();
                columnsDict.Add(columnName, columnData);
            } 
            catch (FormatException e) 
            {
                // Log the column that caused the error
                Debug.LogError($"Error parsing CSV data: {e.Message} (column: {columnName}"); 
            }
        }
        return columnsDict;
    }

}
