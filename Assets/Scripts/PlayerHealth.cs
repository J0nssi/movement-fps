using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ICharacter
{
    public static PlayerHealth singleton;
    public int maxHealth = 100;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        health = maxHealth;
    }

    public int getHealth()
    {
        return health;
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            return;
        }
        health -= amount;
        if(health <= 0) {
            Kill();
        }
    }

    public void Heal(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        if(health + amount <= maxHealth)
        {
            health += maxHealth;
        } else
        {
            health += maxHealth - health + amount;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
