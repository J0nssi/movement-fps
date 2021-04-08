using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    [SerializeReference]
    private UIHealth healthIndicator;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        if(healthIndicator) healthIndicator.setHealth(health);
    }

    public float GetHealth()
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
        if (healthIndicator) healthIndicator.damageUpdateHealth(health);
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
        if (healthIndicator) healthIndicator.healUpdateHealth(health);
    }

    public void Kill()
    {
        Debug.Log(gameObject.name + " was killed.");

        if(gameObject.name == "Player")
            SceneManager.LoadScene("PracticeLevel");
        Destroy(gameObject);
    }
}
