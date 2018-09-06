using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour {

	[Header("Variables")]
	[SerializeField] private float gravity = -4f;
	[SerializeField] private float rotationSpeed = 0.1f;

	void Update ()
	{
		// Rotation personnelle
		transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
	}

	public void Attract(Rigidbody body)
	{
		if (body.transform.parent != transform)
		{
			body.transform.parent = transform;
		}

		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;

		// Gravité
		body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);

		// Rotation en fonction du centre de la planète
		Quaternion wantedRotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
		 
		//then rotate
		body.rotation = Quaternion.Lerp(body.rotation, wantedRotation, Time.deltaTime);
	}
}
