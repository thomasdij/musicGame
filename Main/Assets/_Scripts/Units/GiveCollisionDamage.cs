using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveCollisionDamage : MonoBehaviour
{
    [SerializeField] float damageInterval = 0.5f;
    [SerializeField] float _collisionDamage = 10f;
    private float nextDamageTime;
    private bool hasCollided;

    void Start()
    {
        hasCollided = false;
        nextDamageTime = Time.time;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        ITakeDamageFromCollision damagable = other.GetComponent<ITakeDamageFromCollision>();
        // if the character/object has collided with something that is damagable from collisions then deliver damage to the character/object
        if (damagable != null )
        {
            if (!hasCollided)
            {
                hasCollided = true;
                nextDamageTime = Time.time + damageInterval;
            }
            if (Time.time >= nextDamageTime)
            {
                nextDamageTime = Time.time + damageInterval;
                damagable.TakeDamageFromCollision(_collisionDamage);
            }
        }
    }
}
