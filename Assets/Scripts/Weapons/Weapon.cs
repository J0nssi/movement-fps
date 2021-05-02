using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update

    // --SAA PYSYÄ WEAPONIN SISÄLLÄ
    public bool automatic = false;
    public float damage = 25f;
    public float fireRate = 5f;
    [Range(0f, 100f)]
    public float sideRecoil = 1f;
    [Range(0f, 100f)]
    public float upRecoil = 1f;

    //Reloading
    public int maxGunMagSize = 30;
    //public int currentAmmo;
    public int magazine = 30, ammo, mags = 4;
    public float reloadTime = 2f;
    private bool isReloading = false;

    public WeaponAudioManager weaponAudioManager;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject characterImpactEffect;
    public Transform leftHandGrip;
    public Transform rightHandGrip;

    public GameObject RecoilHandlerObject;

    public string soundName;
    Animator anim;
    // --SAA PYSYÄ WEAPONIN SISÄLLÄ

    // --ASETETAAN WEAPONCONTROLLERISSA
    [HideInInspector]
    public bool holdingFire;
    [HideInInspector]
    public Transform leftArmTarget;
    [HideInInspector]
    public Transform rightArmTarget;

    [HideInInspector]
    public Transform raycastOrigin;
    [HideInInspector]
    public IRecoilHandler recoilHandler;
    // --ASETETAAN WEAPONCONTROLLERISSA

    private float nextTimeToFire = 0f;
    private bool triggerPulled = false;

    void Start()
    {
        recoilHandler = RecoilHandlerObject.GetComponent<IRecoilHandler>();
        anim = GetComponent<Animator>();
        //PlaceHands();
        // toimii currentAmmo = maxAmmo;
        ammo = magazine * mags;
    }

    //Once Object is Enabled
    private void OnEnable()
    {
        isReloading = false;
        PlaceHands();
    }
    // Update is called once per frame
    void Update()
    {
        PlaceHands();
        if (isReloading)
        {
            triggerPulled = false;
            recoilHandler.ResetRecoil();
            return;
        }

        if (magazine <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        else if(magazine < maxGunMagSize && Input.GetKeyDown(KeyCode.R) && gameObject.transform.parent.name == "PlayerWeaponsController")
        {
            StartCoroutine(Reload());
            return;
        }

        if (holdingFire && !triggerPulled && magazine > 0)
        {
            FireWeapon();
        }
        else if(!holdingFire)
        {
            triggerPulled = false;
            recoilHandler.ResetRecoil();
        }
        else if (triggerPulled)
        {
            recoilHandler.ResetRecoil();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo -= maxGunMagSize - magazine;
        magazine = maxGunMagSize;
        if(ammo < 0)
        {
            magazine += ammo;
            ammo = 0;
        }
        isReloading = false;
    }

    void Shoot()
    {
        muzzleFlash.Play();
        anim.SetTrigger("FiringTrigger");
        weaponAudioManager.Play(soundName);
        //currentAmmo--;
        magazine--;

        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit))
        {
            GameObject hitImpactEffect;
            IDamageable target;

            if(hit.transform.TryGetComponent<IDamageable>(out target))
            {
                bool killed = target.Damage(damage);
                UnityEngine.Debug.Log(hit.transform.name + " damaged for: " + damage);
                if (killed) MatchManager.i.AddFrag(transform.parent.parent.name);
            }

            // Different impact effect for enemy and environment;
            if(hit.transform.tag == "Enemy" || hit.transform.tag == "Player")
            {
                hitImpactEffect = characterImpactEffect;
            }
            else
            {
                hitImpactEffect = impactEffect;
            }
            GameObject impactGO = Instantiate(hitImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }

    public void FireWeapon()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            float side = UnityEngine.Random.Range(-sideRecoil, sideRecoil);
            recoilHandler.AddRecoil(upRecoil / 1000, side / 1000, automatic);
            if (!automatic)
            {
                triggerPulled = true;
            }
        }
    }

    void PlaceHands()
    {
        if(leftArmTarget && leftHandGrip && rightArmTarget && rightHandGrip)
		{
            //Place hands correctly
            leftArmTarget.position = leftHandGrip.position;
            leftArmTarget.rotation = leftHandGrip.rotation;
            rightArmTarget.position = rightHandGrip.position;
            rightArmTarget.rotation = rightHandGrip.rotation;
        }
    }
}
