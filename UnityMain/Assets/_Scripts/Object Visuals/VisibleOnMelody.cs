using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleOnMelody : MonoBehaviour
{
    [SerializeField] NoteGenerator Player;
    // Update is called once per frame
    void Update()
    {
        if (Player.MelodyPlaying)
        {
            Color spriteColor = GetComponent<SpriteRenderer>().color;
            spriteColor.a = 1f;
            GetComponent<SpriteRenderer>().color = spriteColor;
        }
        else
        {
            Color spriteColor = GetComponent<SpriteRenderer>().color;
            spriteColor.a = 0f;
            GetComponent<SpriteRenderer>().color = spriteColor;
        }
    }
}
