using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Quit: MonoBehaviour {
	public Button yourButton;
	//public SpriteState sprState = new SpriteState();

	void Start () {
		Button btn = yourButton.GetComponent<Button> ();
		btn.onClick.AddListener (update);
	}

	void update(){

		Application.Quit();
	}
}