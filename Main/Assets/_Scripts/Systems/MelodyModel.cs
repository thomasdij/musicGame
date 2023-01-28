using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyModel : NoteModel
{
    public NoteModel[] MelodyArray;
    NoteModel noteModel = new NoteModel();

    public MelodyModel(int length)
    {
        if (length > 16)
        {
            Debug.Log("MelodyArray cannot exceed 16 elements");
        }
        MelodyArray = new NoteModel[length];
    }

    public MelodyModel(NoteModel[] melodyArray)
    {
        this.MelodyArray = melodyArray;
        if (melodyArray.Length > 16)
        {
            Debug.Log("MelodyArray cannot exceed 16 elements");
        }
    }

    public void Populate(MelodyModel melodyModel, float[] pitchArray, bool[] NewNoteArray)
    {
        if (pitchArray.Length != NewNoteArray.Length || pitchArray.Length != melodyModel.MelodyArray.Length)
        {
            Debug.Log("Input array lengths on MelodyModel.Populate are not equal.");
            return;
        }

        if (pitchArray.Length > 16)
        {
            Debug.Log("Input array lengths on MelodyModel.Populate cannot exceed 16");
            return;
        }

        for (int i = 0; i < pitchArray.Length; i++)
        {
            NoteModel noteModel = new NoteModel(NewNoteArray[i], pitchArray[i]);
            melodyModel.MelodyArray[i] = noteModel;
        }
    }
}