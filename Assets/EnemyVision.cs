using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour, IRecoilHandler
{

    // Player's location
    Transform player;

    // Location player was last seen at
    Vector3 lastSeenLocation;
    bool visited = true;

    // Boolean for storing weather the enemy can see the player or not
    bool isTargetingPlayer = false;

    // Enemy vision properties
    public Transform enemyBody;
    [Range(0f,180f)]
    public float viewAngle = 90f;
    [Range(0f, 100f)]
    public float detectionDistance = 20f;
    [Range(0f, 10f)]
    public float facePlayerDamping = 1f;


    //Recoil
    float sideRecoil = 0f;
    float upRecoil = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Find player
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        // Player's direction and angle from forward vector 
        Vector3 playerDirection = player.position - transform.position;
        float angleOfPlayer = Vector3.Angle(playerDirection, transform.forward);
        // Shoot a raycast from enemy's location and see if it hits player within the detection distance.
        RaycastHit hit;
        Physics.Raycast(transform.position, playerDirection,out hit, detectionDistance);
        if(angleOfPlayer < viewAngle && hit.transform != null && hit.transform.name == "Player")
        {
            // If it hits player, turn the characters body horizontally and vision verticallly to face the player
            isTargetingPlayer = true;

            lastSeenLocation = player.position;
            visited = false;

            float verticalLookRotation = Quaternion.LookRotation(playerDirection).eulerAngles.x;
            playerDirection.y = 0f;
            Quaternion horizontalLookRotation = Quaternion.LookRotation(playerDirection);

            enemyBody.rotation = Quaternion.Slerp(enemyBody.rotation, horizontalLookRotation, Time.deltaTime * facePlayerDamping);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(verticalLookRotation, 0f, 0f), Time.deltaTime * facePlayerDamping);
        }else
        {
            if(lastSeenLocation != null && (lastSeenLocation - transform.position).magnitude < 1f)
            {
                visited = true;
            }
            isTargetingPlayer = false;
            //If it doesn't hit player, face the camera forward and save the location player was last seen in.
            transform.forward = enemyBody.forward;
        }
    }

    public void AddRecoil(float up, float side, bool auto)
    {
        sideRecoil = side;
        upRecoil = up;
    }

    public void ResetRecoil()
    {
        sideRecoil = 0;
        upRecoil = 0;
    }

    public bool IsTargetingPlayer()
    {
        return isTargetingPlayer;
    }

    public Vector3 LastSeenLocation() {
        return lastSeenLocation;
    }

    public bool VisitedLastSeen()
    {
        return visited;
    }

}
