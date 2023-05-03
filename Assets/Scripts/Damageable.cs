using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 10; // The maximum health of the object
    public int currentHealth; // The current health of the object

    public delegate void DamageHandler();
    public event DamageHandler OnDamage; // Event for other components to subscribe to damage events

    public delegate void DieHandler();
    public event DieHandler OnDie; // Event for other components to subscribe to death events

    public bool disableOnDeath = true; // Whether to disable the object instead of destroying it on death

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int amount = 1)
    {
        currentHealth -= amount;

        // Invoke the damage event
        if (OnDamage != null)
        {
            OnDamage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Invoke the death event
        if (OnDie != null)
        {
            OnDie();
        }

        // Disable or destroy the object
        if (disableOnDeath)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
