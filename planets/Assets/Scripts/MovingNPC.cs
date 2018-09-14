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
