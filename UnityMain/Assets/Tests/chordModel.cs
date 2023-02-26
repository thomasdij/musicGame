using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class chordModel
{

    [Test]
    public void ChordModel_root_returns_actual_root()
    {
        // Arrange
        ChordModel testChordModel = new ChordModel(0f, 4f, 7f);
        // Act
        // Assert
        Assert.AreEqual(testChordModel.ChordPitches["root"],0f);
    }

 
}
