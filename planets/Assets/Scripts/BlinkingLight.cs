using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour {

	private Light myLight;
	[SerializeField] private float tempo;
	[SerializeField] private bool isBlinking;
	[SerializeField] private bool isOn;

	// Use this for initialization
	void Awake ()
	{
		myLight = GetComponent<Light>();

		// On annule la variable pour ne pas gêner la coroutine
		isOn = !isOn;
		StartCoroutine(BlinkLight ());
	}
	
	IEnumerator BlinkLight()
	{
		// On inverse le booléen et allumons la lumière en fonction

		while (isBlinking)
		{
			isOn = !isOn;
			myLight.enabled = isOn;

			yield return new WaitForSeconds(tempo);
		}
	}
}
