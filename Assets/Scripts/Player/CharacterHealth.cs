using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public float getHealth()
    {
        return health;
    }

    public void Damage(float amount)
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

    public void Heal(float amount)
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
        if(gameObject.name == "Player")
            SceneManager.LoadScene("PracticeLevel");
        Destroy(gameObject);
    }
}
