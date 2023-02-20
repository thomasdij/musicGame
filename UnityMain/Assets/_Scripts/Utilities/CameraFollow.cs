using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The player's transform
    public Transform player;

    // Update is called once per frame
    void LateUpdate()
    {
        // Set the camera's position to the player's position
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
    
}
