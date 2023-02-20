using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableCharacter : MonoBehaviour
{
    public HealthBar HealthBar;
    protected float _currentHealth;
    [SerializeField] protected int _maxHealth = 100;
    bool _dead = false;

    protected virtual void Start()
    {
        HealthBar.SetMaxHealth(_maxHealth);
        _currentHealth = (float)_maxHealth;
    }

    public void TakeDamageFromNotes(float damage)
    {
        _currentHealth -= damage;
        HealthBar.SetHealth((int)Mathf.Round(_currentHealth));
    }

    protected virtual void Update()
    {
        if (_currentHealth <= 0f && !_dead)
        {
            Die();
            _dead = true;
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Character died");
    }
}
