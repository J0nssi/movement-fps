using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    public Transform lookDirection;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = lookDirection.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = lookDirection.rotation;
    }
}
