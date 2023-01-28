using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearWhenClose : MonoBehaviour
{
    float _disappearDistance = 8f; // Distance at which the sprite will disappear
    [SerializeField] PlayerController Player;

    // Update is called once per frame
    void Update()
    {
        // Get the distance between the sprite and the player
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        // If the distance is less than the disappear distance, set the sprite's alpha to 0
        if (distance < _disappearDistance)
        {
            Color spriteColor = GetComponent<SpriteRenderer>().color;
            spriteColor.a = 0f;
            GetComponent<SpriteRenderer>().color = spriteColor;
        }
    }
}
