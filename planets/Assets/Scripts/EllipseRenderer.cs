using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Ellipse ellipse;
	private LineRenderer lr;

	[Header("Variables")]
	[Range(3,36)]
	[SerializeField] private int segments;

	// Use this for initialization
	void Awake ()
	{
		lr = GetComponent<LineRenderer>();
		CalculateEllipse();
	}
	
	// Update is called once per frame
	private void CalculateEllipse ()
	{
		Vector3[] points = new Vector3[segments + 1];
		
		for (int i=0; i < segments; i++)
		{
			// Détermine un angle en fonction du pourcentage du parcours du tableau
			Vector2 position2D = ellipse.Evaluate((float)i / (float)segments);
			points[i] = new Vector3(position2D.x, transform.position.y, position2D.y);
		}
		// On ferme la boucle
		points[segments] = points[0];

		// Nombre de points à positionner sur la ligne
		lr.positionCount = segments + 1;
		// On place les points
		lr.SetPositions(points);
	}

	void OnValidate ()
	{
		if (Application.isPlaying && lr !=null)
		{
			CalculateEllipse();
		}
	}
}
