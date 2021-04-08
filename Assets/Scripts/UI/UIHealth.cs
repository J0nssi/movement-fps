using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHealth : MonoBehaviour
{
    public static UIHealth playerHealth;
    public float effectTimeOut = 1f;

    int health;
    TextMeshProUGUI healthText;
    Color defaultColor;
    VertexGradient defaultGradient;
    

    Color effectColor;
    VertexGradient effectGradient;
    float effectEndTime;

    void Start()
    {
        playerHealth = this;
        healthText = GetComponentInChildren<TextMeshProUGUI>();
        defaultColor = healthText.color;
        defaultGradient = healthText.colorGradient;
        effectColor = defaultColor;
        effectGradient = defaultGradient;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        if(Time.time <= effectEndTime)
        {
            healthText.color = effectColor;
            healthText.colorGradient = effectGradient;
        }else if(healthText.color != defaultColor)
        {
            healthText.color = defaultColor;
            healthText.colorGradient = defaultGradient;
        }
    }

    public void setHealth(float newHealth)
    {
        health = (int)newHealth;
    }

    public void damageUpdateHealth(float newHealth)
    {
        health = (int)newHealth;
        effectColor = Color.red;
        effectGradient = new VertexGradient(effectColor);
        effectEndTime = Time.time + effectTimeOut;
    }

    public void healUpdateHealth(float newHealth)
    {
        health = (int)newHealth;
        effectColor = Color.blue;
        effectGradient = new VertexGradient(effectColor);
        effectEndTime = Time.time + effectTimeOut;
    }
}
