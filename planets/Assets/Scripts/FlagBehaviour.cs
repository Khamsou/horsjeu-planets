using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehaviour : MonoBehaviour {

	private GravityBody gravityScript;

	// Use this for initialization
	void Awake ()
	{
		gravityScript = GetComponent<GravityBody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void TakeFlag ()
	{

	}
}
