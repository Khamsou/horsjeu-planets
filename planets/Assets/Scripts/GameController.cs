using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

	[Header("References")]
	public static GameController instance;
	public GameObject[] planets;
	public UIController myCanvas;

	[Header("Variables")]
	public bool gamePaused;

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

		if (myCanvas == null)
		{
			myCanvas = GameObject.FindObjectOfType<UIController>();
		}

		planets = GameObject.FindGameObjectsWithTag("Planet");
	}

	void Update ()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			TogglePauseState();
		}
	}

	public void TogglePauseState ()
	{
		gamePaused = !gamePaused;

		myCanvas.TogglePauseScreenState(gamePaused);
	}

	public void ResetGame ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
