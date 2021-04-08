using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    // Character components
    private CharacterController controller;
    private EnemyWeaponsController weaponsController;
    private NavMeshAgent agent;
    private Animator anim;
    private IDamageable character;
    [SerializeReference]
    private EnemyVision vision;

    // Character movement properties
    //public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float fallDamageHeight = 10f;

    // Ground check
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Character jump movement variables
    Vector3 velocity;
    bool isGrounded;
    float stepOffset;

    // -- ENEMY AI --
    public Transform destination;

    public float waitBeforeMoving = 0.5f;

    float nextMoveTime;

    // Patrolling
    Vector3 walkpoint;
    bool walkPointSet = false;
    public float walkpointRange;

    // Attacking
    public float firingCooldown;
    bool alreadyAttacked;

    // Start is called before the first frame update
    void Start()
    {
        nextMoveTime = Time.time + waitBeforeMoving;
        controller = GetComponent<CharacterController>();
        character = GetComponent<IDamageable>();
        weaponsController = GetComponentInChildren<EnemyWeaponsController>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stepOffset = controller.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if character is touching ground and update animation controller accoringly
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        anim.SetBool("isGrounded", isGrounded);

        //If character touching ground
        if (isGrounded && velocity.y < 0)
        {
            // Return stepOffset of character controller to default value
            controller.stepOffset = stepOffset;

            FallDamageHandler();
            velocity.y = -2f;
        }
        else if (isGrounded)
        {
            // Return stepOffset of character controller to default value
            controller.stepOffset = stepOffset;
        }
        else if (!isGrounded)
        {
            // Set stepOffset to 0 to prevent edge glitching while jumping
            controller.stepOffset = 0;
        }

        if (!vision.DetectedCharacter() && vision.VisitedLastSeen() && nextMoveTime <= Time.time) Patrolling();
        if (!vision.DetectedCharacter() && !vision.VisitedLastSeen() && nextMoveTime <= Time.time) Searching();
        if (vision.DetectedCharacter()) Attacking();


        

        // -- Update animation controller variables --
        UpdateAnimation();
    }

    void Patrolling()
    {
        weaponsController.firing = false;
        Debug.Log("Patrolling, walkpointSet: " + walkPointSet + "\nValidDestination:" + ValidDestination(walkpoint));
        destination.position = walkpoint;
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
        {
            agent.SetDestination(walkpoint);
            nextMoveTime = Time.time + waitBeforeMoving;
        }

        Vector3 distanceToWalkpoint = transform.position - walkpoint;

        if(distanceToWalkpoint.magnitude < 3f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        Debug.Log("Searching for walk point");
        //Random point in range
        float randomZ = Random.Range(-walkpointRange, walkpointRange);
        float randomX = Random.Range(-walkpointRange, walkpointRange);

        Vector3 newWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (ValidDestination(newWalkPoint))
        {
            walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            walkPointSet = true;
        }
    }
    void Attacking()
    {
        agent.SetDestination(transform.position);
        if(vision.DetectedCharacter())
        {
            weaponsController.firing = true;
        } else
		{
            weaponsController.firing = false;
        }
    }

    void Searching()
    {
        weaponsController.firing = false;
        Debug.Log("Searching");
        if (ValidDestination(vision.LastSeenLocation()))
        {
            walkpoint = vision.LastSeenLocation();
            walkPointSet = true;
        }
        Patrolling();
    }

    bool ValidDestination(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(destination, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        return false;
    }

    void FallDamageHandler()
    {
        // Calulate fall height and apply falldamage.
        float fallHeight = Mathf.Pow(velocity.y, 2) / -2f / gravity;
        if (fallHeight > fallDamageHeight)
        {
            int damage = (int)((fallHeight - fallDamageHeight) * 20);
            Debug.Log("Damaged player for: " + damage);
            character.Damage(damage);
        }
    }

    // UpdateAnimation()
    void UpdateAnimation()
    {
        Vector3 move = agent.desiredVelocity;
        move.y = 0f;
        if (move != Vector3.zero && isGrounded)
        {
            //Run
            anim.SetFloat("Speed", Mathf.Abs(move.x));
        }
        else if (move == Vector3.zero)
        {
            //Idle
            anim.SetFloat("Speed", 0);
        }
    }

    
}
