using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNPC : MonoBehaviour {

	[Header("References")]
	private Rigidbody rb;
	private Collider myCollider;

	[Header("Variables")]
	[SerializeField] private float speed;
	[SerializeField] private LayerMask groundedMask;
	private bool grounded;

	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		float step = speed * Time.fixedDeltaTime;

		if (grounded)
		{		
			rb.AddForce(transform.forward * step);
		}

		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, myCollider.bounds.size.y*2, groundedMask))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}

	}

}
