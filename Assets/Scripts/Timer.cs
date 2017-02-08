using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Image image; // timer
	public float timeSpeed = 30.0f; // to adjust the speed of countdown timer
	public bool increment;
	public bool decrement;

	private bool powerup = false; // use to see if powerup bar is filled


	
	// Update is called once per frame
	void Update () {
		if(decrement == true){
			image.fillAmount -= 1.0f / timeSpeed * Time.deltaTime;
		}
	}
}

