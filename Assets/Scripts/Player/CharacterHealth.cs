using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    UIHealth healthIndicator;
    float health;
    // Start is called before the first frame update
    void Start()
    {

        health = maxHealth;
        if(gameObject.tag == "Player") healthIndicator = GameObject.Find("UIHealth").GetComponent<UIHealth>();
        if(healthIndicator) healthIndicator.setHealth(health);
    }

    public float GetHealth()
    {
        return health;
    }

    public bool Damage(float amount)
    {
        bool killed = false;
        if (amount < 0)
        {
            return false;
        }
        health -= amount;
        if (healthIndicator) healthIndicator.damageUpdateHealth(health);
        if(health <= 0) {
            killed = true;
            health = 0;
            Kill();
        }
        return killed;
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
        MatchManager.i.AddDeath(gameObject.name);

        if (gameObject.tag == "Player")
		{
            GameObject deathcam = GameObject.Find("DeathCam");
            deathcam.transform.position = gameObject.transform.position;
            deathcam.transform.rotation = gameObject.transform.rotation;
            deathcam.GetComponent<Camera>().enabled = true;
            deathcam.GetComponent<AudioListener>().enabled = true;

        }

        SpawningManager.instance.QueueToSpawn(gameObject);

        Destroy(gameObject);
    }
}
