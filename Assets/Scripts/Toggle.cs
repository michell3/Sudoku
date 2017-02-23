using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour {

	public Button yourButton;
	public UnityEngine.UI.Toggle yourToggle;
	//public SpriteState sprState = new SpriteState();

	void Start () {
		Button btn = yourButton.GetComponent<Button> ();
		btn.onClick.AddListener (update);
		yourToggle.isOn = true;
	}

	void update(){
		Debug.Log ("kkkkk");
		if (yourToggle.isOn == true)
			yourToggle.isOn = false;
		else
			yourToggle.isOn = true;

		//yourButton.image.sprite = yourImage;
		//SceneManager.LoadScene (nextScene);
		//Debug.Log ("You have clicked the button!");
	}
}
