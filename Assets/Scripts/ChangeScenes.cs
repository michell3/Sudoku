using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ChangeScenes : MonoBehaviour {
	
	public void ChangeToScene (int sceneToChangeTo) {
		SceneManager.LoadScene(sceneToChangeTo);
	}
}
