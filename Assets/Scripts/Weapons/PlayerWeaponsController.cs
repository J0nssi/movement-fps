using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeaponsController : MonoBehaviour
{
    public int activeWeaponIndex = 0;
    public Transform leftArmTarget;
    public Transform rightArmTarget;
    public Transform playerCam;
    public GameObject recoilHandlerObject;
    IRecoilHandler recoilHandler;
    List<GameObject> weapons = new List<GameObject>();
    Weapon equippedWeapon;
    public TextMeshProUGUI ammoText;

    // Start is called before the first frame update
    void Start()
    {
        ammoText = GameObject.Find("ammoText (TMP)").GetComponent<TextMeshProUGUI>(); ;
        recoilHandler = recoilHandlerObject.GetComponent<IRecoilHandler>();
        foreach (Transform child in transform)
        {
            
            GameObject obj = child.gameObject;
            obj.SetActive(false);
            weapons.Add(child.gameObject);
        }
        EquipWeapon(activeWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        equippedWeapon.holdingFire = Input.GetButton("Fire1");

        // Check scrollwheel for updates
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            changeActiveWeapon(activeWeaponIndex - 1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            changeActiveWeapon(activeWeaponIndex + 1);
        }

        ammoText.text = equippedWeapon.magazine + "/" + equippedWeapon.ammo;
    }

    void changeActiveWeapon(int index)
    {
        weapons[activeWeaponIndex].SetActive(false);
        
        if(index >= 0 && index < weapons.Count)
        {
            EquipWeapon(index);
            activeWeaponIndex = index;
        } else if (index >= weapons.Count)
        {
            EquipWeapon(0);
            activeWeaponIndex = 0;
        }
        else
        {
            EquipWeapon(weapons.Count - 1);
            activeWeaponIndex = weapons.Count - 1;
        }
    }

    void EquipWeapon(int index)
    {
        GameObject obj = weapons[index];
        Weapon weapon = obj.GetComponent<Weapon>();
        weapon.leftArmTarget = leftArmTarget;
        weapon.rightArmTarget = rightArmTarget;
        weapon.raycastOrigin = playerCam.transform;
        weapon.recoilHandler = recoilHandler;
        equippedWeapon = weapon;
        obj.SetActive(true);
    }

    void dropWeapon()
    {

    }
    void pickUpWeapon()
    {

    }
}
