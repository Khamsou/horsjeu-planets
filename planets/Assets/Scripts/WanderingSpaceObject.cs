using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingSpaceObject : MonoBehaviour {

	[Header("References")]
	private Rigidbody rb;

	[Header("Variables")]
	[SerializeField] private Vector3 minLimit;
	[SerializeField] private Vector3 maxLimit;
	private Vector3 originPoint;
	[SerializeField] private float force;
	private Vector3 targetDirection;
	private float distanceCheck = 700f;
	private bool isInTheZone = false;


	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody>();
		originPoint = maxLimit - minLimit;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isInTheZone)
		{
			FindNextDestination();
		}

		// Si l'objet sort de la zone de jeu
		if (transform.position.x < minLimit.x || transform.position.x > maxLimit.x ||
			transform.position.y < minLimit.y || transform.position.y > maxLimit.y ||
			transform.position.z < minLimit.z || transform.position.z > maxLimit.z)
		{
			isInTheZone = false;
		}

		transform.rotation = Quaternion.LookRotation(targetDirection);
	}

	void FixedUpdate ()
	{
		rb.AddForce(targetDirection * force, ForceMode.Force);

		if (!isInTheZone)
		{
			Vector3 originDirection = originPoint - transform.position;

			rb.AddForce(originDirection.normalized);
		}
	}

	private void FindNextDestination ()
	{
		Vector3 randomDirection = Random.insideUnitSphere;

		Ray ray = new Ray(transform.position, randomDirection);

		if (!Physics.Raycast(ray, distanceCheck))
		{
			targetDirection = randomDirection;
			isInTheZone = true;
		}
	}

}
