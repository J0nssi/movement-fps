using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public bool automatic = false;
    public float damage = 25f;
    public float fireRate = 5f;
    [Range(0f,100f)]
    public float sideRecoil = 1f;
    [Range(0f, 100f)]
    public float upRecoil = 1f;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public Transform leftHandGrip;
    public Transform rightHandGrip;

    public MouseLook mouseLook;

    Camera fpsCam;

    AudioSource gunsound;
    Animator anim;

    Transform leftArmTarget;
    Transform rightArmTarget;


    private float nextTimeToFire = 0f;

    void Start()
    {
        fpsCam = Camera.main;
        anim = GetComponent<Animator>();
        gunsound = GetComponent<AudioSource>();

        //Get constraint targets
        leftArmTarget = GameObject.Find("LeftArmTarget").transform;
        rightArmTarget = GameObject.Find("RightArmTarget").transform;

        //Place hands correctly
        leftArmTarget.position = leftHandGrip.position;
        leftArmTarget.rotation = leftHandGrip.rotation;
        rightArmTarget.position = rightHandGrip.position;
        rightArmTarget.rotation = rightHandGrip.rotation;
    }
    //Once Obect is Enabled
    private void OnEnable()
    {
        //Get constraint targets
        leftArmTarget = GameObject.Find("LeftArmTarget").transform;
        rightArmTarget = GameObject.Find("RightArmTarget").transform;

        //Place hands correctly
        leftArmTarget.position = leftHandGrip.position;
        leftArmTarget.rotation = leftHandGrip.rotation;
        rightArmTarget.position = rightHandGrip.position;
        rightArmTarget.rotation = rightHandGrip.rotation;
    }
    // Update is called once per frame
    void Update()
    {

        //Place hands correctly
        leftArmTarget.position = leftHandGrip.position;
        leftArmTarget.rotation = leftHandGrip.rotation;
        rightArmTarget.position = rightHandGrip.position;
        rightArmTarget.rotation = rightHandGrip.rotation;

        if ((automatic && Input.GetButton("Fire1") && Time.time >= nextTimeToFire) ||
            (!automatic && Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire))
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            float side = UnityEngine.Random.Range(-sideRecoil, sideRecoil);
            mouseLook.AddRecoil(upRecoil / 1000, side / 1000);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            mouseLook.AddRecoil(0, 0);
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        anim.SetTrigger("FiringTrigger");
        gunsound.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }


}
