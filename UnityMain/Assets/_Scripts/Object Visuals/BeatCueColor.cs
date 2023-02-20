using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCueColor : MonoBehaviour
{
    bool _firstChange = true;

    void FixedUpdate()
   {
        if (ManageMeasureProgress.SixteenthNoteProgress % 4 == 0 && _firstChange)
        {
            changeSpriteColor((ManageMeasureProgress.MeasureProgress + 1f)/4);
            _firstChange= false;
        }
        else if (ManageMeasureProgress.SixteenthNoteProgress % 4 != 0)
        {
             _firstChange= true;
        }

        void changeSpriteColor(float colorValue)
        {
            Color spriteColor = GetComponent<SpriteRenderer>().color;
            spriteColor.a = colorValue;
            GetComponent<SpriteRenderer>().color = spriteColor;
        }
   }
}
