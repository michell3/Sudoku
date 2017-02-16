using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour {
	public Button yourButton;
	public Sprite yourImage;
	public string nextScene;

	void Start () {
		//yourImage = Resources.Load<Sprite>("BTNS");
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

	}

	void TaskOnClick(){
		yourButton.image.sprite = yourImage;
		SceneManager.LoadScene (nextScene);
		//Debug.Log ("You have clicked the button!");
	}
}