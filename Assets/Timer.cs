using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Image timerbar;
	public bool coolingDown;
	public float waitTime = 30.0f;

	
	// Update is called once per frame
	void Update () {
		if(coolingDown == true){
			timerbar.fillAmount -= 1.0f / waitTime * Time.deltaTime;
		}
	}
}

