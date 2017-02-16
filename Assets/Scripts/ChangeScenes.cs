using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour {
	public Button yourButton;
	public Sprite yourImage;
	public string nextScene;
	//public SpriteState sprState = new SpriteState();

	void Start () {
		Button btn = yourButton.GetComponent<Button> ();
		btn.onClick.AddListener (update);






	}

	void update(){
		
		//yourButton.image.sprite = yourImage;
		SceneManager.LoadScene (nextScene);
		//Debug.Log ("You have clicked the button!");
	}
}