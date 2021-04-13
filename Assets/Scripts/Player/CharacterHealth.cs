using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    UIHealth healthIndicator;
    float health;
    GameObject deathCamObject;
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

    public void Damage(float amount)
    {
        if (amount < 0)
        {
            return;
        }
        health -= amount;
        if (healthIndicator) healthIndicator.damageUpdateHealth(health);
        if(health <= 0) {
            health = 0;
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

        if (gameObject.tag == "Player")
		{
            deathCamObject = Instantiate(new GameObject("DeathCam"), transform.position, transform.rotation);
            Camera deathCam =  deathCamObject.AddComponent<Camera>();
            deathCamObject.AddComponent<AudioListener>();
            Destroy(deathCam, SpawningManager.instance.respawnTime + 0.1f);
		}

        SpawningManager.instance.QueueToSpawn(gameObject);

        Destroy(gameObject);
    }
}
