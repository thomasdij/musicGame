using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirected : MonoBehaviour
{
    // The player's transform
    public Transform playerTransform;
    [SerializeField] float _offset = 3f;

    // Update is called once per frame
    void LateUpdate()
    {
        // Vector3 offsetDirection = playerTransform.forward * _offset;
        //Vector3 offsetDirection = new Vector3(0f, 1f * _offset, 0f);
        //transform.position = playerTransform.position + offsetDirection;
        Vector2 offsetDirection = playerTransform.up * _offset;
        transform.position = (Vector2)playerTransform.position + offsetDirection;
        transform.rotation = playerTransform.rotation;
    }
}
