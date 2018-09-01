using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	[Header("References")]
	public static GameController instance;
	public GameObject[] planets;

	// Use this for initialization
	void Awake ()
	{
		if (instance == null)
		{
			instance = this;	
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		planets = GameObject.FindGameObjectsWithTag("Planet");
	}
}
