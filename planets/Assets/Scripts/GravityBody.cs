using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	public GameObject planetAttractedTo;
	private GravityAttractor planetScript;
	private Rigidbody rb;

	// Use this for initialization
	void Awake ()
	{
		planetScript = planetAttractedTo.GetComponent<GravityAttractor>();

		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		planetScript.Attract(rb);
	}

	public void ChangePlanetAttractedTo ()
	{
		GameObject newPlanet = GameController.instance.planets[FindClosestPlanet()];

		if (planetAttractedTo != newPlanet)
		{
			planetAttractedTo = newPlanet;
			planetScript = planetAttractedTo.GetComponent<GravityAttractor>();
		}
	}

	private int FindClosestPlanet ()
	{
		// Tableau stockant les distances entre l'objet et les planètes
		float[] distances = new float[GameController.instance.planets.Length];
		float shortestDistance = Mathf.Infinity;
		int closestPlanetIndex = 0;

		for(int i = 0; i < distances.Length; i++)
		{
			// On calcule la distance du joueur avec la planète
			// Puis on retranche l'échelle de la planète pour connaître la distance avec la surface
			float distance = Vector3.Distance(transform.position, GameController.instance.planets[i].transform.position);
			distance = distance - (GameController.instance.planets[i].transform.localScale.x / 2);
			distances[i] = distance;

			// On vérifie quelle distance est la plus courte et on sauvegarde l'index
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				closestPlanetIndex = i;
			}
		}

		return closestPlanetIndex;
	}
}
