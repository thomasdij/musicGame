using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using System;
using System.ComponentModel;

public class noteModel
{
    // A Test behaves as an ordinary method
    [Test]
    public void PitchTension_Returns_1st_PitchHeight_Value_from_Tonic_Note_Reference()
    {
        // Arrange
        NoteModel testNoteModel = new NoteModel();
        // Act
        float testPitchTension = testNoteModel.PitchTension(testNoteModel.Pitch);
        // Assert
        Assert.AreEqual(testNoteModel.TensionCoefficients["PitchHeight"][0], testPitchTension);
    }
    [Test]
    public void ChordalTension_Returns_1st_ChordalDissonance_Value_from_Tonic_Note_Reference()
    {
        // Arrange
        ChordModel testChordModel = new ChordModel(0f,4f, 7f);
        NoteModel testNoteModel = new NoteModel();
        // Act
        float testChordalTension = testNoteModel.ChordalTension(0f, testChordModel);
        // Assert
        Assert.AreEqual(testNoteModel.TensionCoefficients["ChordalDissonance"][1], testChordalTension);
    }
}
