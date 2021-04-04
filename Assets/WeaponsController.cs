using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    List<GameObject> weapons = new List<GameObject>();
    public int activeWeaponIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            GameObject weapon = child.gameObject;
            weapon.SetActive(false);
            weapons.Add(child.gameObject);
        }
        weapons[activeWeaponIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Check scrollwheel for updates
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            changeActiveWeapon(activeWeaponIndex - 1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            changeActiveWeapon(activeWeaponIndex + 1);
        }

    }

    void changeActiveWeapon(int index)
    {
        weapons[activeWeaponIndex].SetActive(false);
        
        if(index > 0 && index < weapons.Count)
        {
            weapons[index].SetActive(true);
            activeWeaponIndex = index;
        } else if (index >= weapons.Count)
        {
            weapons[0].SetActive(true);
            activeWeaponIndex = 0;
        }
        else
        {
            weapons[weapons.Count - 1].SetActive(true);
            activeWeaponIndex = weapons.Count - 1;
        }
    }

    void dropWeapon()
    {

    }
    void pickUpWeapon()
    {

    }
}
