using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class OrbitMotion : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject planetOrbitingAround;
	[SerializeField] private Ellipse orbitPath;
	private LineRenderer lr;

	[Header("Variables")]
	[SerializeField] private Vector3 orbitOffset;
	[Range(0f,1f)]
	[SerializeField] private float orbitProgress = 0f;
	[SerializeField] private float orbitPeriod = 3f;
	[SerializeField] private bool orbitActive = true;
	[Range(3,36)]
	[SerializeField] private int RendererSegmentCount;

	// Use this for initialization
	void Awake ()
	{
		if (planetOrbitingAround == null)
		{
			orbitActive = false;
			return;
		}
		lr = GetComponent<LineRenderer>();
		CalculateEllipse();
		SetOrbitingObjectPosition();
		StartCoroutine(AnimateOrbit ());
	}
	
	void CalculateEllipse ()
	{
		Vector3[] points = new Vector3[RendererSegmentCount + 1];

		for (int i=0; i < RendererSegmentCount; i++)
		{
			float t = (float)i / (float)RendererSegmentCount;

			// Détermine un angle en fonction du pourcentage du parcours du tableau
			Vector3 position3D = orbitPath.Evaluate(t);

			points[i] = planetOrbitingAround.transform.position + position3D - orbitOffset;
		}
		// On ferme la boucle
		points[RendererSegmentCount] = points[0];

		// Nombre de points à positionner sur la ligne
		lr.positionCount = RendererSegmentCount + 1;
		// On place les points
		lr.SetPositions(points);
	}

	void SetOrbitingObjectPosition ()
	{
		Vector3 orbitPos = orbitPath.Evaluate(orbitProgress);
		transform.position = planetOrbitingAround.transform.position + orbitPos - orbitOffset;	
	}

	IEnumerator AnimateOrbit ()
	{
		if (orbitPeriod < 0.1f)
		{
			orbitPeriod = 0.1f;
		}

		float orbitSpeed = 1f / orbitPeriod;

		while (orbitActive)
		{
			orbitProgress += Time.deltaTime * orbitSpeed;
			orbitProgress %= 1f;
			SetOrbitingObjectPosition();
			yield return null;
		}
	}
}
