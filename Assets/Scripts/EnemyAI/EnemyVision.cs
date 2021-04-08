using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour, IRecoilHandler
{
	// Enemy's own collider and detectionCollider
	Collider ownCollider;
	SphereCollider detectionCollider;
	// Location player was last seen at
	Vector3 lastSeenLocation;
	bool visited = true;

	// Character's gameobject for checking weather it was killed or not
	GameObject detectedCharacter;

	//Closest character and distance to it
	GameObject closestCharacter = null;
	float distanceToClosest = float.MaxValue;
	// Boolean for storing weather the enemy can see the player or not
	bool IsDetectingCharacter = false;

	// Enemy vision properties
	public Transform enemyBody;
	[Range(0f, 180f)]
	public float viewAngle = 90f;
	[Range(0f, 100f)]
	public float hearingDistance = 10f;
	[Range(0f, 10f)]
	public float facePlayerDamping = 1f;

	float seeingDistance = 35f;

	//Recoil
	float sideRecoil = 0f;
	float upRecoil = 0f;

	// Start is called before the first frame update
	void Start()
	{
		ownCollider = enemyBody.gameObject.GetComponent<Collider>();
		detectionCollider = this.GetComponent<SphereCollider>();
		seeingDistance = detectionCollider.radius;
		Physics.IgnoreCollision(ownCollider, detectionCollider, true);
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log("Closest character: " + closestCharacter);
		if (closestCharacter == null)
			IsDetectingCharacter = false;
		
	}


	private void OnTriggerStay(Collider other)
	{
		
		// Check if it is another character
		if (other.tag == "Player" || other.tag == "Enemy")
		{
			detectedCharacter = other.gameObject;

			Transform characterTransform = detectedCharacter.transform;

			// Character's direction and angle from forward vector 
			Vector3 characterDirection = characterTransform.position - transform.position;
			float characterAngle = Vector3.Angle(characterDirection, transform.forward);

			// Shoot a raycast from enemy's location and see if it hits character within the detection distance.
			RaycastHit hit;
			if (Physics.Raycast(transform.position, characterDirection, out hit, seeingDistance) && 
				(characterAngle < viewAngle || characterDirection.magnitude < hearingDistance) && 
				 (characterDirection.magnitude < distanceToClosest || (detectedCharacter == closestCharacter || characterDirection.magnitude <= distanceToClosest))  &&
				(hit.transform.tag == "Player" || hit.transform.tag == "Enemy"))
			{

				//New closest character
				closestCharacter = detectedCharacter;
				distanceToClosest = characterDirection.magnitude;

				Debug.Log("Detected character: " + hit.transform.tag);
				// If it hits player, turn the characters body horizontally and vision verticallly to face the player
				IsDetectingCharacter = true;

				lastSeenLocation = closestCharacter.transform.position;
				visited = false;

				faceDirection(characterDirection);


			} else
			{
				if (lastSeenLocation != null && (lastSeenLocation - transform.position).magnitude < 1f)
				{
					visited = true;
				}
				IsDetectingCharacter = false;
				//If it doesn't hit player, face the camera forward and save the location player was last seen in.
				transform.forward = enemyBody.forward;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		IsDetectingCharacter = false;
	}

	private void faceDirection(Vector3 direction)
	{
		float verticalLookRotation = Quaternion.LookRotation(direction).eulerAngles.x;
		direction.y = 0f;
		Quaternion horizontalLookRotation = Quaternion.LookRotation(direction);

		enemyBody.rotation = Quaternion.Slerp(enemyBody.rotation, horizontalLookRotation, Time.deltaTime * facePlayerDamping);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(verticalLookRotation, 0f, 0f), Time.deltaTime * facePlayerDamping);
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

	public bool DetectedCharacter()
	{
		return IsDetectingCharacter;
	}

	public Vector3 LastSeenLocation()
	{
		return lastSeenLocation;
	}

	public bool VisitedLastSeen()
	{
		return visited;
	}

}
