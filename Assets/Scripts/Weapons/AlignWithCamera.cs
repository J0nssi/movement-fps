using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    Camera fpsCam;
    // Start is called before the first frame update
    void Start()
    {
        fpsCam = Camera.main;
        transform.rotation = fpsCam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = fpsCam.transform.rotation;
    }
}
