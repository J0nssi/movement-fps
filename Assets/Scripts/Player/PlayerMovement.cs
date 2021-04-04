using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float fallDamageHeight = 10f;
    public bool crouching = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    float stepOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        stepOffset = controller.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            controller.stepOffset = stepOffset;
            float fallHeight = Mathf.Pow(velocity.y, 2) / -2f / gravity;
            if (fallHeight > fallDamageHeight)
            {
                int damage = (int)((fallHeight - fallDamageHeight) * 20);
                Debug.Log("Damaged player for: " + damage);
                PlayerHealth.singleton.Damage(damage);
            }
            velocity.y = -2f;
        }
        else if (isGrounded)
        {
            controller.stepOffset = stepOffset;
        }
        else if (!isGrounded)
        {
            controller.stepOffset = 0;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;

        if(move != Vector3.zero && isGrounded)
        {
            //Run
            anim.SetFloat("Speed", z);
        }
        else if(move == Vector3.zero)
        {
            //Idle
            anim.SetFloat("Speed", 0);
        }
        
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
