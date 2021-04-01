using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 25f;
    public float fireRate = 5f;
    
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    
    public Transform leftHandGrip;
    public Transform rightHandGrip;

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

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
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
            Destroy(impactGO, 2f);
        }
    }

}
