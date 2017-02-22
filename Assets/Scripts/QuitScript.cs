using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitScript : MonoBehaviour {
	public Button yourButton;
	public string nextScene;
	//public SpriteState sprState = new SpriteState();

	void Start () {
		Button btn = yourButton.GetComponent<Button> ();
		btn.onClick.AddListener (update);
	}
	
	// Update is called once per frame
	void update () {
		Application.Quit ();
	}
}
