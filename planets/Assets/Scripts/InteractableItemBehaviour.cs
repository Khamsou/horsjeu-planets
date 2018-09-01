using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class InteractableItemBehaviour : MonoBehaviour
{
	[Header("References")]
	private Rigidbody rb;

	[Header("Variables")]
	[SerializeField] private float force = 5f;

	void Awake ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			foreach (ContactPoint contact in collision.contacts)
	        {
	            // Vecteur légèrement en hauteur
	            Vector3 shootDirection = (contact.normal + transform.up).normalized;
	            rb.AddForce(shootDirection * force, ForceMode.Impulse);
	        }
		}
	}
}
