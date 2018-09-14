using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandCollision : MonoBehaviour {

	[SerializeField] private GameObject rastaPlanet;
	private ParticleSystem[] particles;

	// Use this for initialization
	void Awake ()
	{
		particles = transform.GetComponentsInChildren<ParticleSystem>();

		if (rastaPlanet == null)
		{
			rastaPlanet = GameObject.Find("Rasta");
		}
	}

	void Update ()
	{	
        float currentDistance = Vector3.Distance(transform.position, rastaPlanet.transform.position);

		if (currentDistance < (rastaPlanet.transform.localScale.x / 2) + (transform.localScale.x / 2))
		{
			foreach( ParticleSystem p in particles)
			{
				p.Emit(1);
				Vector3 gravityUp = (rastaPlanet.transform.position - transform.position).normalized;

				// Rotation en fonction du centre de la planète
				Quaternion wantedRotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
				 
				//then rotate
				p.transform.rotation = wantedRotation;
			}
		}
	}
}
