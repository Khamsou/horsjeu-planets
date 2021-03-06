﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityBody))]
public class MovingNPC : MonoBehaviour {

	[Header("References")]
	private Rigidbody rb;
	private Collider myCollider;
	private GravityBody myBody;

	[Header("Variables")]
	[SerializeField] private float speed;
	[SerializeField] private LayerMask groundedMask;
	[SerializeField] private errandStyle currentErrand;
	private enum errandStyle {forward, drunk};
	private Vector3 moveDirection;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;
	private float directionLag = 2f;
	private bool grounded;

	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();
		myBody = GetComponent<GravityBody>();
	}
	
	void Update ()
	{
		if (currentErrand == errandStyle.forward)
		{
			moveDirection = Vector3.forward;
		}
		else if (currentErrand == errandStyle.drunk && directionLag < 0f)
		{
			moveDirection = (Vector3) Random.insideUnitCircle.normalized;
			moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);

	        Quaternion rotation = Quaternion.LookRotation(transform.TransformDirection(moveDirection), transform.up);
	        transform.rotation = rotation;

			directionLag = Random.Range(2f, 5f);
		}

		Vector3 targetMoveAmount = moveDirection * speed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);

		directionLag -= Time.deltaTime;
	}

	void FixedUpdate ()
	{
		if (myBody.isFlying)
		{

			float currentDistanceToPlanet = (transform.position - myBody.planetAttractedTo.transform.position).magnitude;
			float subtractDistance = 0f;

			// Si la distance à la planète a changé, on fait la différence puis on l'enlève
			if (myBody.distanceToPlanet != currentDistanceToPlanet)
			{
				subtractDistance = currentDistanceToPlanet - myBody.distanceToPlanet;
				moveAmount.y -= subtractDistance;
			}

			Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;

			rb.MovePosition(rb.position + localMove);
		}
		else
		{
			if (grounded)
			{		
				// rb.AddForce(transform.forward * step);
				Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
				rb.MovePosition(rb.position + localMove);
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

}
