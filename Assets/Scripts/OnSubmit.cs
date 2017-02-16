using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSubmit : MonoBehaviour {

	public Button yourButton;
	public Sprite yourImage;
	public string nextScene;


	void update(){
		yourButton.image.sprite = yourImage;
		SceneManager.LoadScene (nextScene);
		//Debug.Log ("You have clicked the button!");
	}
}
