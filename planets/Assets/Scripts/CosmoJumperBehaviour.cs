using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmoJumperBehaviour : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] private float cosmoJumperForce = 50f;
	private Vector3 direction;

	// Use this for initialization
	void Awake ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		direction = target.position - transform.position;
		Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = newRotation;
	}

	void OnTriggerEnter (Collider c)
	{
		if (c.tag == "Player")
		{
			c.attachedRigidbody.AddForce(direction.normalized * cosmoJumperForce, ForceMode.Impulse);
		}
	}
}
