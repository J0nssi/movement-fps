using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAmmo : MonoBehaviour
{

    public static UIAmmo playerAmmo;

    int ammo;
    TextMeshProUGUI ammoText;
    Color defaultColor;
    VertexGradient defaultGradient;

    Color effectColor;
    VertexGradient effectGradient;

    // Start is called before the first frame update
    void Start()
    {
        playerAmmo = this;
        ammoText = GetComponentInChildren<TextMeshProUGUI>();
        defaultColor = ammoText.color;
        defaultGradient = ammoText.colorGradient;
        effectColor = defaultColor;
        effectGradient = defaultGradient;
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = ammo.ToString();
    }

    public void setAmmo(float newAmmo)
    {
        ammo = (int)newAmmo;
    }
}
