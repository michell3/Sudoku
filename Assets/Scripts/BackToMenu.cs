using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour {

	public KeyCode key;

	public Button myButton;


	void Awake() {
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener (trigger);
	}

	/*
	void Update(){

		if (Input.GetKeyDown (KeyCode.Space)) {
			myButton.onClick.AddListener (trigger);
		}

	}*/



	// Update is called once per frame


	void trigger(){
		SceneManager.LoadScene ("Splash_Screen");
	}
		



}
