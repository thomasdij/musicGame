using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordModel
{
    public Dictionary<string, float> ChordPitches { get; private set; }

    public ChordModel() 
    { 
    }
    public ChordModel(float root, float third, float fifth)
    {
        ChordPitches.Add("root", root);
        ChordPitches.Add("third", third);
        ChordPitches.Add("fifth", fifth);
    }
    public ChordModel(float root, float third, float fifth, float colorTone1)
    {
        ChordPitches.Add("root", root);
        ChordPitches.Add("third", third);
        ChordPitches.Add("fifth", fifth);
        ChordPitches.Add("colorTone1", colorTone1);
    }
    public ChordModel(float root, float third, float fifth, float colorTone1, float colorTone2)
    {
        ChordPitches.Add("root", root);
        ChordPitches.Add("third", third);
        ChordPitches.Add("fifth", fifth);
        ChordPitches.Add("colorTone1", colorTone1);
        ChordPitches.Add("colorTone2", colorTone2);
    }
    public ChordModel(float root, float third, float fifth, float colorTone1, float colorTone2, float colorTone3)
    {
        ChordPitches.Add("root", root);
        ChordPitches.Add("third", third);
        ChordPitches.Add("fifth", fifth);
        ChordPitches.Add("colorTone1", colorTone1);
        ChordPitches.Add("colorTone2", colorTone2);
        ChordPitches.Add("colorTone3", colorTone3);
    }
    public void AddColorTone (ChordModel chordModel, float colorTone) 
    {
        int colorToneSignifier = chordModel.ChordPitches.Count - 2;
        ChordPitches.Add("colorTone" + colorToneSignifier, colorTone);
    }
}
