using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [SerializeField] int _movementForce;
    Rigidbody2D _Rigidbody;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
// Linear Movement       
        Vector2 move_force;
        move_force = new Vector2(_movementForce * Input.GetAxis("Horizontal"), _movementForce * Input.GetAxis("Vertical"));
        //Apply a force to this Rigidbody in direction of the input
        _Rigidbody.AddForce(move_force*Time.deltaTime);
    }

    void Update()
    {
    // Rotational Movement
        // Get the mouse position in screen coordinates
        Vector2 mousePos = Input.mousePosition;

        // Convert the mouse position to world coordinates
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Calculate the direction from the player to the mouse position
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        // Calculate the angle between the player's forward direction and the direction to the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // Set the player's rotation to the calculated angle
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

