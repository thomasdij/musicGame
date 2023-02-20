using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;
    public GameObject Player;
  //  public GameObject healthBar;

    public void SetMaxHealth (int health)
    {
        Slider.maxValue = health;
        Slider.value = health;
    }

    public void SetHealth (int health)
    {
        Slider.value = health;
    }

    void LateUpdate()
    {
        // Get the health bar's current rotation
        Quaternion healthBarRotation = transform.rotation;

        // Get the direction vector pointing towards the top of the screen
        Vector3 upDirection = new Vector3(0, 0, 1);

        // Get the rotation needed to orient the health bar towards the top of the screen
        Quaternion lookRotation = Quaternion.LookRotation(upDirection, transform.forward);

        // Set the health bar's rotation to the calculated look rotation
        transform.rotation = lookRotation;

        // Get the player's position
        Vector3 playerPosition = Player.transform.position;

        // Set the health bar's position to be above the player's position
        transform.position = playerPosition + new Vector3(0, 1f, 0);
    }
}


