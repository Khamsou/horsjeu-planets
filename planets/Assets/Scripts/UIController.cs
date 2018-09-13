using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour {

	private GameObject pauseScreenImage;
	private Button[] menuButtons;
	private int selectedButton;
	private Image playerStateImage;
	private float selectionLag = 0f;

	// Use this for initialization
	void Awake ()
	{
		pauseScreenImage = transform.Find("pauseScreen").gameObject;
		menuButtons = pauseScreenImage.GetComponentsInChildren<Button>();
		playerStateImage = transform.Find("playerState").GetComponent<Image>();
	}

	void Update ()
	{
		if (GameController.instance.gamePaused)
		{
			return;
		}

		MenuSelection();

		// Lag pour éviter que l'on défile trop rapidement dans le menu
		if (selectionLag > 0)
		{			
			selectionLag -= Time.deltaTime;
		}
	}

	public void ChangePlayerStateImage (Color c)
	{
		playerStateImage.color = c;
	}

	public void TogglePauseScreenState (bool gamePaused)
	{
		pauseScreenImage.SetActive(gamePaused);
		playerStateImage.gameObject.SetActive(!gamePaused);

		if (gamePaused)
		{
			// Sélectionne la première option du menu
			selectedButton = 0;
			menuButtons[selectedButton].Select();
		}
	}

	private void MenuSelection()
	{
		if (selectionLag < 0 && Input.GetAxisRaw("Horizontal") != 0)
		{
			selectedButton += (int) Input.GetAxisRaw("Horizontal");
			selectedButton = Mathf.Clamp(selectedButton, 0, 2);
			menuButtons[selectedButton].Select();
			selectionLag = 0.5f;
		}
	}
}
