using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagablePlayer : DamagableCharacter, ITakeDamageFromCollision
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void TakeDamageFromCollision(float damage)
    {
        _currentHealth -= damage;
        HealthBar.SetHealth((int)Mathf.Round(_currentHealth));
    }

    protected override void Die()
    {
        Debug.Log("Player died");
    }
}
