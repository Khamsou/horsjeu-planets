using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ellipse  {

	[SerializeField] private float xAxis;
	[SerializeField] private float yAxis;
	public float xAngle;
	public float yAngle;
	public float zAngle;

	public Ellipse (float xAxis, float yAxis, float xAngle, float yAngle, float zAngle)
	{
		this.xAxis = xAxis;
		this.yAxis = yAxis;
		this.xAngle = xAngle;
		this.yAngle = yAngle;
		this.zAngle = zAngle;
	}

	public Vector3 Evaluate(float t)
	{
		float currentEllipseAngle = Mathf.Deg2Rad * 360f * t;
		Quaternion xRotAngle = Quaternion.AngleAxis(xAngle, Vector3.right);
		Quaternion yRotAngle = Quaternion.AngleAxis(yAngle, Vector3.up);
		Quaternion zRotAngle = Quaternion.AngleAxis(zAngle, Vector3.forward);

		float x = Mathf.Sin(currentEllipseAngle) * xAxis;
		float y = Mathf.Cos(currentEllipseAngle) * yAxis;

		Vector3 newPosition = new Vector3(x,y,0f);
		newPosition = xRotAngle * newPosition;
		newPosition = yRotAngle * newPosition;
		newPosition = zRotAngle * newPosition;

		return newPosition;
	}

}
