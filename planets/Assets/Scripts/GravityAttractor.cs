using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour {

	public float gravity = -4f;

	public void Attract(Rigidbody body)
	{
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;

		// Gravité
		body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
		// Rotation en fonction du centre de la planète
		body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
	}
}
